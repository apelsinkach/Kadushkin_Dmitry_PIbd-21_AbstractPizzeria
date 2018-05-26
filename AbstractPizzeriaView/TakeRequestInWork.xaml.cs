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
    /// Логика взаимодействия для TakeRequestInWork.xaml
    /// </summary>
    public partial class TakeRequestInWork : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IWorkerService serviceI;

        private readonly IBasicService serviceM;

        private int? id;

        public TakeRequestInWork(IWorkerService serviceI, IBasicService serviceM)
        {
            InitializeComponent();
            this.serviceI = serviceI;
            this.serviceM = serviceM;
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
                serviceM.TakeOrderInWork(new RequestBindingModel
                {
                    Id = id.Value,
                    WorkerId = Convert.ToInt32(comboBoxWorker.SelectedValue)
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
                if (!id.HasValue)
                {
                    MessageBox.Show("Не указан заказ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                List<WorkerViewModel> listI = serviceI.GetList();
                if (listI != null)
                {
                    comboBoxWorker.DisplayMemberPath = "WorkerFIO";
                    comboBoxWorker.SelectedValuePath = "Id";
                    comboBoxWorker.ItemsSource = listI;
                    comboBoxWorker.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
