﻿using AbstractPizzeriaService.Interfaces;
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
