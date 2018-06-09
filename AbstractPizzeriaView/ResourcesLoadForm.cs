using AbstractPizzeriaService.BindingModels;
using AbstractPizzeriaService.Interfaces;
using AbstractPizzeriaService.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AbstractPizzeriaView
{
    public partial class ResourcesLoadForm : Form
    {
        public ResourcesLoadForm()
        {
            InitializeComponent();
        }

        private void ResourcesLoadForm_Load(object sender, EventArgs e)
        {
            try
            {
                var response = APIClient.GetRequest("api/Statement/GetStocksLoad");
                if (response.Result.IsSuccessStatusCode)
                {
                    dataGridView.Rows.Clear();
                    foreach (var elem in APIClient.GetElement<List<ResourcesLoadViewModel>>(response))
                    {
                        dataGridView.Rows.Add(new object[] { elem.ResourceName, "", "" });
                        foreach (var listElem in elem.Ingridients)
                        {
                            dataGridView.Rows.Add(new object[] { "", listElem.Item1, listElem.Item2 });
                        }
                        dataGridView.Rows.Add(new object[] { "Итого", "", elem.TotalCount });
                        dataGridView.Rows.Add(new object[] { });
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSaveToExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "xls|*.xls|xlsx|*.xlsx"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var response = APIClient.PostRequest("api/Statement/SaveStocksLoad", new StatementBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(response));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
