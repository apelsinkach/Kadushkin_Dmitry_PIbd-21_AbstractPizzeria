using AbstractPizzeriaService;
using AbstractPizzeriaService.ImplementationsBD;
using AbstractPizzeriaService.Interfaces;
using AbstractShopService.ImplementationsList;
using System;
using System.Data.Entity;
using System.Windows;



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
            var application = new App();
            application.Run(new MainWindow());
        }
    }
}
