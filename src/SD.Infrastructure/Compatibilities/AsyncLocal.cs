using System.Runtime.Remoting.Messaging;

// ReSharper disable once CheckNamespace
namespace System.Threading
{
    /// <summary>
    /// 表示对于给定异步控制流（如异步方法）是本地数据的环境数据
    /// </summary>
    /// <typeparam name="T">环境数据的类型</typeparam>
    internal sealed class AsyncLocal<T>
    {
        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync = new object();

        /// <summary>
        /// 内部缓存键
        /// </summary>
        private readonly string _internalKey;

        /// <summary>
        /// 实例化不接收更改通知的 System.Threading.AsyncLocal`1 实例
        /// </summary>
        public AsyncLocal()
        {
            lock (_Sync)
            {
                this._internalKey = Guid.NewGuid().ToString();
            }
        }

        /// <summary>
        /// 获取或设置环境数据的值
        /// </summary>
        /// <returns>环境数据的值。 如果已不设置任何值，返回的值是default （t)</returns>
        public T Value
        {
            get
            {
                object value = CallContext.LogicalGetData(this._internalKey);
                if (value == null)
                {
                    return default(T);
                }

                return (T)value;
            }
            set
            {
                if (value == null || value.Equals(default(T)))
                {
                    CallContext.FreeNamedDataSlot(this._internalKey);
                }
                else
                {
                    CallContext.LogicalSetData(this._internalKey, value);
                }
            }
        }
    }
}
