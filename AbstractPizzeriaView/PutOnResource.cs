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
using Unity;
using Unity.Attributes;

namespace AbstractPizzeriaView
{
    public partial class PutOnResource : Form
    {

        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IResourceService serviceS;

        private readonly IIngridientService serviceC;

        private readonly IBasicService serviceM;

        public PutOnResource(IResourceService serviceS, IIngridientService serviceC, IBasicService serviceM)
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
                serviceM.PutComponentOnStock(new ResourceIngridientBindingModel
                {
                    IngridientId = Convert.ToInt32(comboBoxIngridient.SelectedValue),
                    ResourceId = Convert.ToInt32(comboBoxResource.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
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
                List<IngridientViewModel> listC = serviceC.GetList();
                if (listC != null)
                {
                    comboBoxIngridient.DisplayMember = "IngridientName";
                    comboBoxIngridient.ValueMember = "Id";
                    comboBoxIngridient.DataSource = listC;
                    comboBoxIngridient.SelectedItem = null;
                }
                List<ResourceViewModel> listS = serviceS.GetList();
                if (listS != null)
                {
                    comboBoxResource.DisplayMember = "ResourceName";
                    comboBoxResource.ValueMember = "Id";
                    comboBoxResource.DataSource = listS;
                    comboBoxResource.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
