using AbstractPizzeriaService;
using AbstractPizzeriaService.ImplementationsBD;
using AbstractPizzeriaService.Interfaces;
using AbstractShopService.ImplementationsList;
using System;
using System.Data.Entity;
using System.Windows;
using Unity;
using Unity.Lifetime;


namespace AbstractPizzeriaView
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {

        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();
            var application = new App();

            application.Run(container.Resolve<MainWindow>());
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<DbContext, AbstractDbContext>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ICustomerService, CustomerServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IIngridientService, IngridientServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IWorkerService, WorkerServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IArticleService, ArticleServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IResourceService, ResourceServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IBasicService, BasicServiceBD>(new HierarchicalLifetimeManager());

            return currentContainer;
        }
    }
}
