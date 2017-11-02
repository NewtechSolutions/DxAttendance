﻿namespace Attendance.Forms
{
    partial class frmMastCostCodeSanManPower
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
            this.txtContEmp = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCompEmp = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtValidFrom = new DevExpress.XtraEditors.DateEdit();
            this.txtDescription = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCostCode = new DevExpress.XtraEditors.TextEdit();
            this.Group2 = new System.Windows.Forms.GroupBox();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grpUserRights = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtContEmp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompEmp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValidFrom.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValidFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostCode.Properties)).BeginInit();
            this.Group2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.grpUserRights.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtContEmp);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtCompEmp);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtValidFrom);
            this.groupBox1.Controls.Add(this.txtDescription);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtCostCode);
            this.groupBox1.Location = new System.Drawing.Point(12, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(818, 102);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // txtContEmp
            // 
            this.txtContEmp.Location = new System.Drawing.Point(373, 71);
            this.txtContEmp.Name = "txtContEmp";
            this.txtContEmp.Properties.Mask.EditMask = "[0-9]+";
            this.txtContEmp.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtContEmp.Properties.Mask.ShowPlaceHolders = false;
            this.txtContEmp.Properties.MaxLength = 4;
            this.txtContEmp.Size = new System.Drawing.Size(96, 20);
            this.txtContEmp.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(260, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 15);
            this.label3.TabIndex = 18;
            this.label3.Text = "Contractual Emp, :";
            // 
            // txtCompEmp
            // 
            this.txtCompEmp.Location = new System.Drawing.Point(158, 71);
            this.txtCompEmp.Name = "txtCompEmp";
            this.txtCompEmp.Properties.Mask.EditMask = "[0-9]+";
            this.txtCompEmp.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtCompEmp.Properties.Mask.ShowPlaceHolders = false;
            this.txtCompEmp.Properties.MaxLength = 4;
            this.txtCompEmp.Size = new System.Drawing.Size(96, 20);
            this.txtCompEmp.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 15);
            this.label2.TabIndex = 16;
            this.label2.Text = "Comp. OnRoll Emp, :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 15);
            this.label1.TabIndex = 14;
            this.label1.Text = "Valid From :";
            // 
            // txtValidFrom
            // 
            this.txtValidFrom.EditValue = null;
            this.txtValidFrom.Location = new System.Drawing.Point(101, 46);
            this.txtValidFrom.Name = "txtValidFrom";
            this.txtValidFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtValidFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtValidFrom.Properties.CalendarTimeProperties.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4);
            this.txtValidFrom.Properties.CalendarTimeProperties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.Default;
            this.txtValidFrom.Size = new System.Drawing.Size(153, 20);
            this.txtValidFrom.TabIndex = 13;
            this.txtValidFrom.EditValueChanged += new System.EventHandler(this.txtValidFrom_EditValueChanged);
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(260, 20);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Properties.MaxLength = 100;
            this.txtDescription.Properties.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(292, 20);
            this.txtDescription.TabIndex = 1;
            this.txtDescription.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "CostCode :";
            // 
            // txtCostCode
            // 
            this.txtCostCode.Location = new System.Drawing.Point(101, 20);
            this.txtCostCode.Name = "txtCostCode";
            this.txtCostCode.Properties.Mask.EditMask = "[A-Z0-9]+";
            this.txtCostCode.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtCostCode.Size = new System.Drawing.Size(153, 20);
            this.txtCostCode.TabIndex = 0;
            this.txtCostCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCostCode_KeyDown);
            this.txtCostCode.Validated += new System.EventHandler(this.txtCostCode_Validated);
            // 
            // Group2
            // 
            this.Group2.Controls.Add(this.grid);
            this.Group2.Location = new System.Drawing.Point(12, 169);
            this.Group2.Name = "Group2";
            this.Group2.Size = new System.Drawing.Size(818, 307);
            this.Group2.TabIndex = 4;
            this.Group2.TabStop = false;
            // 
            // grid
            // 
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(3, 17);
            this.grid.MainView = this.gridView1;
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(812, 287);
            this.grid.TabIndex = 0;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridView1.Appearance.EvenRow.Options.UseBackColor = true;
            this.gridView1.GridControl = this.grid;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsCustomization.AllowGroup = false;
            this.gridView1.OptionsCustomization.AllowQuickHideColumns = false;
            this.gridView1.OptionsCustomization.AllowSort = false;
            this.gridView1.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridView1.OptionsFilter.AllowFilterEditor = false;
            this.gridView1.OptionsFilter.AllowFilterIncrementalSearch = false;
            this.gridView1.OptionsFilter.AllowMRUFilterList = false;
            this.gridView1.OptionsFilter.FilterEditorUseMenuForOperandsAndOperators = false;
            this.gridView1.OptionsFind.AllowFindPanel = false;
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsMenu.EnableFooterMenu = false;
            this.gridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView1.OptionsMenu.ShowAddNewSummaryItem = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridView1.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridView1.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridView1.OptionsMenu.ShowSplitItem = false;
            this.gridView1.OptionsView.ShowDetailButtons = false;
            this.gridView1.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            // 
            // grpUserRights
            // 
            this.grpUserRights.Controls.Add(this.btnClose);
            this.grpUserRights.Controls.Add(this.btnCancel);
            this.grpUserRights.Controls.Add(this.btnDelete);
            this.grpUserRights.Controls.Add(this.btnUpdate);
            this.grpUserRights.Controls.Add(this.btnAdd);
            this.grpUserRights.Location = new System.Drawing.Point(12, 111);
            this.grpUserRights.Name = "grpUserRights";
            this.grpUserRights.Size = new System.Drawing.Size(818, 52);
            this.grpUserRights.TabIndex = 23;
            this.grpUserRights.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Cornsilk;
            this.btnClose.Location = new System.Drawing.Point(520, 14);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 32);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Clos&e";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Cornsilk;
            this.btnCancel.Location = new System.Drawing.Point(440, 14);
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
            this.btnDelete.Location = new System.Drawing.Point(359, 14);
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
            this.btnUpdate.Location = new System.Drawing.Point(278, 14);
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
            this.btnAdd.Location = new System.Drawing.Point(197, 14);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 32);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // frmMastCostCodeSanManPower
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 488);
            this.Controls.Add(this.grpUserRights);
            this.Controls.Add(this.Group2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmMastCostCodeSanManPower";
            this.Text = "Cost Center Wise Sanction Manpower Master";
            this.Load += new System.EventHandler(this.frmMastCostCodeSanManPower_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtContEmp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompEmp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValidFrom.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValidFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostCode.Properties)).EndInit();
            this.Group2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.grpUserRights.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit txtCostCode;
        private DevExpress.XtraEditors.TextEdit txtDescription;
        private System.Windows.Forms.GroupBox Group2;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.GroupBox grpUserRights;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.DateEdit txtValidFrom;
        private DevExpress.XtraEditors.TextEdit txtContEmp;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.TextEdit txtCompEmp;
        private System.Windows.Forms.Label label2;
    }
}