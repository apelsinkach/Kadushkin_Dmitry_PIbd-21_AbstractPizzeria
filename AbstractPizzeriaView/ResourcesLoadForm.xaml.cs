using AbstractPizzeriaService.BindingModels;
using AbstractPizzeriaService.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Unity;
using Unity.Attributes;

namespace AbstractPizzeriaView
{
    /// <summary>
    /// Логика взаимодействия для ResourcesLoadForm.xaml
    /// </summary>
    public partial class ResourcesLoadForm : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IStatementService service;

        public ResourcesLoadForm(IStatementService service)
        {
            InitializeComponent();
            this.service = service;
        }


        private void buttonSaveToExcel_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "xls|*.xls|xlsx|*.xlsx"
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    service.SaveStocksLoad(new StatementBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ResourcesLoadForm_Load(object sender, RoutedEventArgs e)
        {
            try
            {
                var dict = service.GetStocksLoad();
                if (dict != null)
                {              
                    dataGridView.ItemsSource=dict;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

