using SD.Infrastructure.Global;
using System;
using System.Web;

namespace SD.Infrastructure.AspNet.Server.Modules
{
    /// <summary>
    /// ASP.NET��ʼ��ģ��
    /// </summary>
    internal class InitializationModule : IHttpModule
    {
        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">Ӧ�ó���������</param>
        public void Init(HttpApplication context)
        {
            #region # ��֤

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            #endregion

            //��ʼ��SessionId
            Initializer.InitSessionId();

            //��ʼ�����ݿ�
            Initializer.InitDataBase();

            //ע���¼�
            context.BeginRequest += OnBeginRequest;
        }

        /// <summary>
        /// ����ʼ�¼�
        /// </summary>
        private static void OnBeginRequest(object sender, EventArgs eventArgs)
        {
            //��ʼ��SessionId
            Initializer.InitSessionId();
        }

        /// <summary>
        /// �ͷ���Դ
        /// </summary>
        public void Dispose()
        {

        }
    }
}