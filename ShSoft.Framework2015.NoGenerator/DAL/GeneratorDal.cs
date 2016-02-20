using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using System.Text;
using ShSoft.Framework2015.Common.PoweredByLee;
using ShSoft.Framework2015.NoGenerator.Model;

namespace ShSoft.Framework2015.NoGenerator.DAL
{
    /// <summary>
    /// 编号生成器数据访问类
    /// </summary>
    internal sealed class GeneratorDal
    {
        #region # 常量、字段及构造器

        /// <summary>
        /// 默认连接字符串名称AppSetting键
        /// </summary>
        private const string DefaultConnectionStringAppSettingKey = "NoConnection";

        /// <summary>
        /// SQL工具
        /// </summary>
        private static readonly SqlHelper _SqlHelper;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static GeneratorDal()
        {
            //初始化连接字符串
            string defaultConnectionStringName = ConfigurationManager.AppSettings[DefaultConnectionStringAppSettingKey];
            string connectionString = ConfigurationManager.ConnectionStrings[defaultConnectionStringName].ConnectionString;

            //初始化SQL工具
            _SqlHelper = new SqlHelper(connectionString);
        }

        /// <summary>
        /// 构造器
        /// </summary>
        private GeneratorDal()
        {
            //初始化数据表
            this.InitTable();
        }

        #endregion

        #region # 访问器 —— static GeneratorDal CreateInstance()
        /// <summary>
        /// 创建编号生成器数据访问对象
        /// </summary>
        /// <returns>编号生成器数据访问对象</returns>
        public static GeneratorDal CreateInstance()
        {
            GeneratorDal current = CallContext.GetData(typeof(GeneratorDal).Name) as GeneratorDal;
            if (current == null)
            {
                current = new GeneratorDal();
                CallContext.SetData(typeof(GeneratorDal).Name, current);
            }
            return current;
        }
        #endregion

        //Internal

        #region # 获取唯一序列号 —— SerialNumber SingleOrDefault(string prefix...
        /// <summary>
        /// 获取唯一序列号，
        /// 如没有，则返回null
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <param name="formatDate">数据格式</param>
        /// <param name="className">类名</param>
        /// <param name="length">长度</param>
        /// <returns>唯一序列号</returns>
        public SerialNumber SingleOrDefault(string prefix, string formatDate, string className, int length)
        {
            string sql = "SELECT * FROM SerialNumbers WHERE Prefix = @prefix AND FormatDate = @formatDate AND ClassName = @className AND Length = @length";

            SqlParameter[] parameters = {
                new SqlParameter("@prefix", prefix),
                new SqlParameter("@formatDate", formatDate),
                new SqlParameter("@className", className),
                new SqlParameter("@length", length)
            };

            using (SqlDataReader reader = _SqlHelper.ExecuteReader(sql, parameters))
            {
                return reader.Read() ? this.ToModel(reader) : null;
            }
        }
        #endregion

        #region # 添加方法 —— SerialNumber RegisterAdd(SerialNumber serialNumber)
        /// <summary>
        /// 添加方法
        /// </summary>
        /// <param name="serialNumber">序列号实例</param>
        /// <returns>序列号实例</returns>
        public SerialNumber RegisterAdd(SerialNumber serialNumber)
        {
            string sql = "INSERT INTO SerialNumbers (Id, Prefix, FormatDate, ClassName, Length, TodayCount, Description)  VALUES (@Id, @Prefix, @FormatDate, @ClassName, @Length, @TodayCount, @Description)";
            SqlParameter[] parameters = {
					new SqlParameter("@Id", this.ToDbValue(serialNumber.Id)),
					new SqlParameter("@Prefix", this.ToDbValue(serialNumber.Prefix)),
					new SqlParameter("@FormatDate", this.ToDbValue(serialNumber.FormatDate)),
					new SqlParameter("@ClassName", this.ToDbValue(serialNumber.ClassName)),
					new SqlParameter("@Length", this.ToDbValue(serialNumber.Length)),
					new SqlParameter("@TodayCount", this.ToDbValue(serialNumber.TodayCount)),
					new SqlParameter("@Description", this.ToDbValue(serialNumber.Description))
				};
            _SqlHelper.ExecuteNonQuery(sql, parameters);
            return serialNumber;
        }
        #endregion

        #region # 修改方法 —— int RegisterSave(SerialNumber serialNumber)
        /// <summary>
        /// 修改方法
        /// </summary>
        /// <param name="serialNumber">序列号实例</param>
        /// <returns>受影响的行数</returns>
        public int RegisterSave(SerialNumber serialNumber)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE SerialNumbers SET Prefix = @Prefix");
            sql.Append(", FormatDate = @FormatDate");
            sql.Append(", ClassName = @ClassName");
            sql.Append(", Length = @Length");
            sql.Append(", TodayCount = @TodayCount");
            sql.Append(", Description = @Description");
            sql.Append(" WHERE Id = @Id");
            SqlParameter[] args = new SqlParameter[] {
				     new SqlParameter("@Id", serialNumber.Id)
					,new SqlParameter("@Prefix", this.ToDbValue(serialNumber.Prefix))
					,new SqlParameter("@FormatDate", this.ToDbValue(serialNumber.FormatDate))
					,new SqlParameter("@ClassName", this.ToDbValue(serialNumber.ClassName))
					,new SqlParameter("@Length", this.ToDbValue(serialNumber.Length))
					,new SqlParameter("@TodayCount", this.ToDbValue(serialNumber.TodayCount))
					,new SqlParameter("@Description", this.ToDbValue(serialNumber.Description))
			};
            return _SqlHelper.ExecuteNonQuery(sql.ToString(), args);
        }
        #endregion

        //Private

        #region # 初始化序列号数据表 —— static void InitTable()
        /// <summary>
        /// 初始化序列号数据表
        /// </summary>
        private void InitTable()
        {
            //构造sql语句
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("IF OBJECT_ID('[dbo].[SerialNumbers]') IS NULL ");
            sqlBuilder.Append("BEGIN ");
            sqlBuilder.Append("CREATE TABLE [dbo].[SerialNumbers] ([Id] [uniqueidentifier] PRIMARY KEY NOT NULL, [Prefix] [NVARCHAR](MAX) NULL, [FormatDate] [NVARCHAR](MAX) NULL, [ClassName] [NVARCHAR](MAX) NULL, [Length] [INT] NOT NULL, [TodayCount] [INT] NOT NULL, [Description] [NVARCHAR](MAX) NULL) ");
            sqlBuilder.Append("END ");

            //执行创建表
            _SqlHelper.ExecuteNonQuery(sqlBuilder.ToString());
        }
        #endregion

        #region # 根据SqlDataReader返回对象 —— SerialNumber ToModel(SqlDataReader reader)
        /// <summary>
        /// 根据SqlDataReader返回对象
        /// </summary>
        /// <param name="reader">SqlDataReader对象</param>
        /// <returns>实体对象</returns>
        private SerialNumber ToModel(SqlDataReader reader)
        {
            SerialNumber serialNumber = new SerialNumber
            {
                Id = (Guid)this.ToModelValue(reader, "Id"),
                Prefix = (string)this.ToModelValue(reader, "Prefix"),
                FormatDate = (string)this.ToModelValue(reader, "FormatDate"),
                ClassName = (string)this.ToModelValue(reader, "ClassName"),
                Length = (int)this.ToModelValue(reader, "Length"),
                TodayCount = (int)this.ToModelValue(reader, "TodayCount"),
                Description = (string)this.ToModelValue(reader, "Description")
            };
            return serialNumber;
        }
        #endregion

        #region # C#值转数据库值空值处理 —— object ToDbValue(object value)
        /// <summary>
        /// C#值转数据库值空值处理
        /// </summary>
        /// <param name="value">C#值</param>
        /// <returns>处理后的数据库值</returns>
        private object ToDbValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return value;
        }

        #endregion

        #region # 数据库值转C#值空值处理 —— object ToModelValue(SqlDataReader reader, string columnName)
        /// <summary>
        /// 数据库值转C#值空值处理
        /// </summary>
        /// <param name="reader">IDataReader对象</param>
        /// <param name="columnName">列名</param>
        /// <returns>C#值</returns>
        private object ToModelValue(SqlDataReader reader, string columnName)
        {
            if (reader.IsDBNull(reader.GetOrdinal(columnName)))
            {
                return null;
            }
            return reader[columnName];
        }

        #endregion
    }
}
