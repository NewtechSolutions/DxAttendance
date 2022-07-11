namespace Attendance.Forms
{
    partial class frmMastEmpBulkIdentity
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
            this.PassportNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AdharNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DrvLicNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ElectionNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PANNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PFACNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.UANNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.BankAcNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.BankName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Remarks = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtBrowse = new System.Windows.Forms.TextBox();
            this.BankIFSC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.BankBranch = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.PassportNo,
            this.AdharNo,
            this.DrvLicNo,
            this.ElectionNo,
            this.PANNo,
            this.PFACNo,
            this.UANNo,
            this.BankName,
            this.BankAcNo,
            this.BankBranch,
            this.BankIFSC,
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
            this.EmpUnqID.Width = 71;
            // 
            // PassportNo
            // 
            this.PassportNo.Caption = "PassportNo";
            this.PassportNo.FieldName = "PassportNo";
            this.PassportNo.Name = "PassportNo";
            this.PassportNo.OptionsColumn.AllowEdit = false;
            this.PassportNo.OptionsColumn.AllowMove = false;
            this.PassportNo.OptionsColumn.ReadOnly = true;
            this.PassportNo.UnboundDataType = typeof(string);
            this.PassportNo.Visible = true;
            this.PassportNo.VisibleIndex = 1;
            this.PassportNo.Width = 104;
            // 
            // AdharNo
            // 
            this.AdharNo.Caption = "AdharNo";
            this.AdharNo.FieldName = "AdharNo";
            this.AdharNo.Name = "AdharNo";
            this.AdharNo.OptionsColumn.AllowEdit = false;
            this.AdharNo.OptionsColumn.AllowMove = false;
            this.AdharNo.OptionsColumn.ReadOnly = true;
            this.AdharNo.UnboundDataType = typeof(string);
            this.AdharNo.Visible = true;
            this.AdharNo.VisibleIndex = 2;
            this.AdharNo.Width = 107;
            // 
            // DrvLicNo
            // 
            this.DrvLicNo.Caption = "DrvLicNo";
            this.DrvLicNo.FieldName = "DrvLicNo";
            this.DrvLicNo.Name = "DrvLicNo";
            this.DrvLicNo.OptionsColumn.AllowEdit = false;
            this.DrvLicNo.OptionsColumn.AllowMove = false;
            this.DrvLicNo.OptionsColumn.ReadOnly = true;
            this.DrvLicNo.UnboundDataType = typeof(string);
            this.DrvLicNo.Visible = true;
            this.DrvLicNo.VisibleIndex = 3;
            this.DrvLicNo.Width = 109;
            // 
            // ElectionNo
            // 
            this.ElectionNo.Caption = "ElectionNo";
            this.ElectionNo.FieldName = "ElectionNo";
            this.ElectionNo.Name = "ElectionNo";
            this.ElectionNo.OptionsColumn.AllowEdit = false;
            this.ElectionNo.OptionsColumn.AllowMove = false;
            this.ElectionNo.OptionsColumn.ReadOnly = true;
            this.ElectionNo.UnboundDataType = typeof(string);
            this.ElectionNo.Visible = true;
            this.ElectionNo.VisibleIndex = 4;
            this.ElectionNo.Width = 85;
            // 
            // PANNo
            // 
            this.PANNo.Caption = "PANNo";
            this.PANNo.FieldName = "PANNo";
            this.PANNo.Name = "PANNo";
            this.PANNo.OptionsColumn.AllowEdit = false;
            this.PANNo.OptionsColumn.AllowMove = false;
            this.PANNo.OptionsColumn.ReadOnly = true;
            this.PANNo.UnboundDataType = typeof(string);
            this.PANNo.Visible = true;
            this.PANNo.VisibleIndex = 5;
            this.PANNo.Width = 64;
            // 
            // PFACNo
            // 
            this.PFACNo.Caption = "PFACNo";
            this.PFACNo.FieldName = "PFACNo";
            this.PFACNo.Name = "PFACNo";
            this.PFACNo.OptionsColumn.AllowEdit = false;
            this.PFACNo.OptionsColumn.AllowMove = false;
            this.PFACNo.OptionsColumn.ReadOnly = true;
            this.PFACNo.UnboundDataType = typeof(string);
            this.PFACNo.Visible = true;
            this.PFACNo.VisibleIndex = 6;
            this.PFACNo.Width = 85;
            // 
            // UANNo
            // 
            this.UANNo.Caption = "UANNo";
            this.UANNo.FieldName = "UANNo";
            this.UANNo.Name = "UANNo";
            this.UANNo.OptionsColumn.AllowEdit = false;
            this.UANNo.OptionsColumn.AllowMove = false;
            this.UANNo.OptionsColumn.ReadOnly = true;
            this.UANNo.UnboundDataType = typeof(string);
            this.UANNo.Visible = true;
            this.UANNo.VisibleIndex = 7;
            // 
            // BankAcNo
            // 
            this.BankAcNo.Caption = "BankAcNo";
            this.BankAcNo.FieldName = "BankAcNo";
            this.BankAcNo.Name = "BankAcNo";
            this.BankAcNo.OptionsColumn.AllowEdit = false;
            this.BankAcNo.OptionsColumn.AllowMove = false;
            this.BankAcNo.OptionsColumn.ReadOnly = true;
            this.BankAcNo.UnboundDataType = typeof(string);
            this.BankAcNo.Visible = true;
            this.BankAcNo.VisibleIndex = 9;
            this.BankAcNo.Width = 74;
            // 
            // BankName
            // 
            this.BankName.Caption = "BankName";
            this.BankName.FieldName = "BankName";
            this.BankName.Name = "BankName";
            this.BankName.OptionsColumn.AllowEdit = false;
            this.BankName.OptionsColumn.AllowMove = false;
            this.BankName.OptionsColumn.ReadOnly = true;
            this.BankName.UnboundDataType = typeof(string);
            this.BankName.Visible = true;
            this.BankName.VisibleIndex = 8;
            this.BankName.Width = 63;
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
            this.Remarks.Width = 87;
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
            // BankIFSC
            // 
            this.BankIFSC.Caption = "BankIFSC";
            this.BankIFSC.FieldName = "BankIFSC";
            this.BankIFSC.Name = "BankIFSC";
            this.BankIFSC.OptionsColumn.AllowEdit = false;
            this.BankIFSC.OptionsColumn.AllowMove = false;
            this.BankIFSC.OptionsColumn.ReadOnly = true;
            this.BankIFSC.UnboundDataType = typeof(string);
            this.BankIFSC.Visible = true;
            this.BankIFSC.VisibleIndex = 10;
            // 
            // BankBranch
            // 
            this.BankBranch.Caption = "BankBranch";
            this.BankBranch.FieldName = "BankBranch";
            this.BankBranch.Name = "BankBranch";
            this.BankBranch.OptionsColumn.AllowEdit = false;
            this.BankBranch.OptionsColumn.AllowMove = false;
            this.BankBranch.OptionsColumn.ReadOnly = true;
            this.BankBranch.UnboundDataType = typeof(string);
            this.BankBranch.Visible = true;
            this.BankBranch.VisibleIndex = 11;
            // 
            // frmMastEmpBulkIdentity
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 602);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmMastEmpBulkIdentity";
            this.Text = "Employee Import Identities";
            this.Load += new System.EventHandler(this.frmMastEmpBulkIdentity_Load);
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
        private DevExpress.XtraGrid.Columns.GridColumn Remarks;
        private DevExpress.XtraGrid.Columns.GridColumn PassportNo;
        private DevExpress.XtraGrid.Columns.GridColumn AdharNo;
        private DevExpress.XtraGrid.Columns.GridColumn DrvLicNo;
        private DevExpress.XtraGrid.Columns.GridColumn ElectionNo;
        private DevExpress.XtraGrid.Columns.GridColumn PANNo;
        private DevExpress.XtraGrid.Columns.GridColumn PFACNo;
        private DevExpress.XtraGrid.Columns.GridColumn UANNo;
        private DevExpress.XtraGrid.Columns.GridColumn BankAcNo;
        private DevExpress.XtraGrid.Columns.GridColumn BankName;
        private DevExpress.XtraGrid.Columns.GridColumn BankBranch;
        private DevExpress.XtraGrid.Columns.GridColumn BankIFSC;
    }
}