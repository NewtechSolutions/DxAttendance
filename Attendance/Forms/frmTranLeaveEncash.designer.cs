namespace Attendance.Forms
{
    partial class frmTranLeaveEncash
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
            this.GrpMain = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtAvlBal = new DevExpress.XtraEditors.CalcEdit();
            this.txtYear = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEncashDt = new DevExpress.XtraEditors.DateEdit();
            this.btnClose = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtEncDay = new DevExpress.XtraEditors.CalcEdit();
            this.Group2 = new System.Windows.Forms.GroupBox();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLeaveType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ctrlEmp1 = new Attendance.ctrlEmp();
            this.GrpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAvlBal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtYear.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEncashDt.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEncashDt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEncDay.Properties)).BeginInit();
            this.Group2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLeaveType.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // GrpMain
            // 
            this.GrpMain.Controls.Add(this.label6);
            this.GrpMain.Controls.Add(this.txtAvlBal);
            this.GrpMain.Controls.Add(this.txtYear);
            this.GrpMain.Controls.Add(this.label5);
            this.GrpMain.Controls.Add(this.btnDelete);
            this.GrpMain.Controls.Add(this.label2);
            this.GrpMain.Controls.Add(this.txtEncashDt);
            this.GrpMain.Controls.Add(this.btnClose);
            this.GrpMain.Controls.Add(this.label3);
            this.GrpMain.Controls.Add(this.btnCancel);
            this.GrpMain.Controls.Add(this.btnAdd);
            this.GrpMain.Controls.Add(this.label1);
            this.GrpMain.Controls.Add(this.txtEncDay);
            this.GrpMain.Controls.Add(this.Group2);
            this.GrpMain.Controls.Add(this.label4);
            this.GrpMain.Controls.Add(this.txtLeaveType);
            this.GrpMain.Location = new System.Drawing.Point(13, 191);
            this.GrpMain.Name = "GrpMain";
            this.GrpMain.Size = new System.Drawing.Size(922, 227);
            this.GrpMain.TabIndex = 1;
            this.GrpMain.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(286, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 15);
            this.label6.TabIndex = 46;
            this.label6.Text = "(Use F1/F2 View )";
            // 
            // txtAvlBal
            // 
            this.txtAvlBal.Location = new System.Drawing.Point(158, 97);
            this.txtAvlBal.Name = "txtAvlBal";
            this.txtAvlBal.Properties.ReadOnly = true;
            this.txtAvlBal.Size = new System.Drawing.Size(122, 20);
            this.txtAvlBal.TabIndex = 8;
            // 
            // txtYear
            // 
            this.txtYear.Location = new System.Drawing.Point(158, 20);
            this.txtYear.Name = "txtYear";
            this.txtYear.Properties.Mask.EditMask = "d";
            this.txtYear.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtYear.Properties.MaxLength = 4;
            this.txtYear.Size = new System.Drawing.Size(122, 20);
            this.txtYear.TabIndex = 5;
            this.txtYear.Validated += new System.EventHandler(this.txtYear_Validated);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 15);
            this.label5.TabIndex = 42;
            this.label5.Text = "Available Bal :";
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Cornsilk;
            this.btnDelete.Location = new System.Drawing.Point(94, 163);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 32);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 15);
            this.label2.TabIndex = 40;
            this.label2.Text = "Encashment Date :";
            // 
            // txtEncashDt
            // 
            this.txtEncashDt.EditValue = null;
            this.txtEncashDt.Location = new System.Drawing.Point(158, 73);
            this.txtEncashDt.Name = "txtEncashDt";
            this.txtEncashDt.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEncashDt.Properties.Appearance.Options.UseFont = true;
            this.txtEncashDt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtEncashDt.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtEncashDt.Properties.CalendarTimeProperties.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4);
            this.txtEncashDt.Properties.CalendarTimeProperties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.Default;
            this.txtEncashDt.Size = new System.Drawing.Size(122, 20);
            this.txtEncashDt.TabIndex = 7;
            this.txtEncashDt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtEncashDt_KeyDown);
            this.txtEncashDt.Validated += new System.EventHandler(this.txtEncashDt_Validated);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Cornsilk;
            this.btnClose.Location = new System.Drawing.Point(256, 163);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 32);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Clos&e";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 15);
            this.label3.TabIndex = 38;
            this.label3.Text = "Encashed Days :";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Cornsilk;
            this.btnCancel.Location = new System.Drawing.Point(175, 163);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 32);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.Cornsilk;
            this.btnAdd.Location = new System.Drawing.Point(13, 163);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 32);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 15);
            this.label1.TabIndex = 37;
            this.label1.Text = "Leave Type :";
            // 
            // txtEncDay
            // 
            this.txtEncDay.Location = new System.Drawing.Point(158, 121);
            this.txtEncDay.Name = "txtEncDay";
            this.txtEncDay.Size = new System.Drawing.Size(122, 20);
            this.txtEncDay.TabIndex = 9;
            // 
            // Group2
            // 
            this.Group2.Controls.Add(this.grid);
            this.Group2.Location = new System.Drawing.Point(425, 14);
            this.Group2.Name = "Group2";
            this.Group2.Size = new System.Drawing.Size(485, 199);
            this.Group2.TabIndex = 35;
            this.Group2.TabStop = false;
            // 
            // grid
            // 
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(3, 17);
            this.grid.MainView = this.gridView1;
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(479, 179);
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 15);
            this.label4.TabIndex = 36;
            this.label4.Text = "Year :";
            // 
            // txtLeaveType
            // 
            this.txtLeaveType.Location = new System.Drawing.Point(158, 46);
            this.txtLeaveType.Name = "txtLeaveType";
            this.txtLeaveType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtLeaveType.Properties.MaxLength = 100;
            this.txtLeaveType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.txtLeaveType.Size = new System.Drawing.Size(122, 20);
            this.txtLeaveType.TabIndex = 6;
            this.txtLeaveType.TabStop = false;
            this.txtLeaveType.SelectedIndexChanged += new System.EventHandler(this.txtLeaveType_SelectedIndexChanged);
            // 
            // ctrlEmp1
            // 
            this.ctrlEmp1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlEmp1.Location = new System.Drawing.Point(12, 12);
            this.ctrlEmp1.Name = "ctrlEmp1";
            this.ctrlEmp1.Size = new System.Drawing.Size(933, 171);
            this.ctrlEmp1.TabIndex = 3;
            // 
            // frmTranLeaveEncash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(947, 426);
            this.Controls.Add(this.ctrlEmp1);
            this.Controls.Add(this.GrpMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "frmTranLeaveEncash";
            this.Text = "Employee Leave Encashment";
            this.Load += new System.EventHandler(this.frmMastEmpLeaveEncash_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmTranLeaveEncash_KeyDown);
            this.GrpMain.ResumeLayout(false);
            this.GrpMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAvlBal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtYear.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEncashDt.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEncashDt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEncDay.Properties)).EndInit();
            this.Group2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLeaveType.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox GrpMain;
        private ctrlEmp ctrlEmp1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.CalcEdit txtEncDay;
        private System.Windows.Forms.GroupBox Group2;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.ComboBoxEdit txtLeaveType;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.DateEdit txtEncashDt;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.TextEdit txtYear;
        private DevExpress.XtraEditors.CalcEdit txtAvlBal;
        private System.Windows.Forms.Label label6;
    }
}