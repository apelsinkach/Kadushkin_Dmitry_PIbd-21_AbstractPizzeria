using AbstractPizzeriaService.BindingModels;
using AbstractPizzeriaService.Interfaces;
using Microsoft.Reporting.WinForms;
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
    /// Логика взаимодействия для CustomerRequestsForm.xaml
    /// </summary>
    public partial class CustomerRequestsForm : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IStatementService service;

        public CustomerRequestsForm(IStatementService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void buttonMake_Click(object sender, RoutedEventArgs e)
        {
            if (dateTimePickerFrom.SelectedDate >= dateTimePickerTo.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var dataSource = service.GetClientOrders(new StatementBindingModel
                {
                    DateFrom = dateTimePickerFrom.SelectedDate,
                    DateTo = dateTimePickerTo.SelectedDate
                });

                ReportDataSource source = new ReportDataSource("DataSetRequests", dataSource);
                reportViewer.LocalReport.DataSources.Add(source);


                reportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.reportViewer.RefreshReport();
            this.reportViewer.LocalReport.ReportEmbeddedResource = "AbstractPizzeriaView.Report.rdlc";

        }
    }
}
