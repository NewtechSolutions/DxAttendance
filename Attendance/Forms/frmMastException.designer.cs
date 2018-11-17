namespace Attendance.Forms
{
    partial class frmMastException
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkGateInOut = new DevExpress.XtraEditors.CheckEdit();
            this.txtMaxOt = new DevExpress.XtraEditors.CalcEdit();
            this.chkMaxOT = new DevExpress.XtraEditors.CheckEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPshift = new DevExpress.XtraEditors.TextEdit();
            this.txtEmpName = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEmpUnqID = new DevExpress.XtraEditors.TextEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.txtWrkGrpCode = new DevExpress.XtraEditors.TextEdit();
            this.grpUserRights = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.chkGrace = new DevExpress.XtraEditors.CheckEdit();
            this.chkEarlyGoing = new DevExpress.XtraEditors.CheckEdit();
            this.chkLateCome = new DevExpress.XtraEditors.CheckEdit();
            this.chkHalfDay = new DevExpress.XtraEditors.CheckEdit();
            this.chkAutoOut = new DevExpress.XtraEditors.CheckEdit();
            this.Group2 = new System.Windows.Forms.GroupBox();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.chkLunchInOut = new DevExpress.XtraEditors.CheckEdit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkGateInOut.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxOt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMaxOT.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPshift.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpUnqID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWrkGrpCode.Properties)).BeginInit();
            this.grpUserRights.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkGrace.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEarlyGoing.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkLateCome.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkHalfDay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoOut.Properties)).BeginInit();
            this.Group2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkLunchInOut.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkLunchInOut);
            this.groupBox1.Controls.Add(this.chkGateInOut);
            this.groupBox1.Controls.Add(this.txtMaxOt);
            this.groupBox1.Controls.Add(this.chkMaxOT);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtPshift);
            this.groupBox1.Controls.Add(this.txtEmpName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtEmpUnqID);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtWrkGrpCode);
            this.groupBox1.Controls.Add(this.grpUserRights);
            this.groupBox1.Controls.Add(this.chkGrace);
            this.groupBox1.Controls.Add(this.chkEarlyGoing);
            this.groupBox1.Controls.Add(this.chkLateCome);
            this.groupBox1.Controls.Add(this.chkHalfDay);
            this.groupBox1.Controls.Add(this.chkAutoOut);
            this.groupBox1.Location = new System.Drawing.Point(12, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(865, 174);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // chkGateInOut
            // 
            this.chkGateInOut.Location = new System.Drawing.Point(7, 48);
            this.chkGateInOut.Name = "chkGateInOut";
            this.chkGateInOut.Properties.Caption = "Exclude From GateInOut :";
            this.chkGateInOut.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.chkGateInOut.Size = new System.Drawing.Size(172, 19);
            this.chkGateInOut.TabIndex = 3;
            // 
            // txtMaxOt
            // 
            this.txtMaxOt.Location = new System.Drawing.Point(601, 49);
            this.txtMaxOt.Name = "txtMaxOt";
            this.txtMaxOt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, false, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.txtMaxOt.Properties.Mask.EditMask = "f0";
            this.txtMaxOt.Properties.MaxLength = 2;
            this.txtMaxOt.Properties.Precision = 0;
            this.txtMaxOt.Properties.ShowPopupShadow = false;
            this.txtMaxOt.Size = new System.Drawing.Size(65, 20);
            this.txtMaxOt.TabIndex = 5;
            this.txtMaxOt.Visible = false;
            // 
            // chkMaxOT
            // 
            this.chkMaxOT.Location = new System.Drawing.Point(500, 49);
            this.chkMaxOT.Name = "chkMaxOT";
            this.chkMaxOT.Properties.Caption = "Consider MAX OT :";
            this.chkMaxOT.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.chkMaxOT.Size = new System.Drawing.Size(96, 19);
            this.chkMaxOT.TabIndex = 4;
            this.chkMaxOT.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(785, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 15);
            this.label4.TabIndex = 67;
            this.label4.Text = "(F1/F2)";
            this.label4.Visible = false;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(832, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 43);
            this.label2.TabIndex = 66;
            this.label2.Text = "* Prefered Shifts only works if two or more shift starts the same time in Auto Sh" +
    "ift Calc";
            this.label2.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(628, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 15);
            this.label1.TabIndex = 65;
            this.label1.Text = "Prefered Shifts :";
            this.label1.Visible = false;
            // 
            // txtPshift
            // 
            this.txtPshift.EditValue = "";
            this.txtPshift.Location = new System.Drawing.Point(683, 20);
            this.txtPshift.Name = "txtPshift";
            this.txtPshift.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPshift.Properties.Appearance.Options.UseFont = true;
            this.txtPshift.Properties.Mask.EditMask = "[A-Z,]+";
            this.txtPshift.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtPshift.Properties.MaxLength = 5;
            this.txtPshift.Properties.ReadOnly = true;
            this.txtPshift.Size = new System.Drawing.Size(96, 20);
            this.txtPshift.TabIndex = 3;
            this.txtPshift.ToolTip = "Use F1 ->Prefered Shift (where shifts start with same time in auto shift calc)";
            this.txtPshift.Visible = false;
            this.txtPshift.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPshift_KeyDown);
            // 
            // txtEmpName
            // 
            this.txtEmpName.Location = new System.Drawing.Point(185, 18);
            this.txtEmpName.Name = "txtEmpName";
            this.txtEmpName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmpName.Properties.Appearance.Options.UseFont = true;
            this.txtEmpName.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtEmpName.Properties.AppearanceReadOnly.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtEmpName.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            this.txtEmpName.Properties.Mask.EditMask = "[A-Z .]+";
            this.txtEmpName.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtEmpName.Properties.MaxLength = 100;
            this.txtEmpName.Properties.ReadOnly = true;
            this.txtEmpName.Size = new System.Drawing.Size(245, 20);
            this.txtEmpName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 15);
            this.label3.TabIndex = 63;
            this.label3.Text = "EmpUnqID :";
            // 
            // txtEmpUnqID
            // 
            this.txtEmpUnqID.EditValue = "20005890";
            this.txtEmpUnqID.Location = new System.Drawing.Point(83, 18);
            this.txtEmpUnqID.Name = "txtEmpUnqID";
            this.txtEmpUnqID.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmpUnqID.Properties.Appearance.Options.UseFont = true;
            this.txtEmpUnqID.Properties.Mask.EditMask = "[0-9]+";
            this.txtEmpUnqID.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtEmpUnqID.Properties.MaxLength = 10;
            this.txtEmpUnqID.Size = new System.Drawing.Size(96, 20);
            this.txtEmpUnqID.TabIndex = 0;
            this.txtEmpUnqID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtEmpUnqID_KeyDown);
            this.txtEmpUnqID.Validated += new System.EventHandler(this.txtEmpUnqID_Validated);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(436, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 15);
            this.label8.TabIndex = 32;
            this.label8.Text = "WrkGrpCode :";
            // 
            // txtWrkGrpCode
            // 
            this.txtWrkGrpCode.Location = new System.Drawing.Point(520, 20);
            this.txtWrkGrpCode.Name = "txtWrkGrpCode";
            this.txtWrkGrpCode.Properties.Mask.ShowPlaceHolders = false;
            this.txtWrkGrpCode.Properties.ReadOnly = true;
            this.txtWrkGrpCode.Size = new System.Drawing.Size(96, 20);
            this.txtWrkGrpCode.TabIndex = 2;
            // 
            // grpUserRights
            // 
            this.grpUserRights.Controls.Add(this.btnClose);
            this.grpUserRights.Controls.Add(this.btnCancel);
            this.grpUserRights.Controls.Add(this.btnDelete);
            this.grpUserRights.Controls.Add(this.btnUpdate);
            this.grpUserRights.Controls.Add(this.btnAdd);
            this.grpUserRights.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpUserRights.Location = new System.Drawing.Point(3, 119);
            this.grpUserRights.Name = "grpUserRights";
            this.grpUserRights.Size = new System.Drawing.Size(859, 52);
            this.grpUserRights.TabIndex = 22;
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
            // chkGrace
            // 
            this.chkGrace.Location = new System.Drawing.Point(495, 71);
            this.chkGrace.Name = "chkGrace";
            this.chkGrace.Properties.Caption = "Do Not Consider Grace Period :";
            this.chkGrace.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.chkGrace.Size = new System.Drawing.Size(171, 19);
            this.chkGrace.TabIndex = 10;
            this.chkGrace.Visible = false;
            // 
            // chkEarlyGoing
            // 
            this.chkEarlyGoing.Location = new System.Drawing.Point(688, 71);
            this.chkEarlyGoing.Name = "chkEarlyGoing";
            this.chkEarlyGoing.Properties.Caption = "Do Not Consider Early Going :";
            this.chkEarlyGoing.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.chkEarlyGoing.Size = new System.Drawing.Size(172, 19);
            this.chkEarlyGoing.TabIndex = 8;
            this.chkEarlyGoing.Visible = false;
            // 
            // chkLateCome
            // 
            this.chkLateCome.Location = new System.Drawing.Point(689, 96);
            this.chkLateCome.Name = "chkLateCome";
            this.chkLateCome.Properties.Caption = "Do Not Consider LateCome :";
            this.chkLateCome.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.chkLateCome.Size = new System.Drawing.Size(171, 19);
            this.chkLateCome.TabIndex = 9;
            this.chkLateCome.Visible = false;
            // 
            // chkHalfDay
            // 
            this.chkHalfDay.Location = new System.Drawing.Point(495, 96);
            this.chkHalfDay.Name = "chkHalfDay";
            this.chkHalfDay.Properties.Caption = "Do Not Consider Half Day :";
            this.chkHalfDay.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.chkHalfDay.Size = new System.Drawing.Size(171, 19);
            this.chkHalfDay.TabIndex = 7;
            this.chkHalfDay.Visible = false;
            // 
            // chkAutoOut
            // 
            this.chkAutoOut.Location = new System.Drawing.Point(687, 48);
            this.chkAutoOut.Name = "chkAutoOut";
            this.chkAutoOut.Properties.Caption = "Auto Out if Single Punch";
            this.chkAutoOut.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.chkAutoOut.Size = new System.Drawing.Size(173, 19);
            this.chkAutoOut.TabIndex = 6;
            this.chkAutoOut.Visible = false;
            // 
            // Group2
            // 
            this.Group2.Controls.Add(this.grid);
            this.Group2.Location = new System.Drawing.Point(12, 174);
            this.Group2.Name = "Group2";
            this.Group2.Size = new System.Drawing.Size(865, 322);
            this.Group2.TabIndex = 3;
            this.Group2.TabStop = false;
            // 
            // grid
            // 
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(3, 17);
            this.grid.MainView = this.gridView1;
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(859, 302);
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
            // chkLunchInOut
            // 
            this.chkLunchInOut.Location = new System.Drawing.Point(7, 71);
            this.chkLunchInOut.Name = "chkLunchInOut";
            this.chkLunchInOut.Properties.Caption = "Exclude From LunchInOut :";
            this.chkLunchInOut.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.chkLunchInOut.Size = new System.Drawing.Size(172, 19);
            this.chkLunchInOut.TabIndex = 68;
            // 
            // frmMastException
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 508);
            this.Controls.Add(this.Group2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmMastException";
            this.Text = "Exception Configuration Master";
            this.Load += new System.EventHandler(this.frmMastException_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkGateInOut.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxOt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMaxOT.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPshift.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpUnqID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWrkGrpCode.Properties)).EndInit();
            this.grpUserRights.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkGrace.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEarlyGoing.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkLateCome.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkHalfDay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoOut.Properties)).EndInit();
            this.Group2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkLunchInOut.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.CheckEdit chkGrace;
        private DevExpress.XtraEditors.CheckEdit chkEarlyGoing;
        private DevExpress.XtraEditors.CheckEdit chkLateCome;
        private DevExpress.XtraEditors.CheckEdit chkHalfDay;
        private DevExpress.XtraEditors.CheckEdit chkAutoOut;
        private System.Windows.Forms.GroupBox Group2;
        private System.Windows.Forms.GroupBox grpUserRights;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.TextEdit txtWrkGrpCode;
        private DevExpress.XtraEditors.TextEdit txtEmpName;
        private System.Windows.Forms.Label label3;
        public DevExpress.XtraEditors.TextEdit txtEmpUnqID;
        private System.Windows.Forms.Label label1;
        public DevExpress.XtraEditors.TextEdit txtPshift;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.CalcEdit txtMaxOt;
        private DevExpress.XtraEditors.CheckEdit chkMaxOT;
        private DevExpress.XtraEditors.CheckEdit chkGateInOut;
        private DevExpress.XtraEditors.CheckEdit chkLunchInOut;
    }
}