using AbstractPizzeriaService.Interfaces;
using AbstractShopService.ImplementationsList;
using System;
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
            currentContainer.RegisterType<ICustomerService, CustomerServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IIngridientService, IngridientServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IWorkerService, WorkerServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IArticleService, ArticleServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IResourceService, ResourceServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IBasicService, BasicServiceList>(new HierarchicalLifetimeManager());

            return currentContainer;
        }
    }
}
