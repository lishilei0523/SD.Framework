using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace ShSoft.Framework2016.Common.PoweredByLee
{
    /// <summary>
    /// Excel读写帮助类
    /// </summary>
    public static class ExcelAssistant
    {
        #region 01.读取Excel并转换为给定类型数组 —— static T[] ReadFile<T>(string path, int sheetIndex, int rowIndex)
        /// <summary>
        /// 读取Excel并转换为给定类型数组
        /// </summary>
        /// <param name="path">读取路径</param>
        /// <param name="sheetIndex">工作簿索引</param>
        /// <param name="rowIndex">行索引</param>
        /// <returns>给定类型数组</returns>
        public static T[] ReadFile<T>(string path, int sheetIndex, int rowIndex)
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException("path", @"文件路径不可为空！");
            }

            if (Path.GetExtension(path) != ".xls")
            {
                throw new ArgumentOutOfRangeException("path", @"文件格式不正确！");
            }

            if (sheetIndex < 0)
            {
                throw new ArgumentOutOfRangeException("sheetIndex", @"工作簿索引不可为负数！");
            }

            if (rowIndex < 0)
            {
                throw new ArgumentOutOfRangeException("rowIndex", @"行索引不可为负数！");
            }

            #endregion

            List<T> list = new List<T>();
            //01.创建文件流
            using (FileStream fsRead = File.OpenRead(path))
            {
                //02.创建工作薄
                IWorkbook workbook = new HSSFWorkbook(fsRead);
                //03.读取给定工作簿

                ISheet sheet = workbook.GetSheetAt(sheetIndex);
                //04.读取sheet中除标题外的每一行

                #region # 验证逻辑

                if (sheetIndex + 1 > workbook.NumberOfSheets)
                {
                    throw new InvalidOperationException("给定工作簿索引超出了Excel有效工作簿数！");
                }

                if (rowIndex > sheet.LastRowNum)
                {
                    throw new InvalidOperationException("给定行索引超出了Excel有效行数！");
                }

                #endregion

                for (int i = rowIndex; i <= sheet.LastRowNum; i++)
                {
                    Type sourceType = typeof(T);
                    T sourceObj = (T)Activator.CreateInstance(sourceType);
                    PropertyInfo[] properties = sourceType.GetProperties();

                    //读取每行并为对象赋值
                    FillInstanceValue(sheet, i, properties, sourceObj);
                    list.Add(sourceObj);
                }
                return list.ToArray();
            }
        }
        #endregion

        #region 02.读取Excel并转换为给定类型数组 —— static T[] ReadFile<T>(string path)
        /// <summary>
        /// 读取Excel并转换为给定类型数组
        /// </summary>
        /// <param name="path">读取路径</param>
        /// <returns>给定类型数组</returns>
        public static T[] ReadFile<T>(string path)
        {
            //默认读取第一张工作簿，第二行
            return ReadFile<T>(path, 0, 1);
        }
        #endregion

        #region 03.将集合写入Excel —— static void WriteFile<T>(IEnumerable<T> list, string path)
        /// <summary>
        /// 将集合写入Excel
        /// </summary>
        /// <param name="list">集合对象</param>
        /// <param name="path">写入路径</param>
        public static void WriteFile<T>(IEnumerable<T> list, string path)
        {
            #region # 验证参数

            if (list == null || !list.Any())
            {
                throw new ArgumentNullException("list", @"源集合不可为空！");
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException("path", @"目标路径不可为空！");
            }

            if (Path.GetExtension(path) != ".xls")
            {
                throw new ArgumentOutOfRangeException("path", @"文件格式不正确！");
            }

            #endregion

            //创建工作簿
            IWorkbook workbook = CreateWorkbook(list);

            //写入文件
            using (FileStream fsWrite = File.OpenWrite(path))
            {
                workbook.Write(fsWrite);
            }
        }
        #endregion

        #region 04.将集合写入Excel，并返回内存流 —— static Stream WriteStream<T>(IEnumerable<T> list)
        /// <summary>
        /// 将集合写入Excel，并返回内存流
        /// </summary>
        /// <param name="list">集合对象</param>
        /// <returns>流</returns>
        public static Stream WriteStream<T>(IEnumerable<T> list)
        {
            #region # 验证参数

            if (list == null || !list.Any())
            {
                throw new ArgumentNullException("list", @"源集合不可为空！");
            }

            #endregion

            IWorkbook workbook = CreateWorkbook(list);

            //写入文件
            Stream stream = new MemoryStream();
            workbook.Write(stream);
            return stream;
        }
        #endregion

        #region 05.创建工作簿 —— static IWorkbook CreateWorkbook<T>(IEnumerable<T> list)
        /// <summary>
        /// 创建工作簿
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">集合对象</param>
        /// <returns>工作簿</returns>
        private static IWorkbook CreateWorkbook<T>(IEnumerable<T> list)
        {
            //01.创建工作簿
            IWorkbook workbook = new HSSFWorkbook();

            //02.创建工作表
            ISheet sheet = workbook.CreateSheet(typeof(T).Name);

            //03.创建标题行
            IRow rowTitle = sheet.CreateRow(0);
            CreateTitleRow(list, rowTitle);

            //04.创建数据行
            CreateDataRows(list, sheet);
            return workbook;
        }
        #endregion

        #region 06.读取每一行，并填充对象属性值 —— static void FillInstanceValue<T>(ISheet sheet, int index...
        /// <summary>
        /// 读取每一行，并填充对象属性值
        /// </summary>
        /// <param name="sheet">工作簿对象</param>
        /// <param name="index">行索引</param>
        /// <param name="properties">对象属性集合</param>
        /// <param name="instance">对象实例</param>
        private static void FillInstanceValue<T>(ISheet sheet, int index, PropertyInfo[] properties, T instance)
        {
            IRow row = sheet.GetRow(index);
            if (properties.Length != row.Cells.Count)
            {
                throw new InvalidOperationException("模型与Excel表格不兼容：第" + (index + 1) + "行 列数不一致！");
            }
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].PropertyType == typeof(double))
                {
                    properties[i].SetValue(instance, Convert.ToDouble(row.GetCell(i).ToString().Trim()));
                }
                else if (properties[i].PropertyType == typeof(float))
                {
                    properties[i].SetValue(instance, Convert.ToSingle(row.GetCell(i).ToString().Trim()));
                }
                else if (properties[i].PropertyType == typeof(decimal))
                {
                    properties[i].SetValue(instance, Convert.ToDecimal(row.GetCell(i).ToString().Trim()));
                }
                else if (properties[i].PropertyType == typeof(byte))
                {
                    properties[i].SetValue(instance, Convert.ToByte(row.GetCell(i).ToString().Trim()));
                }
                else if (properties[i].PropertyType == typeof(short))
                {
                    properties[i].SetValue(instance, Convert.ToInt16(row.GetCell(i).ToString().Trim()));
                }
                else if (properties[i].PropertyType == typeof(int))
                {
                    properties[i].SetValue(instance, Convert.ToInt32(row.GetCell(i).ToString().Trim()));
                }
                else if (properties[i].PropertyType == typeof(long))
                {
                    properties[i].SetValue(instance, Convert.ToInt64(row.GetCell(i).ToString().Trim()));
                }
                else if (properties[i].PropertyType == typeof(bool))
                {
                    properties[i].SetValue(instance, Convert.ToBoolean(row.GetCell(i).ToString().Trim()));
                }
                else if (properties[i].PropertyType == typeof(DateTime))
                {
                    properties[i].SetValue(instance, Convert.ToDateTime(row.GetCell(i).ToString().Trim()));
                }
                else
                {
                    properties[i].SetValue(instance, row.GetCell(i).ToString().Trim());
                }
            }
        }
        #endregion

        #region 07.创建标题行 —— static void CreateTitleRow<T>(IEnumerable<T> list, IRow rowTitle)
        /// <summary>
        /// 创建标题行
        /// </summary>
        /// <param name="list">集合对象</param>
        /// <param name="rowTitle">标题行</param>
        private static void CreateTitleRow<T>(IEnumerable<T> list, IRow rowTitle)
        {
            for (int i = 0; i < list.ToArray().Length; i++)
            {
                T item = list.ToArray()[i];
                Type sourceType = item.GetType();
                PropertyInfo[] properties = sourceType.GetProperties();
                for (int j = 0; j < properties.Length; j++)
                {
                    rowTitle.CreateCell(j).SetCellValue(properties[j].Name);
                }
            }
        }
        #endregion

        #region 08.创建数据行 —— static void CreateDataRows<T>(IEnumerable<T> list, ISheet sheet)
        /// <summary>
        /// 创建数据行
        /// </summary>
        /// <param name="list">集合对象</param>
        /// <param name="sheet">工作簿对象</param>
        private static void CreateDataRows<T>(IEnumerable<T> list, ISheet sheet)
        {
            for (int i = 0; i < list.ToArray().Length; i++)
            {
                T item = list.ToArray()[i];
                Type sourceType = item.GetType();
                PropertyInfo[] properties = sourceType.GetProperties();
                IRow rowData = sheet.CreateRow(i + 1);
                for (int j = 0; j < properties.Length; j++)
                {
                    //在行中创建单元格
                    if (properties[j].PropertyType == typeof(double))
                    {
                        rowData.CreateCell(j).SetCellValue((double)properties[j].GetValue(item));
                    }
                    else if (properties[j].PropertyType == typeof(float))
                    {
                        rowData.CreateCell(j).SetCellValue((float)properties[j].GetValue(item));
                    }
                    else if (properties[j].PropertyType == typeof(decimal))
                    {
                        rowData.CreateCell(j).SetCellValue(Convert.ToDouble(properties[j].GetValue(item)));
                    }
                    else if (properties[j].PropertyType == typeof(byte))
                    {
                        rowData.CreateCell(j).SetCellValue((byte)properties[j].GetValue(item));
                    }
                    else if (properties[j].PropertyType == typeof(short))
                    {
                        rowData.CreateCell(j).SetCellValue((short)properties[j].GetValue(item));
                    }
                    else if (properties[j].PropertyType == typeof(int))
                    {
                        rowData.CreateCell(j).SetCellValue((int)properties[j].GetValue(item));
                    }
                    else if (properties[j].PropertyType == typeof(long))
                    {
                        rowData.CreateCell(j).SetCellValue((long)properties[j].GetValue(item));
                    }
                    else if (properties[j].PropertyType == typeof(bool))
                    {
                        rowData.CreateCell(j).SetCellValue((bool)properties[j].GetValue(item));
                    }
                    else if (properties[j].PropertyType == typeof(DateTime))
                    {
                        rowData.CreateCell(j).SetCellValue(((DateTime)properties[j].GetValue(item)).ToString("yyyy/MM/dd HH:mm:ss"));
                    }
                    else
                    {
                        rowData.CreateCell(j).SetCellValue(properties[j].GetValue(item).ToString());
                    }
                }
            }
        }
        #endregion

        #region 09.创建自定义工作簿
        private static IWorkbook _customerWorkbook = null;

        /// <summary>
        /// 第一步：创建一个空工作簿
        /// </summary>
        /// <returns></returns>
        public static void CreateCustomeWorkbook()
        {
            _customerWorkbook = new HSSFWorkbook();
        }


        /// <summary>
        /// 第二步：为工作簿批量创建工作表
        /// </summary>
        /// <param name="sheetNames">工作表名集</param>
        /// <returns></returns>
        public static void CreateSheets(List<string> sheetNames)
        {
            sheetNames.ForEach(x =>
            {
                _customerWorkbook.CreateSheet(x);
            });
        }

        /// <summary>
        /// 第三步：为每个工作表创建表头
        /// </summary>
        /// <param name="rowTitles"></param>
        /// <returns></returns>
        public static void CreateTitleRows(List<string> rowTitles)
        {
            for (int i = 0; i < _customerWorkbook.NumberOfSheets; i++)
            {
                IRow rowTitle = _customerWorkbook.GetSheetAt(i).CreateRow(0);
                for (int j = 0; j < rowTitles.Count; j++)
                {
                    rowTitle.CreateCell(j).SetCellValue(rowTitles[j]);
                }
            }
        }

        /// <summary>
        /// 第四步：为每个工作表插入数据行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public static void CreateAllSheetDataRows<T>(List<List<T>> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                ISheet sheet = _customerWorkbook.GetSheetAt(i);
                CreateDataRows(data[i], sheet);
            }
        }
        /// <summary>
        /// 第五步：获取自定义工作簿文件流
        /// </summary>
        /// <returns></returns>
        public static Stream GetCustomerWorkBookStream()
        {
            //写入文件
            Stream stream = new MemoryStream();
            _customerWorkbook.Write(stream);
            return stream;
        }
        /// <summary>
        /// 第五步：获取自定义工作簿对象
        /// </summary>
        /// <returns></returns>
        public static IWorkbook GetCustomerWorkBook()
        {
            return _customerWorkbook;
        }


        #endregion

    }
}
