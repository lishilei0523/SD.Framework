using SD.Infrastructure.Global;
using System;
using System.Web;

namespace SD.Infrastructure.WebApi.WebHost.Server
{
    /// <summary>
    /// ��ʼ��HttpModule
    /// </summary>
    internal class InitializationHttpModule : IHttpModule
    {
        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">Ӧ�ó���������</param>
        public void Init(HttpApplication context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

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
        private void OnBeginRequest(object sender, EventArgs e)
        {
            //��ʼ��SessionId
            Initializer.InitSessionId();
        }

        /// <summary>
        /// ��������¼�
        /// </summary>
        private void OnEndRequest(object sender, EventArgs e)
        {
            //�������ݿ�
            Finalizer.CleanDb();
        }

        /// <summary>
        /// �ͷ���Դ
        /// </summary>
        public void Dispose() { }
    }
}