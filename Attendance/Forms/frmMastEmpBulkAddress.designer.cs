﻿namespace Attendance.Forms
{
    partial class frmMastEmpBulkAddress
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
            this.AddressType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Area = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Society = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Village = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Taluka = new DevExpress.XtraGrid.Columns.GridColumn();
            this.City = new DevExpress.XtraGrid.Columns.GridColumn();
            this.District = new DevExpress.XtraGrid.Columns.GridColumn();
            this.State = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PinCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Phone = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.AddressType,
            this.Area,
            this.Society,
            this.Village,
            this.Taluka,
            this.City,
            this.District,
            this.State,
            this.PinCode,
            this.Phone,
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
            // AddressType
            // 
            this.AddressType.Caption = "AddressType";
            this.AddressType.FieldName = "AddressType";
            this.AddressType.Name = "AddressType";
            this.AddressType.OptionsColumn.AllowEdit = false;
            this.AddressType.OptionsColumn.AllowMove = false;
            this.AddressType.OptionsColumn.ReadOnly = true;
            this.AddressType.UnboundDataType = typeof(string);
            this.AddressType.Visible = true;
            this.AddressType.VisibleIndex = 1;
            this.AddressType.Width = 72;
            // 
            // Area
            // 
            this.Area.Caption = "Area";
            this.Area.FieldName = "Area";
            this.Area.Name = "Area";
            this.Area.OptionsColumn.AllowEdit = false;
            this.Area.OptionsColumn.AllowMove = false;
            this.Area.OptionsColumn.ReadOnly = true;
            this.Area.UnboundDataType = typeof(string);
            this.Area.Visible = true;
            this.Area.VisibleIndex = 2;
            this.Area.Width = 104;
            // 
            // Society
            // 
            this.Society.Caption = "Society";
            this.Society.FieldName = "Society";
            this.Society.Name = "Society";
            this.Society.OptionsColumn.AllowEdit = false;
            this.Society.OptionsColumn.AllowMove = false;
            this.Society.OptionsColumn.ReadOnly = true;
            this.Society.UnboundDataType = typeof(string);
            this.Society.Visible = true;
            this.Society.VisibleIndex = 3;
            this.Society.Width = 155;
            // 
            // Village
            // 
            this.Village.Caption = "Village";
            this.Village.FieldName = "Village";
            this.Village.Name = "Village";
            this.Village.OptionsColumn.AllowEdit = false;
            this.Village.OptionsColumn.AllowMove = false;
            this.Village.OptionsColumn.ReadOnly = true;
            this.Village.UnboundDataType = typeof(string);
            this.Village.Visible = true;
            this.Village.VisibleIndex = 4;
            this.Village.Width = 109;
            // 
            // Taluka
            // 
            this.Taluka.Caption = "Taluka";
            this.Taluka.FieldName = "Taluka";
            this.Taluka.Name = "Taluka";
            this.Taluka.OptionsColumn.AllowEdit = false;
            this.Taluka.OptionsColumn.AllowMove = false;
            this.Taluka.OptionsColumn.ReadOnly = true;
            this.Taluka.UnboundDataType = typeof(string);
            this.Taluka.Visible = true;
            this.Taluka.VisibleIndex = 5;
            this.Taluka.Width = 85;
            // 
            // City
            // 
            this.City.Caption = "City";
            this.City.FieldName = "City";
            this.City.Name = "City";
            this.City.OptionsColumn.AllowEdit = false;
            this.City.OptionsColumn.AllowMove = false;
            this.City.OptionsColumn.ReadOnly = true;
            this.City.UnboundDataType = typeof(string);
            this.City.Visible = true;
            this.City.VisibleIndex = 6;
            this.City.Width = 64;
            // 
            // District
            // 
            this.District.Caption = "District";
            this.District.FieldName = "District";
            this.District.Name = "District";
            this.District.OptionsColumn.AllowEdit = false;
            this.District.OptionsColumn.AllowMove = false;
            this.District.OptionsColumn.ReadOnly = true;
            this.District.UnboundDataType = typeof(string);
            this.District.Visible = true;
            this.District.VisibleIndex = 7;
            this.District.Width = 85;
            // 
            // State
            // 
            this.State.Caption = "State";
            this.State.FieldName = "State";
            this.State.Name = "State";
            this.State.OptionsColumn.AllowEdit = false;
            this.State.OptionsColumn.AllowMove = false;
            this.State.OptionsColumn.ReadOnly = true;
            this.State.UnboundDataType = typeof(string);
            this.State.Visible = true;
            this.State.VisibleIndex = 8;
            // 
            // PinCode
            // 
            this.PinCode.Caption = "PinCode";
            this.PinCode.FieldName = "PinCode";
            this.PinCode.Name = "PinCode";
            this.PinCode.OptionsColumn.AllowEdit = false;
            this.PinCode.OptionsColumn.AllowMove = false;
            this.PinCode.OptionsColumn.ReadOnly = true;
            this.PinCode.UnboundDataType = typeof(string);
            this.PinCode.Visible = true;
            this.PinCode.VisibleIndex = 9;
            this.PinCode.Width = 74;
            // 
            // Phone
            // 
            this.Phone.Caption = "Phone";
            this.Phone.FieldName = "Phone";
            this.Phone.Name = "Phone";
            this.Phone.OptionsColumn.AllowEdit = false;
            this.Phone.OptionsColumn.AllowMove = false;
            this.Phone.OptionsColumn.ReadOnly = true;
            this.Phone.UnboundDataType = typeof(string);
            this.Phone.Visible = true;
            this.Phone.VisibleIndex = 10;
            this.Phone.Width = 64;
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
            this.Remarks.VisibleIndex = 11;
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
            // frmMastEmpBulkAddress
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 602);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmMastEmpBulkAddress";
            this.Text = "Employee Import Address";
            this.Load += new System.EventHandler(this.frmMastEmpBulkAddress_Load);
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
        private DevExpress.XtraGrid.Columns.GridColumn AddressType;
        private DevExpress.XtraGrid.Columns.GridColumn Area;
        private DevExpress.XtraGrid.Columns.GridColumn Society;
        private DevExpress.XtraGrid.Columns.GridColumn Village;
        private DevExpress.XtraGrid.Columns.GridColumn Taluka;
        private DevExpress.XtraGrid.Columns.GridColumn City;
        private DevExpress.XtraGrid.Columns.GridColumn District;
        private DevExpress.XtraGrid.Columns.GridColumn State;
        private DevExpress.XtraGrid.Columns.GridColumn PinCode;
        private DevExpress.XtraGrid.Columns.GridColumn Phone;
    }
}