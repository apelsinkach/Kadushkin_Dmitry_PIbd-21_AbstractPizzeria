namespace AbstractPizzeriaView
{
    partial class CustomerRequestsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.reportViewer = new Microsoft.Reporting.WinForms.ReportViewer();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.buttonMake = new System.Windows.Forms.Button();
            this.buttonToPdf = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Location = new System.Drawing.Point(59, 12);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(200, 22);
            this.dateTimePickerFrom.TabIndex = 0;
            // 
            // reportViewer
            // 
            this.reportViewer.LocalReport.ReportEmbeddedResource = "AbstractPizzeriaView.Report.rdlc";
            this.reportViewer.Location = new System.Drawing.Point(2, 51);
            this.reportViewer.Name = "reportViewer";
            this.reportViewer.ServerReport.BearerToken = null;
            this.reportViewer.Size = new System.Drawing.Size(979, 450);
            this.reportViewer.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "C";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(278, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "по";
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.Location = new System.Drawing.Point(323, 12);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(200, 22);
            this.dateTimePickerTo.TabIndex = 4;
            // 
            // buttonMake
            // 
            this.buttonMake.Location = new System.Drawing.Point(568, 12);
            this.buttonMake.Name = "buttonMake";
            this.buttonMake.Size = new System.Drawing.Size(176, 23);
            this.buttonMake.TabIndex = 5;
            this.buttonMake.Text = "Сформировать";
            this.buttonMake.UseVisualStyleBackColor = true;
            this.buttonMake.Click += new System.EventHandler(this.buttonMake_Click);
            // 
            // buttonToPdf
            // 
            this.buttonToPdf.Location = new System.Drawing.Point(836, 13);
            this.buttonToPdf.Name = "buttonToPdf";
            this.buttonToPdf.Size = new System.Drawing.Size(114, 23);
            this.buttonToPdf.TabIndex = 6;
            this.buttonToPdf.Text = "В Pdf";
            this.buttonToPdf.UseVisualStyleBackColor = true;
            this.buttonToPdf.Click += new System.EventHandler(this.buttonToPdf_Click);
            // 
            // CustomerRequestsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(980, 500);
            this.Controls.Add(this.buttonToPdf);
            this.Controls.Add(this.buttonMake);
            this.Controls.Add(this.dateTimePickerTo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.reportViewer);
            this.Controls.Add(this.dateTimePickerFrom);
            this.Name = "CustomerRequestsForm";
            this.Text = "Заказы клинтов";
            this.Load += new System.EventHandler(this.CustomerRequestsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.Button buttonMake;
        private System.Windows.Forms.Button buttonToPdf;
    }
}