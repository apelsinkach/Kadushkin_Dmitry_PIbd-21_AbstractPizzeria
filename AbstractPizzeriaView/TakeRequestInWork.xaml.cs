using AbstractPizzeriaService.BindingModels;
using AbstractPizzeriaService.Interfaces;
using AbstractPizzeriaService.ViewModels;
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
    /// Логика взаимодействия для TakeRequestInWork.xaml
    /// </summary>
    public partial class TakeRequestInWork : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public TakeRequestInWork()
        {
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxWorker.SelectedValue == null)
            {
                MessageBox.Show("Выберите исполнителя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var response = APIClient.PostRequest("api/Basic/TakeOrderInWork", new RequestBindingModel
                {
                    Id = id.Value,
                    WorkerId = Convert.ToInt32(comboBoxWorker.SelectedValue)
                });
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
            try
            {
                if (!id.HasValue)
                {
                    MessageBox.Show("Не указан заказ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                var response = APIClient.GetRequest("api/Implementer/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<WorkerViewModel> list = APIClient.GetElement<List<WorkerViewModel>>(response);
                    if (list != null)
                    {
                        comboBoxWorker.DisplayMemberPath = "WorkerFIO";
                        comboBoxWorker.SelectedValuePath = "Id";
                        comboBoxWorker.ItemsSource = list;
                        comboBoxWorker.SelectedItem = null;
                    }
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
