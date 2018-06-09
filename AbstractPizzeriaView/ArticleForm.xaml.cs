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
using AbstractPizzeriaService.BindingModels;
using AbstractPizzeriaService.Interfaces;
using AbstractPizzeriaService.ViewModels;

namespace AbstractPizzeriaView
{
    /// <summary>
    /// Логика взаимодействия для ArticleForm.xaml
    /// </summary>
    public partial class ArticleForm : Window
    {

        [Dependency]
        public IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IArticleService service;

        private int? id;

        private List<ArticleIngridientViewModel> productComponents;


        public ArticleForm(IArticleService service)
        {
            InitializeComponent();
            this.service = service;
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<ArticleIngridientForm>();
            if (form.ShowDialog() == true)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                    {
                        form.Model.ArticleId = id.Value;
                    }
                    productComponents.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedItem != null)
            {
                var form = Container.Resolve<ArticleIngridientForm>();
                form.Model = productComponents[dataGridView.SelectedIndex];
                if (form.ShowDialog() == true)
                {
                    productComponents[dataGridView.SelectedIndex] = form.Model;
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
                    try
                    {
                        productComponents.RemoveAt(dataGridView.SelectedIndex);
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


        private void LoadData()
        {
            try
            {
                if (productComponents != null)
                {
                    dataGridView.ItemsSource = null;
                    dataGridView.ItemsSource = productComponents;
                    dataGridView.Columns[0].Visibility = Visibility.Hidden;
                    dataGridView.Columns[1].Visibility = Visibility.Hidden;
                    dataGridView.Columns[2].Visibility = Visibility.Hidden;
                    dataGridView.Columns[3].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (productComponents == null || productComponents.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                List<ArticleIngridientBindingModel> productComponentBM = new List<ArticleIngridientBindingModel>();
                for (int i = 0; i < productComponents.Count; ++i)
                {
                    productComponentBM.Add(new ArticleIngridientBindingModel
                    {
                        Id = productComponents[i].Id,
                        ArticleId = productComponents[i].ArticleId,
                        IngridientId = productComponents[i].IngridientId,
                        Count = productComponents[i].Count
                    });
                }
                if (id.HasValue)
                {
                    service.UpdElement(new ArticleBindingModel
                    {
                        Id = id.Value,
                        ArticleName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        ArticleIngridients = productComponentBM
                    });
                }
                else
                {
                    service.AddElement(new ArticleBindingModel
                    {
                        ArticleName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        ArticleIngridients = productComponentBM
                    });
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
            if (id.HasValue)
            {
                try
                {
                    ArticleViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.ArticleName;
                        textBoxPrice.Text = view.Price.ToString();
                        productComponents = view.ArticleIngridients;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                productComponents = new List<ArticleIngridientViewModel>();
            }
        }
    }
}
