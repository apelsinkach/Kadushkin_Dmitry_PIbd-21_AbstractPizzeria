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
    public partial class PutOnResource : Form
    {
        public PutOnResource()
        {
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxIngridient.SelectedValue == null)
            {
                MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxResource.SelectedValue == null)
            {
                MessageBox.Show("Выберите склад", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void PutOnResource_Load(object sender, EventArgs e)
        {
            try
            {
                var responseC = APIClient.GetRequest("api/Ingridient/GetList");
                if (responseC.Result.IsSuccessStatusCode)
                {
                    List<IngridientViewModel> list = APIClient.GetElement<List<IngridientViewModel>>(responseC);
                    if (list != null)
                    {
                        comboBoxIngridient.DisplayMember = "IngridientName";
                        comboBoxIngridient.ValueMember = "Id";
                        comboBoxIngridient.DataSource = list;
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
                        comboBoxResource.DisplayMember = "ResourceName";
                        comboBoxResource.ValueMember = "Id";
                        comboBoxResource.DataSource = list;
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
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
