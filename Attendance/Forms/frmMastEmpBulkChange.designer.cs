namespace Attendance.Forms
{
    partial class frmMastEmpBulkChange
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.grd_view = new DevExpress.XtraGrid.GridControl();
            this.grd_view1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.EmpUnqID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ValidFrom = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CatCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GradeCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DesgCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.UnitCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DeptCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.StatCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ContCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CostCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.WeekOff = new DevExpress.XtraGrid.Columns.GridColumn();
            this.OTFLG = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Remarks = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtBrowse = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd_view)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grd_view1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupControl2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupControl1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.1196F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85.8804F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(945, 602);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupControl2
            // 
            this.groupControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl2.Appearance.Options.UseFont = true;
            this.groupControl2.Controls.Add(this.grd_view);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(3, 87);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(939, 512);
            this.groupControl2.TabIndex = 1;
            this.groupControl2.Text = "Data";
            // 
            // grd_view
            // 
            this.grd_view.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd_view.Location = new System.Drawing.Point(2, 23);
            this.grd_view.MainView = this.grd_view1;
            this.grd_view.Name = "grd_view";
            this.grd_view.Size = new System.Drawing.Size(935, 487);
            this.grd_view.TabIndex = 2;
            this.grd_view.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grd_view1});
            // 
            // grd_view1
            // 
            this.grd_view1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.EmpUnqID,
            this.ValidFrom,
            this.CatCode,
            this.GradeCode,
            this.DesgCode,
            this.UnitCode,
            this.DeptCode,
            this.StatCode,
            this.ContCode,
            this.CostCode,
            this.WeekOff,
            this.OTFLG,
            this.Remarks});
            this.grd_view1.GridControl = this.grd_view;
            this.grd_view1.Name = "grd_view1";
            this.grd_view1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.grd_view1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.grd_view1.OptionsBehavior.Editable = false;
            this.grd_view1.OptionsBehavior.ReadOnly = true;
            this.grd_view1.OptionsCustomization.AllowColumnMoving = false;
            this.grd_view1.OptionsCustomization.AllowFilter = false;
            this.grd_view1.OptionsCustomization.AllowGroup = false;
            this.grd_view1.OptionsCustomization.AllowQuickHideColumns = false;
            this.grd_view1.OptionsCustomization.AllowRowSizing = true;
            this.grd_view1.OptionsCustomization.AllowSort = false;
            this.grd_view1.OptionsDetail.AllowZoomDetail = false;
            this.grd_view1.OptionsDetail.EnableMasterViewMode = false;
            this.grd_view1.OptionsDetail.ShowDetailTabs = false;
            this.grd_view1.OptionsDetail.SmartDetailExpand = false;
            this.grd_view1.OptionsMenu.EnableColumnMenu = false;
            this.grd_view1.OptionsMenu.EnableFooterMenu = false;
            this.grd_view1.OptionsMenu.EnableGroupPanelMenu = false;
            this.grd_view1.OptionsMenu.ShowAddNewSummaryItem = DevExpress.Utils.DefaultBoolean.False;
            this.grd_view1.OptionsMenu.ShowAutoFilterRowItem = false;
            this.grd_view1.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.grd_view1.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.grd_view1.OptionsMenu.ShowSplitItem = false;
            this.grd_view1.OptionsNavigation.EnterMoveNextColumn = true;
            this.grd_view1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.grd_view1.OptionsView.ShowDetailButtons = false;
            this.grd_view1.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.grd_view1.OptionsView.ShowGroupPanel = false;
            // 
            // EmpUnqID
            // 
            this.EmpUnqID.Caption = "EmpUnqID";
            this.EmpUnqID.FieldName = "EmpUnqID";
            this.EmpUnqID.Name = "EmpUnqID";
            this.EmpUnqID.OptionsColumn.AllowEdit = false;
            this.EmpUnqID.OptionsColumn.AllowMove = false;
            this.EmpUnqID.OptionsColumn.ReadOnly = true;
            this.EmpUnqID.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.EmpUnqID.Visible = true;
            this.EmpUnqID.VisibleIndex = 0;
            this.EmpUnqID.Width = 74;
            // 
            // ValidFrom
            // 
            this.ValidFrom.Caption = "ValidFrom";
            this.ValidFrom.FieldName = "ValidFrom";
            this.ValidFrom.Name = "ValidFrom";
            this.ValidFrom.OptionsColumn.AllowEdit = false;
            this.ValidFrom.OptionsColumn.AllowMove = false;
            this.ValidFrom.OptionsColumn.ReadOnly = true;
            this.ValidFrom.UnboundDataType = typeof(string);
            this.ValidFrom.Visible = true;
            this.ValidFrom.VisibleIndex = 1;
            this.ValidFrom.Width = 96;
            // 
            // CatCode
            // 
            this.CatCode.Caption = "CatCode";
            this.CatCode.FieldName = "CatCode";
            this.CatCode.Name = "CatCode";
            this.CatCode.OptionsColumn.AllowEdit = false;
            this.CatCode.OptionsColumn.AllowMove = false;
            this.CatCode.OptionsColumn.ReadOnly = true;
            this.CatCode.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.CatCode.Visible = true;
            this.CatCode.VisibleIndex = 2;
            this.CatCode.Width = 68;
            // 
            // GradeCode
            // 
            this.GradeCode.Caption = "GradeCode";
            this.GradeCode.FieldName = "GradeCode";
            this.GradeCode.Name = "GradeCode";
            this.GradeCode.OptionsColumn.AllowEdit = false;
            this.GradeCode.OptionsColumn.AllowMove = false;
            this.GradeCode.OptionsColumn.ReadOnly = true;
            this.GradeCode.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.GradeCode.Visible = true;
            this.GradeCode.VisibleIndex = 3;
            // 
            // DesgCode
            // 
            this.DesgCode.Caption = "DesgCode";
            this.DesgCode.FieldName = "DesgCode";
            this.DesgCode.Name = "DesgCode";
            this.DesgCode.OptionsColumn.AllowEdit = false;
            this.DesgCode.OptionsColumn.AllowMove = false;
            this.DesgCode.OptionsColumn.ReadOnly = true;
            this.DesgCode.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.DesgCode.Visible = true;
            this.DesgCode.VisibleIndex = 4;
            this.DesgCode.Width = 73;
            // 
            // UnitCode
            // 
            this.UnitCode.Caption = "UnitCode";
            this.UnitCode.FieldName = "UnitCode";
            this.UnitCode.Name = "UnitCode";
            this.UnitCode.OptionsColumn.AllowEdit = false;
            this.UnitCode.OptionsColumn.AllowMove = false;
            this.UnitCode.OptionsColumn.ReadOnly = true;
            this.UnitCode.UnboundDataType = typeof(string);
            this.UnitCode.Visible = true;
            this.UnitCode.VisibleIndex = 5;
            this.UnitCode.Width = 60;
            // 
            // DeptCode
            // 
            this.DeptCode.Caption = "DeptCode";
            this.DeptCode.FieldName = "DeptCode";
            this.DeptCode.Name = "DeptCode";
            this.DeptCode.OptionsColumn.AllowEdit = false;
            this.DeptCode.OptionsColumn.AllowMove = false;
            this.DeptCode.OptionsColumn.ReadOnly = true;
            this.DeptCode.UnboundDataType = typeof(string);
            this.DeptCode.Visible = true;
            this.DeptCode.VisibleIndex = 6;
            this.DeptCode.Width = 60;
            // 
            // StatCode
            // 
            this.StatCode.Caption = "StatCode";
            this.StatCode.FieldName = "StatCode";
            this.StatCode.Name = "StatCode";
            this.StatCode.OptionsColumn.AllowEdit = false;
            this.StatCode.OptionsColumn.AllowMove = false;
            this.StatCode.OptionsColumn.ReadOnly = true;
            this.StatCode.UnboundDataType = typeof(string);
            this.StatCode.Visible = true;
            this.StatCode.VisibleIndex = 7;
            this.StatCode.Width = 60;
            // 
            // ContCode
            // 
            this.ContCode.Caption = "ContCode";
            this.ContCode.FieldName = "ContCode";
            this.ContCode.Name = "ContCode";
            this.ContCode.OptionsColumn.AllowEdit = false;
            this.ContCode.OptionsColumn.AllowMove = false;
            this.ContCode.OptionsColumn.ReadOnly = true;
            this.ContCode.UnboundDataType = typeof(string);
            this.ContCode.Visible = true;
            this.ContCode.VisibleIndex = 8;
            this.ContCode.Width = 60;
            // 
            // CostCode
            // 
            this.CostCode.Caption = "CostCode";
            this.CostCode.FieldName = "CostCode";
            this.CostCode.Name = "CostCode";
            this.CostCode.OptionsColumn.AllowEdit = false;
            this.CostCode.OptionsColumn.AllowMove = false;
            this.CostCode.OptionsColumn.ReadOnly = true;
            this.CostCode.UnboundDataType = typeof(string);
            this.CostCode.Visible = true;
            this.CostCode.VisibleIndex = 9;
            this.CostCode.Width = 60;
            // 
            // WeekOff
            // 
            this.WeekOff.Caption = "WeekOff";
            this.WeekOff.FieldName = "WeekOff";
            this.WeekOff.Name = "WeekOff";
            this.WeekOff.OptionsColumn.AllowEdit = false;
            this.WeekOff.OptionsColumn.AllowMove = false;
            this.WeekOff.OptionsColumn.ReadOnly = true;
            this.WeekOff.UnboundDataType = typeof(string);
            this.WeekOff.Visible = true;
            this.WeekOff.VisibleIndex = 10;
            this.WeekOff.Width = 60;
            // 
            // OTFLG
            // 
            this.OTFLG.Caption = "OTFLG";
            this.OTFLG.FieldName = "OTFLG";
            this.OTFLG.Name = "OTFLG";
            this.OTFLG.OptionsColumn.AllowEdit = false;
            this.OTFLG.OptionsColumn.AllowMove = false;
            this.OTFLG.OptionsColumn.ReadOnly = true;
            this.OTFLG.UnboundDataType = typeof(string);
            this.OTFLG.Visible = true;
            this.OTFLG.VisibleIndex = 11;
            this.OTFLG.Width = 60;
            // 
            // Remarks
            // 
            this.Remarks.Caption = "Remarks";
            this.Remarks.FieldName = "Remarks";
            this.Remarks.Name = "Remarks";
            this.Remarks.OptionsColumn.AllowEdit = false;
            this.Remarks.OptionsColumn.AllowMove = false;
            this.Remarks.OptionsColumn.ReadOnly = true;
            this.Remarks.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.Remarks.Visible = true;
            this.Remarks.VisibleIndex = 12;
            this.Remarks.Width = 255;
            // 
            // groupControl1
            // 
            this.groupControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl1.Appearance.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.btnExport);
            this.groupControl1.Controls.Add(this.btnPreview);
            this.groupControl1.Controls.Add(this.btnImport);
            this.groupControl1.Controls.Add(this.btnBrowse);
            this.groupControl1.Controls.Add(this.txtBrowse);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(3, 3);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(939, 78);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Import File";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(827, 35);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(103, 23);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "E&xport";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(640, 35);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 23);
            this.btnPreview.TabIndex = 3;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(721, 35);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(103, 23);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(559, 35);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtBrowse
            // 
            this.txtBrowse.Location = new System.Drawing.Point(9, 35);
            this.txtBrowse.Name = "txtBrowse";
            this.txtBrowse.Size = new System.Drawing.Size(543, 23);
            this.txtBrowse.TabIndex = 0;
            // 
            // frmMastEmpBulkChange
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 602);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmMastEmpBulkChange";
            this.Text = "Employee Bulk Attribute Change";
            this.Load += new System.EventHandler(this.frmMastEmpBulkChange_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd_view)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grd_view1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtBrowse;
        private DevExpress.XtraGrid.GridControl grd_view;
        private DevExpress.XtraGrid.Views.Grid.GridView grd_view1;
        private DevExpress.XtraGrid.Columns.GridColumn EmpUnqID;
        private System.Windows.Forms.Button btnExport;
        private DevExpress.XtraGrid.Columns.GridColumn CatCode;
        private DevExpress.XtraGrid.Columns.GridColumn GradeCode;
        private DevExpress.XtraGrid.Columns.GridColumn DesgCode;
        private DevExpress.XtraGrid.Columns.GridColumn Remarks;
        private DevExpress.XtraGrid.Columns.GridColumn ValidFrom;
        private DevExpress.XtraGrid.Columns.GridColumn UnitCode;
        private DevExpress.XtraGrid.Columns.GridColumn DeptCode;
        private DevExpress.XtraGrid.Columns.GridColumn StatCode;
        private DevExpress.XtraGrid.Columns.GridColumn ContCode;
        private DevExpress.XtraGrid.Columns.GridColumn CostCode;
        private DevExpress.XtraGrid.Columns.GridColumn WeekOff;
        private DevExpress.XtraGrid.Columns.GridColumn OTFLG;
    }
}