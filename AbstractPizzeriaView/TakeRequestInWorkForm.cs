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
    public partial class TakeRequestInWorkForm : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        public TakeRequestInWorkForm()
        {
            InitializeComponent();
        }

        private void TakeRequestInWorkForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (!id.HasValue)
                {
                    MessageBox.Show("Не указан заказ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }
                var response = APIClient.GetRequest("api/Worker/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<WorkerViewModel> list = APIClient.GetElement<List<WorkerViewModel>>(response);
                    if (list != null)
                    {
                        comboBoxWorker.DisplayMember = "ImplementerFIO";
                        comboBoxWorker.ValueMember = "Id";
                        comboBoxWorker.DataSource = list;
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
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxWorker.SelectedValue == null)
            {
                MessageBox.Show("Выберите исполнителя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();

        }
    }
}
