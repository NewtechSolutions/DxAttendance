namespace SAPLeaveTemplate
{
    partial class Form1
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtCurUploadDt = new System.Windows.Forms.TextBox();
            this.txtCurYearMt = new DevExpress.XtraEditors.DateEdit();
            this.label12 = new System.Windows.Forms.Label();
            this.txtLastYearMt = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtLastUploadDt = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.chkFinalUpload = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.chkPassOpt = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chkedLeave = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chkExclude = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbTemplate = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtToDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFromDt = new System.Windows.Forms.DateTimePicker();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurYearMt.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurYearMt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkedLeave.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtCurUploadDt);
            this.groupBox1.Controls.Add(this.txtCurYearMt);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtLastYearMt);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtLastUploadDt);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.chkFinalUpload);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.chkPassOpt);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.chkedLeave);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.chkExclude);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbTemplate);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtToDate);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtFromDt);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(507, 376);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parameters";
            // 
            // txtCurUploadDt
            // 
            this.txtCurUploadDt.Location = new System.Drawing.Point(303, 24);
            this.txtCurUploadDt.Name = "txtCurUploadDt";
            this.txtCurUploadDt.ReadOnly = true;
            this.txtCurUploadDt.Size = new System.Drawing.Size(180, 21);
            this.txtCurUploadDt.TabIndex = 27;
            this.txtCurUploadDt.TabStop = false;
            // 
            // txtCurYearMt
            // 
            this.txtCurYearMt.EditValue = null;
            this.txtCurYearMt.Location = new System.Drawing.Point(187, 24);
            this.txtCurYearMt.Name = "txtCurYearMt";
            this.txtCurYearMt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtCurYearMt.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtCurYearMt.Properties.CalendarTimeProperties.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4);
            this.txtCurYearMt.Properties.CalendarTimeProperties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.Default;
            this.txtCurYearMt.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
            this.txtCurYearMt.Properties.Mask.EditMask = "Y";
            this.txtCurYearMt.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtCurYearMt.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearView;
            this.txtCurYearMt.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
            this.txtCurYearMt.Size = new System.Drawing.Size(108, 20);
            this.txtCurYearMt.TabIndex = 0;
            this.txtCurYearMt.Validated += new System.EventHandler(this.txtCurYearMt_Validated);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(23, 26);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(158, 13);
            this.label12.TabIndex = 26;
            this.label12.Text = "Tempate for YearMt (yyyyMM):";
            // 
            // txtLastYearMt
            // 
            this.txtLastYearMt.Location = new System.Drawing.Point(187, 51);
            this.txtLastYearMt.Name = "txtLastYearMt";
            this.txtLastYearMt.ReadOnly = true;
            this.txtLastYearMt.Size = new System.Drawing.Size(108, 21);
            this.txtLastYearMt.TabIndex = 25;
            this.txtLastYearMt.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(46, 54);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(135, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "Prv Mth Upload DateTime :";
            // 
            // txtLastUploadDt
            // 
            this.txtLastUploadDt.Location = new System.Drawing.Point(303, 51);
            this.txtLastUploadDt.Name = "txtLastUploadDt";
            this.txtLastUploadDt.ReadOnly = true;
            this.txtLastUploadDt.Size = new System.Drawing.Size(180, 21);
            this.txtLastUploadDt.TabIndex = 23;
            this.txtLastUploadDt.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(275, 254);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(208, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "( Works with only Arrear upload template)";
            // 
            // chkFinalUpload
            // 
            this.chkFinalUpload.AutoSize = true;
            this.chkFinalUpload.Checked = true;
            this.chkFinalUpload.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFinalUpload.Location = new System.Drawing.Point(187, 253);
            this.chkFinalUpload.Name = "chkFinalUpload";
            this.chkFinalUpload.Size = new System.Drawing.Size(82, 17);
            this.chkFinalUpload.TabIndex = 8;
            this.chkFinalUpload.Text = "Check - Yes";
            this.chkFinalUpload.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(61, 254);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(120, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Mark as Final Upload ? :";
            // 
            // chkPassOpt
            // 
            this.chkPassOpt.AutoSize = true;
            this.chkPassOpt.Checked = true;
            this.chkPassOpt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPassOpt.Location = new System.Drawing.Point(187, 225);
            this.chkPassOpt.Name = "chkPassOpt";
            this.chkPassOpt.Size = new System.Drawing.Size(82, 17);
            this.chkPassOpt.TabIndex = 7;
            this.chkPassOpt.Text = "Check - Yes";
            this.chkPassOpt.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(75, 226);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Password Protect ? :";
            // 
            // chkedLeave
            // 
            this.chkedLeave.Location = new System.Drawing.Point(187, 199);
            this.chkedLeave.Name = "chkedLeave";
            this.chkedLeave.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.chkedLeave.Properties.DropDownRows = 20;
            this.chkedLeave.Size = new System.Drawing.Size(205, 20);
            this.chkedLeave.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(111, 202);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Leave Type :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(275, 150);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(188, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "( Works with only EL upload template)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(161, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Exclude Previous Period date ? :";
            // 
            // chkExclude
            // 
            this.chkExclude.AutoSize = true;
            this.chkExclude.Checked = true;
            this.chkExclude.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExclude.Location = new System.Drawing.Point(187, 149);
            this.chkExclude.Name = "chkExclude";
            this.chkExclude.Size = new System.Drawing.Size(82, 17);
            this.chkExclude.TabIndex = 3;
            this.chkExclude.Text = "Check - Yes";
            this.chkExclude.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 354);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(276, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "* This utility will generate password protected excel file.";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(186, 300);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 35);
            this.button1.TabIndex = 9;
            this.button1.Text = "Generate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(123, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Template :";
            // 
            // cmbTemplate
            // 
            this.cmbTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTemplate.FormattingEnabled = true;
            this.cmbTemplate.Items.AddRange(new object[] {
            "SAP_ShiftSchedule_UPLOAD",
            "SAP_EL_UPLOAD",
            "SAP_FULL_UPLOAD",
            "SAP_OD_UPLOAD",
            "SAP_HALF_ABLWPAL_UPLOAD",
            "SAP_HALF_CLSL_UPLOAD",
            "SAP_ARREAR_FULL_UPLOAD",
            "SAP_ARREAR_HALF_UPLOAD"});
            this.cmbTemplate.Location = new System.Drawing.Point(187, 172);
            this.cmbTemplate.Name = "cmbTemplate";
            this.cmbTemplate.Size = new System.Drawing.Size(205, 21);
            this.cmbTemplate.TabIndex = 4;
            this.cmbTemplate.SelectionChangeCommitted += new System.EventHandler(this.cmbTemplate_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "To Date (dd/MM/yyyy) :";
            // 
            // txtToDate
            // 
            this.txtToDate.CustomFormat = "dd/MM/yyyy";
            this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.txtToDate.Location = new System.Drawing.Point(187, 108);
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(108, 21);
            this.txtToDate.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "From Date (dd/MM/yyyy)  :";
            // 
            // txtFromDt
            // 
            this.txtFromDt.CustomFormat = "dd/MM/yyyy";
            this.txtFromDt.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.txtFromDt.Location = new System.Drawing.Point(187, 81);
            this.txtFromDt.Name = "txtFromDt";
            this.txtFromDt.Size = new System.Drawing.Size(108, 21);
            this.txtFromDt.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 376);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "SAP Upload Template Utility v 4.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurYearMt.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurYearMt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkedLeave.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbTemplate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker txtToDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker txtFromDt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkExclude;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.CheckedComboBoxEdit chkedLeave;
        private System.Windows.Forms.CheckBox chkPassOpt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkFinalUpload;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtLastUploadDt;
        private System.Windows.Forms.TextBox txtLastYearMt;
        private System.Windows.Forms.Label label12;
        private DevExpress.XtraEditors.DateEdit txtCurYearMt;
        private System.Windows.Forms.TextBox txtCurUploadDt;

    }
}

