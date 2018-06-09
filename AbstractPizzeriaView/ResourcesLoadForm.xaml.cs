using AbstractPizzeriaService.BindingModels;
using AbstractPizzeriaService.Interfaces;
using AbstractPizzeriaService.ViewModels;
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


namespace AbstractPizzeriaView
{
    /// <summary>
    /// Логика взаимодействия для ResourcesLoadForm.xaml
    /// </summary>
    public partial class ResourcesLoadForm : Window
    {

        public ResourcesLoadForm()
        {
            InitializeComponent();
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
                    var response = APIClient.PostRequest("api/Statement/SaveStocksLoad", new StatementBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(response));
                    }
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
                var response = APIClient.GetRequest("api/Statement/GetStocksLoad");
                if (response.Result.IsSuccessStatusCode)
                {
                    dataGridView.ItemsSource = APIClient.GetElement<List<ResourcesLoadViewModel>>(response);
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

