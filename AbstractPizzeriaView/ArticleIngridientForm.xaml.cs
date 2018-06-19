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
    /// Логика взаимодействия для ArticleIngridientForm.xaml
    /// </summary>
    public partial class ArticleIngridientForm : Window
    {

        public ArticleIngridientViewModel Model { set { model = value; } get { return model; } }

        private ArticleIngridientViewModel model;

        public ArticleIngridientForm()
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
            try
            {
                if (model == null)
                {
                    model = new ArticleIngridientViewModel
                    {
                        IngridientId = Convert.ToInt32(comboBoxIngridient.SelectedValue),
                        IngridientName = comboBoxIngridient.Text,
                        Count = Convert.ToInt32(textBoxCount.Text)
                    };
                }
                else
                {
                    model.Count = Convert.ToInt32(textBoxCount.Text);
                }
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
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
                var response = APIClient.GetRequest("api/Ingridient/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    comboBoxIngridient.DisplayMemberPath = "IngridientName";
                    comboBoxIngridient.SelectedValuePath = "Id";
                    comboBoxIngridient.ItemsSource = APIClient.GetElement<List<IngridientViewModel>>(response);
                    comboBoxIngridient.SelectedItem = null;
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
            if (model != null)
            {
                comboBoxIngridient.IsEnabled = false;
                comboBoxIngridient.SelectedValue = model.IngridientId;
                textBoxCount.Text = model.Count.ToString();
            }
        }
    }
}
