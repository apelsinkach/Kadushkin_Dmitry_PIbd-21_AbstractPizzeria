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
    public partial class ResourceForm : Form
    {

        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IResourceService service;

        private int? id;

        public ResourceForm(IResourceService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void ResourceForm_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    ResourceViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.ResourceName;
                        dataGridView.DataSource = view.ResourceIngridients;
                        dataGridView.Columns[0].Visible = false;
                        dataGridView.Columns[1].Visible = false;
                        dataGridView.Columns[2].Visible = false;
                        dataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (id.HasValue)
                {
                    service.UpdElement(new ResourceBindingModel
                    {
                        Id = id.Value,
                        ResourceName = textBoxName.Text
                    });
                }
                else
                {
                    service.AddElement(new ResourceBindingModel
                    {
                        ResourceName = textBoxName.Text
                    });
                }
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
    }
}
