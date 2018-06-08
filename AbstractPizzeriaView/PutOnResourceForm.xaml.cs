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
    /// Логика взаимодействия для PutOnResourceForm.xaml
    /// </summary>
    public partial class PutOnResourceForm : Window
    {

        public PutOnResourceForm()
        {
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxIngridient.SelectedValue == null)
            {
                MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxResource.SelectedValue == null)
            {
                MessageBox.Show("Выберите склад", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var response = APIClient.PostRequest("api/Basic/PutComponentOnStock", new ResourceIngridientBindingModel
                {
                    IngridientId = Convert.ToInt32(comboBoxIngridient.SelectedValue),
                    ResourceId = Convert.ToInt32(comboBoxResource.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
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
                var responseC = APIClient.GetRequest("api/Ingridient/GetList");
                if (responseC.Result.IsSuccessStatusCode)
                {
                    List<IngridientViewModel> list = APIClient.GetElement<List<IngridientViewModel>>(responseC);
                    if (list != null)
                    {
                        comboBoxIngridient.DisplayMemberPath = "IngridientName";
                        comboBoxIngridient.SelectedValuePath = "Id";
                        comboBoxIngridient.ItemsSource = list;
                        comboBoxIngridient.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseC));
                }
                var responseS = APIClient.GetRequest("api/Resource/GetList");
                if (responseS.Result.IsSuccessStatusCode)
                {
                    List<ResourceViewModel> list = APIClient.GetElement<List<ResourceViewModel>>(responseS);
                    if (list != null)
                    {
                        comboBoxResource.DisplayMemberPath = "ResourceName";
                        comboBoxResource.SelectedValuePath = "Id";
                        comboBoxResource.ItemsSource = list;
                        comboBoxResource.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseC));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
