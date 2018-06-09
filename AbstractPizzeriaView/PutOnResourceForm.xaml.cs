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
using Unity;
using Unity.Attributes;

namespace AbstractPizzeriaView
{
    /// <summary>
    /// Логика взаимодействия для PutOnResourceForm.xaml
    /// </summary>
    public partial class PutOnResourceForm : Window
    {

        [Dependency]
        public IUnityContainer Container { get; set; }

        private readonly IResourceService serviceS;

        private readonly IIngridientService serviceC;

        private readonly IBasicService serviceM;

        public PutOnResourceForm(IResourceService serviceS, IIngridientService serviceC, IBasicService serviceM)
        {
            InitializeComponent();
            this.serviceS = serviceS;
            this.serviceC = serviceC;
            this.serviceM = serviceM;
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
                serviceM.PutComponentOnStock(new ResourceIngridientBindingModel
                {
                    IngridientId = Convert.ToInt32(comboBoxIngridient.SelectedValue),
                    ResourceId = Convert.ToInt32(comboBoxResource.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
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
                List<IngridientViewModel> listC = serviceC.GetList();
                if (listC != null)
                {
                    comboBoxIngridient.DisplayMemberPath = "IngridientName";
                    comboBoxIngridient.SelectedValuePath = "Id";
                    comboBoxIngridient.ItemsSource = listC;
                    comboBoxIngridient.SelectedItem = null;
                }
                List<ResourceViewModel> listS = serviceS.GetList();
                if (listS != null)
                {
                    comboBoxResource.DisplayMemberPath = "ResourceName";
                    comboBoxResource.SelectedValuePath = "Id";
                    comboBoxResource.ItemsSource = listS;
                    comboBoxResource.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
