using AbstractPizzeriaService.ImplementationsBD;
using AbstractPizzeriaService.Interfaces;
using AbstractShopService.ImplementationsList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;

namespace AbstractPizzeriaView
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<MainForm>());
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<ICustomerService, CustomerServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IIngridientService, IngridientServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IWorkerService, WorkerServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IArticleService, ArticleServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IResourceService, ResourceServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IBasicService, BasicServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IStatementService, StatementServiceBD>(new HierarchicalLifetimeManager());
            return currentContainer;
        }


    }
}
