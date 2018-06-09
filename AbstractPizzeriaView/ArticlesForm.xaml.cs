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
    /// Логика взаимодействия для ArticlesForm.xaml
    /// </summary>
    public partial class ArticlesForm : Window
    {
        public ArticlesForm()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                var response = APIClient.GetRequest("api/Article/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<ArticleViewModel> list = APIClient.GetElement<List<ArticleViewModel>>(response);
                    if (list != null)
                    {
                        dataGridView.ItemsSource = list;
                        dataGridView.Columns[0].Visibility = Visibility.Hidden;
                        dataGridView.Columns[1].Width = DataGridLength.Auto;
                        dataGridView.Columns[3].Visibility = Visibility.Hidden;
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


        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new ArticleForm();
            if (form.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedItem != null)
            {
                var form = new ArticleForm();

                form.Id = Convert.ToInt32(dataGridView.SelectedValue);
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridView.SelectedIndex + 1);
                    try
                    {
                        var response = APIClient.PostRequest("api/Article/DelElement", new CustomerBindingModel { Id = id });
                        if (!response.Result.IsSuccessStatusCode)
                        {
                            throw new Exception(APIClient.GetError(response));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}
