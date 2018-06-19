using AbstractPizzeriaService.BindingModels;
using AbstractPizzeriaService.Interfaces;
using AbstractPizzeriaService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    /// Логика взаимодействия для ResourceForm.xaml
    /// </summary>
    public partial class ResourceForm : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public ResourceForm()
        {
            InitializeComponent();
        }


        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/Resource/UpdElement", new ResourceBindingModel
                    {
                        Id = id.Value,
                        ResourceName = textBoxName.Text
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Resource/AddElement", new ResourceBindingModel
                    {
                        ResourceName = textBoxName.Text
                    });
                }
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIClient.GetRequest("api/Resource/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var stock = APIClient.GetElement<ResourceViewModel>(response);
                        textBoxName.Text = stock.ResourceName;
                        dataGridView.ItemsSource = stock.ResourceIngridients;
                        dataGridView.Columns[0].Visibility = Visibility.Hidden;
                        dataGridView.Columns[1].Visibility = Visibility.Hidden;
                        dataGridView.Columns[2].Visibility = Visibility.Hidden;
                        dataGridView.Columns[3].Width = DataGridLength.Auto;
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
}
