namespace Attendance.Forms
{
    partial class frmMastEmpBlackList
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
            this.label3 = new System.Windows.Forms.Label();
            this.txtUnBlockRemarks = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBlockRemark = new DevExpress.XtraEditors.TextEdit();
            this.txtUpdID = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUpdDt = new DevExpress.XtraEditors.DateEdit();
            this.txtAddID = new DevExpress.XtraEditors.TextEdit();
            this.label22 = new System.Windows.Forms.Label();
            this.txtAddDt = new DevExpress.XtraEditors.DateEdit();
            this.chkActive = new DevExpress.XtraEditors.CheckEdit();
            this.label20 = new System.Windows.Forms.Label();
            this.txtAdharNo = new DevExpress.XtraEditors.TextEdit();
            this.grpUserRights = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUnBlockRemarks.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBlockRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpdID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpdDt.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpdDt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddDt.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddDt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkActive.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAdharNo.Properties)).BeginInit();
            this.grpUserRights.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtUnBlockRemarks);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtBlockRemark);
            this.groupBox1.Controls.Add(this.txtUpdID);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtUpdDt);
            this.groupBox1.Controls.Add(this.txtAddID);
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Controls.Add(this.txtAddDt);
            this.groupBox1.Controls.Add(this.chkActive);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.txtAdharNo);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(452, 211);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 161);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 18);
            this.label3.TabIndex = 105;
            this.label3.Text = "UnBlock Remarks :";
            // 
            // txtUnBlockRemarks
            // 
            this.txtUnBlockRemarks.EditValue = "";
            this.txtUnBlockRemarks.Location = new System.Drawing.Point(122, 159);
            this.txtUnBlockRemarks.Name = "txtUnBlockRemarks";
            this.txtUnBlockRemarks.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUnBlockRemarks.Properties.Appearance.Options.UseFont = true;
            this.txtUnBlockRemarks.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtUnBlockRemarks.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            this.txtUnBlockRemarks.Properties.Mask.EditMask = "[A-Z .]+";
            this.txtUnBlockRemarks.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtUnBlockRemarks.Properties.MaxLength = 50;
            this.txtUnBlockRemarks.Size = new System.Drawing.Size(282, 20);
            this.txtUnBlockRemarks.TabIndex = 104;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 18);
            this.label2.TabIndex = 103;
            this.label2.Text = "Block Remarks :";
            // 
            // txtBlockRemark
            // 
            this.txtBlockRemark.EditValue = "";
            this.txtBlockRemark.Location = new System.Drawing.Point(122, 133);
            this.txtBlockRemark.Name = "txtBlockRemark";
            this.txtBlockRemark.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBlockRemark.Properties.Appearance.Options.UseFont = true;
            this.txtBlockRemark.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtBlockRemark.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            this.txtBlockRemark.Properties.Mask.EditMask = "[A-Z .]+";
            this.txtBlockRemark.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtBlockRemark.Properties.MaxLength = 50;
            this.txtBlockRemark.Size = new System.Drawing.Size(282, 20);
            this.txtBlockRemark.TabIndex = 102;
            // 
            // txtUpdID
            // 
            this.txtUpdID.Location = new System.Drawing.Point(238, 97);
            this.txtUpdID.Name = "txtUpdID";
            this.txtUpdID.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUpdID.Properties.Appearance.Options.UseFont = true;
            this.txtUpdID.Properties.ReadOnly = true;
            this.txtUpdID.Size = new System.Drawing.Size(166, 20);
            this.txtUpdID.TabIndex = 101;
            this.txtUpdID.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(62, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 15);
            this.label1.TabIndex = 100;
            this.label1.Text = "Upd Dt. :";
            // 
            // txtUpdDt
            // 
            this.txtUpdDt.EditValue = null;
            this.txtUpdDt.Location = new System.Drawing.Point(122, 97);
            this.txtUpdDt.Name = "txtUpdDt";
            this.txtUpdDt.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUpdDt.Properties.Appearance.Options.UseFont = true;
            this.txtUpdDt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtUpdDt.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtUpdDt.Properties.CalendarTimeProperties.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4);
            this.txtUpdDt.Properties.CalendarTimeProperties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.Default;
            this.txtUpdDt.Properties.ReadOnly = true;
            this.txtUpdDt.Size = new System.Drawing.Size(110, 20);
            this.txtUpdDt.TabIndex = 99;
            // 
            // txtAddID
            // 
            this.txtAddID.Location = new System.Drawing.Point(238, 71);
            this.txtAddID.Name = "txtAddID";
            this.txtAddID.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddID.Properties.Appearance.Options.UseFont = true;
            this.txtAddID.Properties.ReadOnly = true;
            this.txtAddID.Size = new System.Drawing.Size(166, 20);
            this.txtAddID.TabIndex = 98;
            this.txtAddID.TabStop = false;
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(62, 74);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(52, 15);
            this.label22.TabIndex = 97;
            this.label22.Text = "Add Dt. :";
            // 
            // txtAddDt
            // 
            this.txtAddDt.EditValue = null;
            this.txtAddDt.Location = new System.Drawing.Point(122, 71);
            this.txtAddDt.Name = "txtAddDt";
            this.txtAddDt.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddDt.Properties.Appearance.Options.UseFont = true;
            this.txtAddDt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtAddDt.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtAddDt.Properties.CalendarTimeProperties.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4);
            this.txtAddDt.Properties.CalendarTimeProperties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.Default;
            this.txtAddDt.Properties.ReadOnly = true;
            this.txtAddDt.Size = new System.Drawing.Size(110, 20);
            this.txtAddDt.TabIndex = 96;
            // 
            // chkActive
            // 
            this.chkActive.Location = new System.Drawing.Point(122, 46);
            this.chkActive.Name = "chkActive";
            this.chkActive.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkActive.Properties.Appearance.Options.UseFont = true;
            this.chkActive.Properties.Caption = "Balck Listed";
            this.chkActive.Size = new System.Drawing.Size(84, 19);
            this.chkActive.TabIndex = 95;
            this.chkActive.TabStop = false;
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(37, 22);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(81, 18);
            this.label20.TabIndex = 94;
            this.label20.Text = "Aadhaar No :";
            // 
            // txtAdharNo
            // 
            this.txtAdharNo.EditValue = "123456789012";
            this.txtAdharNo.Location = new System.Drawing.Point(124, 20);
            this.txtAdharNo.Name = "txtAdharNo";
            this.txtAdharNo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAdharNo.Properties.Appearance.Options.UseFont = true;
            this.txtAdharNo.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtAdharNo.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            this.txtAdharNo.Properties.Mask.EditMask = "[0-9]+";
            this.txtAdharNo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtAdharNo.Properties.MaxLength = 12;
            this.txtAdharNo.Size = new System.Drawing.Size(280, 20);
            this.txtAdharNo.TabIndex = 93;
            this.txtAdharNo.Validated += new System.EventHandler(this.txtAdharNo_Validated);
            // 
            // grpUserRights
            // 
            this.grpUserRights.Controls.Add(this.btnClose);
            this.grpUserRights.Controls.Add(this.btnCancel);
            this.grpUserRights.Controls.Add(this.btnDelete);
            this.grpUserRights.Controls.Add(this.btnUpdate);
            this.grpUserRights.Controls.Add(this.btnAdd);
            this.grpUserRights.Location = new System.Drawing.Point(12, 229);
            this.grpUserRights.Name = "grpUserRights";
            this.grpUserRights.Size = new System.Drawing.Size(452, 52);
            this.grpUserRights.TabIndex = 3;
            this.grpUserRights.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Cornsilk;
            this.btnClose.Location = new System.Drawing.Point(329, 14);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 32);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Clos&e";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Cornsilk;
            this.btnCancel.Location = new System.Drawing.Point(249, 14);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 32);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Cornsilk;
            this.btnDelete.Location = new System.Drawing.Point(168, 14);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 32);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.Cornsilk;
            this.btnUpdate.Location = new System.Drawing.Point(87, 14);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 32);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "&Update";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.Cornsilk;
            this.btnAdd.Location = new System.Drawing.Point(6, 14);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 32);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // frmMastEmpBlackList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 283);
            this.Controls.Add(this.grpUserRights);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmMastEmpBlackList";
            this.Text = "AdharCard Black List";
            this.Load += new System.EventHandler(this.frmMastEmpBlackList_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtUnBlockRemarks.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBlockRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpdID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpdDt.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpdDt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddDt.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddDt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkActive.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAdharNo.Properties)).EndInit();
            this.grpUserRights.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox grpUserRights;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label20;
        private DevExpress.XtraEditors.TextEdit txtAdharNo;
        private DevExpress.XtraEditors.CheckEdit chkActive;
        private System.Windows.Forms.Label label22;
        private DevExpress.XtraEditors.DateEdit txtAddDt;
        private DevExpress.XtraEditors.TextEdit txtUpdID;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.DateEdit txtUpdDt;
        private DevExpress.XtraEditors.TextEdit txtAddID;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit txtBlockRemark;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.TextEdit txtUnBlockRemarks;
    }
}