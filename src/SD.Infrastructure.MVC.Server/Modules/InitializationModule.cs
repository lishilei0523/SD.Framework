using SD.Infrastructure.Global;
using SD.IOC.Integration.MVC;
using System;
using System.Web;

namespace SD.Infrastructure.MVC.Server.Modules
{
    /// <summary>
    /// ��ʼ��ģ��
    /// </summary>
    internal class InitializationModule : IHttpModule
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
            MvcDependencyResolver.OnGetInstance += this.MvcDependencyResolver_OnGetInstance;
            MvcDependencyResolver.OnReleaseInstance += this.MvcDependencyResolver_OnReleaseInstance;
        }

        /// <summary>
        /// ��ȡ����ʵ���¼�
        /// </summary>
        private void MvcDependencyResolver_OnGetInstance()
        {
            //��ʼ��SessionId
            Initializer.InitSessionId();
        }

        /// <summary>
        /// ���ٷ���ʵ���¼�
        /// </summary>
        private void MvcDependencyResolver_OnReleaseInstance()
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