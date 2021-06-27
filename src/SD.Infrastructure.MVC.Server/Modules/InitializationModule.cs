using SD.Infrastructure.Global;
using System;
using System.Web;

namespace SD.Infrastructure.MVC.Server.Modules
{
    /// <summary>
    /// ASP.NET MVC��ʼ��ģ��
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
            context.EndRequest += OnEndRequest;
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
        /// ��������¼�
        /// </summary>
        private static void OnEndRequest(object sender, EventArgs eventArgs)
        {
            //�������ݿ�
            Finalizer.CleanDb();
        }

        /// <summary>
        /// �ͷ���Դ
        /// </summary>
        public void Dispose()
        {
            //�������ݿ�
            Finalizer.CleanDb();
        }
    }
}