namespace Attendance.Forms
{
    partial class frmOtherConfig
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
            this.btnUpdateSan = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSanDayLimit = new DevExpress.XtraEditors.SpinEdit();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtAutoProccessTime = new DevExpress.XtraEditors.TimeEdit();
            this.label12 = new System.Windows.Forms.Label();
            this.txtAutoProcessWrkGrp = new DevExpress.XtraEditors.TextEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtServerWorkerIP = new DevExpress.XtraEditors.TextEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.txtReportSerExeURL = new DevExpress.XtraEditors.TextEdit();
            this.txtReportServiceURL = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSMTPIP = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEmailID = new DevExpress.XtraEditors.TextEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.btnUpdateNetwork = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnTimeDel = new System.Windows.Forms.Button();
            this.btnTimeAdd = new System.Windows.Forms.Button();
            this.txtTime = new DevExpress.XtraEditors.TimeEdit();
            this.grd_avbl = new DevExpress.XtraGrid.GridControl();
            this.gv_avbl = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.SchTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.label15 = new System.Windows.Forms.Label();
            this.txtLateComeSec = new DevExpress.XtraEditors.SpinEdit();
            this.label16 = new System.Windows.Forms.Label();
            this.txtEarlyComeSec = new DevExpress.XtraEditors.SpinEdit();
            this.label17 = new System.Windows.Forms.Label();
            this.txtEarlyGoingSec = new DevExpress.XtraEditors.SpinEdit();
            this.txtGracePeriodSec = new DevExpress.XtraEditors.SpinEdit();
            this.txtLateHalfDaySec = new DevExpress.XtraEditors.SpinEdit();
            this.chkLateHalfDayFlg = new System.Windows.Forms.CheckBox();
            this.chkEarlyGoingHalfDayFlg = new System.Windows.Forms.CheckBox();
            this.txtEarlyGoingHalfDaySec = new DevExpress.XtraEditors.SpinEdit();
            this.chkGraceHalfDayFlg = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSanDayLimit.Properties)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAutoProccessTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAutoProcessWrkGrp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerWorkerIP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReportSerExeURL.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReportServiceURL.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSMTPIP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmailID.Properties)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grd_avbl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_avbl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLateComeSec.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEarlyComeSec.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEarlyGoingSec.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGracePeriodSec.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLateHalfDaySec.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEarlyGoingHalfDaySec.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkGraceHalfDayFlg);
            this.groupBox1.Controls.Add(this.chkEarlyGoingHalfDayFlg);
            this.groupBox1.Controls.Add(this.txtEarlyGoingHalfDaySec);
            this.groupBox1.Controls.Add(this.chkLateHalfDayFlg);
            this.groupBox1.Controls.Add(this.txtLateHalfDaySec);
            this.groupBox1.Controls.Add(this.txtGracePeriodSec);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.txtEarlyGoingSec);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.txtEarlyComeSec);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.txtLateComeSec);
            this.groupBox1.Controls.Add(this.btnUpdateSan);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtSanDayLimit);
            this.groupBox1.Location = new System.Drawing.Point(12, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(618, 189);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Other Configuration - In Respect of Shift Timing";
            // 
            // btnUpdateSan
            // 
            this.btnUpdateSan.Location = new System.Drawing.Point(503, 141);
            this.btnUpdateSan.Name = "btnUpdateSan";
            this.btnUpdateSan.Size = new System.Drawing.Size(99, 34);
            this.btnUpdateSan.TabIndex = 1;
            this.btnUpdateSan.Text = "UpDate";
            this.btnUpdateSan.UseVisualStyleBackColor = true;
            this.btnUpdateSan.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(260, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(330, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "* Verious App Setting will Mark respective fields in Process.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "Sanction Days Limit :";
            // 
            // txtSanDayLimit
            // 
            this.txtSanDayLimit.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtSanDayLimit.Location = new System.Drawing.Point(138, 22);
            this.txtSanDayLimit.Name = "txtSanDayLimit";
            this.txtSanDayLimit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtSanDayLimit.Size = new System.Drawing.Size(96, 20);
            this.txtSanDayLimit.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.txtAutoProccessTime);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.txtAutoProcessWrkGrp);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.txtServerWorkerIP);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txtReportSerExeURL);
            this.groupBox3.Controls.Add(this.txtReportServiceURL);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.txtSMTPIP);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.txtEmailID);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.btnUpdateNetwork);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Location = new System.Drawing.Point(12, 214);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(618, 275);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Default Network/Scheduling Configuration";
            
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(135, 189);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(311, 14);
            this.label14.TabIndex = 16;
            this.label14.Text = "* ( Comma Seperated WrkGrpCode : Ex. COMP,CONT )";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(19, 217);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(95, 14);
            this.label13.TabIndex = 15;
            this.label13.Text = "Auto Pro Time :";
            // 
            // txtAutoProccessTime
            // 
            this.txtAutoProccessTime.EditValue = new System.DateTime(2017, 12, 1, 0, 0, 0, 0);
            this.txtAutoProccessTime.Location = new System.Drawing.Point(138, 215);
            this.txtAutoProccessTime.Name = "txtAutoProccessTime";
            this.txtAutoProccessTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtAutoProccessTime.Properties.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4);
            this.txtAutoProccessTime.Properties.Mask.EditMask = "HH:mm";
            this.txtAutoProccessTime.Properties.MaxLength = 5;
            this.txtAutoProccessTime.Properties.NullValuePrompt = "Please Enter Time";
            this.txtAutoProccessTime.Properties.NullValuePromptShowForEmptyValue = true;
            this.txtAutoProccessTime.Properties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.Default;
            this.txtAutoProccessTime.Size = new System.Drawing.Size(76, 20);
            this.txtAutoProccessTime.TabIndex = 6;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(17, 168);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(105, 14);
            this.label12.TabIndex = 13;
            this.label12.Text = "Auto ProWrkGrp :";
            // 
            // txtAutoProcessWrkGrp
            // 
            this.txtAutoProcessWrkGrp.Location = new System.Drawing.Point(138, 166);
            this.txtAutoProcessWrkGrp.Name = "txtAutoProcessWrkGrp";
            this.txtAutoProcessWrkGrp.Properties.Mask.EditMask = "[a-zA-Z0-9@./:,]+";
            this.txtAutoProcessWrkGrp.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtAutoProcessWrkGrp.Properties.Mask.ShowPlaceHolders = false;
            this.txtAutoProcessWrkGrp.Size = new System.Drawing.Size(464, 20);
            this.txtAutoProcessWrkGrp.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(392, 143);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(115, 14);
            this.label8.TabIndex = 9;
            this.label8.Text = "* ( Scheduler Host)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 116);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 14);
            this.label6.TabIndex = 11;
            this.label6.Text = "ReportSerExe URL :";
            // 
            // txtServerWorkerIP
            // 
            this.txtServerWorkerIP.Location = new System.Drawing.Point(138, 140);
            this.txtServerWorkerIP.Name = "txtServerWorkerIP";
            this.txtServerWorkerIP.Properties.Mask.EditMask = "[0-9.]+";
            this.txtServerWorkerIP.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtServerWorkerIP.Properties.Mask.ShowPlaceHolders = false;
            this.txtServerWorkerIP.Size = new System.Drawing.Size(248, 20);
            this.txtServerWorkerIP.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 142);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 14);
            this.label7.TabIndex = 7;
            this.label7.Text = "Worker HOST IP :";
            // 
            // txtReportSerExeURL
            // 
            this.txtReportSerExeURL.Location = new System.Drawing.Point(138, 114);
            this.txtReportSerExeURL.Name = "txtReportSerExeURL";
            this.txtReportSerExeURL.Properties.Mask.EditMask = "[a-zA-Z0-9@./:]+";
            this.txtReportSerExeURL.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtReportSerExeURL.Properties.Mask.ShowPlaceHolders = false;
            this.txtReportSerExeURL.Size = new System.Drawing.Size(464, 20);
            this.txtReportSerExeURL.TabIndex = 3;
            // 
            // txtReportServiceURL
            // 
            this.txtReportServiceURL.Location = new System.Drawing.Point(138, 88);
            this.txtReportServiceURL.Name = "txtReportServiceURL";
            this.txtReportServiceURL.Properties.Mask.EditMask = "[a-zA-Z0-9@./:]+";
            this.txtReportServiceURL.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtReportServiceURL.Properties.Mask.ShowPlaceHolders = false;
            this.txtReportServiceURL.Size = new System.Drawing.Size(464, 20);
            this.txtReportServiceURL.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 14);
            this.label5.TabIndex = 8;
            this.label5.Text = "ReportService URL :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(392, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(185, 14);
            this.label4.TabIndex = 7;
            this.label4.Text = "* Local SMTP Host (E-Mail Relay)";
            // 
            // txtSMTPIP
            // 
            this.txtSMTPIP.Location = new System.Drawing.Point(138, 57);
            this.txtSMTPIP.Name = "txtSMTPIP";
            this.txtSMTPIP.Properties.Mask.EditMask = "[0-9.]+";
            this.txtSMTPIP.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtSMTPIP.Properties.Mask.ShowPlaceHolders = false;
            this.txtSMTPIP.Size = new System.Drawing.Size(248, 20);
            this.txtSMTPIP.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 14);
            this.label3.TabIndex = 5;
            this.label3.Text = "SMTP HOST IP :";
            // 
            // txtEmailID
            // 
            this.txtEmailID.Location = new System.Drawing.Point(138, 31);
            this.txtEmailID.Name = "txtEmailID";
            this.txtEmailID.Properties.Mask.EditMask = "[a-zA-Z0-9@_./:]+";
            this.txtEmailID.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtEmailID.Properties.Mask.ShowPlaceHolders = false;
            this.txtEmailID.Size = new System.Drawing.Size(248, 20);
            this.txtEmailID.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 34);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 14);
            this.label9.TabIndex = 3;
            this.label9.Text = "Email ID :";
            // 
            // btnUpdateNetwork
            // 
            this.btnUpdateNetwork.Location = new System.Drawing.Point(505, 227);
            this.btnUpdateNetwork.Name = "btnUpdateNetwork";
            this.btnUpdateNetwork.Size = new System.Drawing.Size(99, 34);
            this.btnUpdateNetwork.TabIndex = 7;
            this.btnUpdateNetwork.Text = "UpDate";
            this.btnUpdateNetwork.UseVisualStyleBackColor = true;
            this.btnUpdateNetwork.Click += new System.EventHandler(this.btnUpdateNetwork_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(392, 33);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(212, 14);
            this.label10.TabIndex = 2;
            this.label10.Text = "* Manual report distribution on behalf";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.btnTimeDel);
            this.groupBox2.Controls.Add(this.btnTimeAdd);
            this.groupBox2.Controls.Add(this.txtTime);
            this.groupBox2.Controls.Add(this.grd_avbl);
            this.groupBox2.Location = new System.Drawing.Point(636, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(324, 473);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Auto Time Setting in Machine Scheduler";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(88, 33);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 14);
            this.label11.TabIndex = 10;
            this.label11.Text = "* ( 24 Hrs )";
            // 
            // btnTimeDel
            // 
            this.btnTimeDel.Location = new System.Drawing.Point(243, 62);
            this.btnTimeDel.Name = "btnTimeDel";
            this.btnTimeDel.Size = new System.Drawing.Size(75, 23);
            this.btnTimeDel.TabIndex = 7;
            this.btnTimeDel.Text = "Delete";
            this.btnTimeDel.UseVisualStyleBackColor = true;
            this.btnTimeDel.Click += new System.EventHandler(this.btnTimeDel_Click);
            // 
            // btnTimeAdd
            // 
            this.btnTimeAdd.Location = new System.Drawing.Point(243, 33);
            this.btnTimeAdd.Name = "btnTimeAdd";
            this.btnTimeAdd.Size = new System.Drawing.Size(75, 23);
            this.btnTimeAdd.TabIndex = 6;
            this.btnTimeAdd.Text = "Add";
            this.btnTimeAdd.UseVisualStyleBackColor = true;
            this.btnTimeAdd.Click += new System.EventHandler(this.btnTimeAdd_Click);
            // 
            // txtTime
            // 
            this.txtTime.EditValue = new System.DateTime(2017, 12, 1, 0, 0, 0, 0);
            this.txtTime.Location = new System.Drawing.Point(6, 31);
            this.txtTime.Name = "txtTime";
            this.txtTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtTime.Properties.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4);
            this.txtTime.Properties.Mask.EditMask = "HH:mm";
            this.txtTime.Properties.MaxLength = 5;
            this.txtTime.Properties.NullValuePrompt = "Please Enter Time";
            this.txtTime.Properties.NullValuePromptShowForEmptyValue = true;
            this.txtTime.Properties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.Default;
            this.txtTime.Size = new System.Drawing.Size(76, 20);
            this.txtTime.TabIndex = 5;
            // 
            // grd_avbl
            // 
            this.grd_avbl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grd_avbl.Location = new System.Drawing.Point(3, 106);
            this.grd_avbl.MainView = this.gv_avbl;
            this.grd_avbl.Name = "grd_avbl";
            this.grd_avbl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.grd_avbl.Size = new System.Drawing.Size(318, 364);
            this.grd_avbl.TabIndex = 4;
            this.grd_avbl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv_avbl});
            // 
            // gv_avbl
            // 
            this.gv_avbl.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.SchTime});
            this.gv_avbl.GridControl = this.grd_avbl;
            this.gv_avbl.Name = "gv_avbl";
            this.gv_avbl.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gv_avbl.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gv_avbl.OptionsBehavior.Editable = false;
            this.gv_avbl.OptionsBehavior.ReadOnly = true;
            this.gv_avbl.OptionsCustomization.AllowColumnMoving = false;
            this.gv_avbl.OptionsCustomization.AllowFilter = false;
            this.gv_avbl.OptionsCustomization.AllowGroup = false;
            this.gv_avbl.OptionsCustomization.AllowQuickHideColumns = false;
            this.gv_avbl.OptionsCustomization.AllowRowSizing = true;
            this.gv_avbl.OptionsCustomization.AllowSort = false;
            this.gv_avbl.OptionsDetail.AllowZoomDetail = false;
            this.gv_avbl.OptionsDetail.EnableMasterViewMode = false;
            this.gv_avbl.OptionsDetail.ShowDetailTabs = false;
            this.gv_avbl.OptionsDetail.SmartDetailExpand = false;
            this.gv_avbl.OptionsMenu.EnableColumnMenu = false;
            this.gv_avbl.OptionsMenu.EnableFooterMenu = false;
            this.gv_avbl.OptionsMenu.EnableGroupPanelMenu = false;
            this.gv_avbl.OptionsMenu.ShowAddNewSummaryItem = DevExpress.Utils.DefaultBoolean.False;
            this.gv_avbl.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gv_avbl.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gv_avbl.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gv_avbl.OptionsMenu.ShowSplitItem = false;
            this.gv_avbl.OptionsNavigation.EnterMoveNextColumn = true;
            this.gv_avbl.OptionsSelection.MultiSelect = true;
            this.gv_avbl.OptionsView.ShowDetailButtons = false;
            this.gv_avbl.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gv_avbl.OptionsView.ShowGroupPanel = false;
            this.gv_avbl.DoubleClick += new System.EventHandler(this.gv_avbl_DoubleClick);
            // 
            // SchTime
            // 
            this.SchTime.Caption = "SchTime";
            this.SchTime.FieldName = "SchTime";
            this.SchTime.Name = "SchTime";
            this.SchTime.Visible = true;
            this.SchTime.VisibleIndex = 0;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Caption = "Check";
            this.repositoryItemCheckEdit1.DisplayValueChecked = "1";
            this.repositoryItemCheckEdit1.DisplayValueGrayed = "0";
            this.repositoryItemCheckEdit1.DisplayValueUnchecked = "0";
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            this.repositoryItemCheckEdit1.ValueGrayed = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(16, 50);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(98, 14);
            this.label15.TabIndex = 4;
            this.label15.Text = "LateCome Sec. :";
            // 
            // txtLateComeSec
            // 
            this.txtLateComeSec.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtLateComeSec.Location = new System.Drawing.Point(138, 48);
            this.txtLateComeSec.Name = "txtLateComeSec";
            this.txtLateComeSec.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtLateComeSec.Size = new System.Drawing.Size(96, 20);
            this.txtLateComeSec.TabIndex = 3;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(16, 76);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(99, 14);
            this.label16.TabIndex = 6;
            this.label16.Text = "EarlyCome Sec. :";
            // 
            // txtEarlyComeSec
            // 
            this.txtEarlyComeSec.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtEarlyComeSec.Location = new System.Drawing.Point(138, 74);
            this.txtEarlyComeSec.Name = "txtEarlyComeSec";
            this.txtEarlyComeSec.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtEarlyComeSec.Size = new System.Drawing.Size(96, 20);
            this.txtEarlyComeSec.TabIndex = 5;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(253, 76);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(99, 14);
            this.label17.TabIndex = 8;
            this.label17.Text = "EarlyGoing Sec. :";
            // 
            // txtEarlyGoingSec
            // 
            this.txtEarlyGoingSec.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtEarlyGoingSec.Location = new System.Drawing.Point(358, 74);
            this.txtEarlyGoingSec.Name = "txtEarlyGoingSec";
            this.txtEarlyGoingSec.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtEarlyGoingSec.Size = new System.Drawing.Size(93, 20);
            this.txtEarlyGoingSec.TabIndex = 7;
            // 
            // txtGracePeriodSec
            // 
            this.txtGracePeriodSec.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtGracePeriodSec.Location = new System.Drawing.Point(358, 156);
            this.txtGracePeriodSec.Name = "txtGracePeriodSec";
            this.txtGracePeriodSec.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtGracePeriodSec.Size = new System.Drawing.Size(93, 20);
            this.txtGracePeriodSec.TabIndex = 9;
            // 
            // txtLateHalfDaySec
            // 
            this.txtLateHalfDaySec.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtLateHalfDaySec.Location = new System.Drawing.Point(358, 107);
            this.txtLateHalfDaySec.Name = "txtLateHalfDaySec";
            this.txtLateHalfDaySec.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtLateHalfDaySec.Size = new System.Drawing.Size(93, 20);
            this.txtLateHalfDaySec.TabIndex = 12;
            // 
            // chkLateHalfDayFlg
            // 
            this.chkLateHalfDayFlg.AutoSize = true;
            this.chkLateHalfDayFlg.Location = new System.Drawing.Point(9, 108);
            this.chkLateHalfDayFlg.Name = "chkLateHalfDayFlg";
            this.chkLateHalfDayFlg.Size = new System.Drawing.Size(247, 18);
            this.chkLateHalfDayFlg.TabIndex = 13;
            this.chkLateHalfDayFlg.Text = "* Mark Half Day if Late More Than Sec. :";
            this.chkLateHalfDayFlg.UseVisualStyleBackColor = true;
            // 
            // chkEarlyGoingHalfDayFlg
            // 
            this.chkEarlyGoingHalfDayFlg.AutoSize = true;
            this.chkEarlyGoingHalfDayFlg.Location = new System.Drawing.Point(9, 132);
            this.chkEarlyGoingHalfDayFlg.Name = "chkEarlyGoingHalfDayFlg";
            this.chkEarlyGoingHalfDayFlg.Size = new System.Drawing.Size(283, 18);
            this.chkEarlyGoingHalfDayFlg.TabIndex = 15;
            this.chkEarlyGoingHalfDayFlg.Text = "* Mark Half Day if Early Going More Than Sec. :";
            this.chkEarlyGoingHalfDayFlg.UseVisualStyleBackColor = true;
            // 
            // txtEarlyGoingHalfDaySec
            // 
            this.txtEarlyGoingHalfDaySec.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtEarlyGoingHalfDaySec.Location = new System.Drawing.Point(358, 132);
            this.txtEarlyGoingHalfDaySec.Name = "txtEarlyGoingHalfDaySec";
            this.txtEarlyGoingHalfDaySec.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtEarlyGoingHalfDaySec.Size = new System.Drawing.Size(93, 20);
            this.txtEarlyGoingHalfDaySec.TabIndex = 14;
            // 
            // chkGraceHalfDayFlg
            // 
            this.chkGraceHalfDayFlg.AutoSize = true;
            this.chkGraceHalfDayFlg.Location = new System.Drawing.Point(9, 156);
            this.chkGraceHalfDayFlg.Name = "chkGraceHalfDayFlg";
            this.chkGraceHalfDayFlg.Size = new System.Drawing.Size(269, 18);
            this.chkGraceHalfDayFlg.TabIndex = 16;
            this.chkGraceHalfDayFlg.Text = "* Mark Half Day if Exceed Grace Period Sec :";
            this.chkGraceHalfDayFlg.UseVisualStyleBackColor = true;
            // 
            // frmOtherConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 504);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmOtherConfig";
            this.Text = "Other Configuration";
            this.Load += new System.EventHandler(this.frmOtherConfig_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSanDayLimit.Properties)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAutoProccessTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAutoProcessWrkGrp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerWorkerIP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReportSerExeURL.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReportServiceURL.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSMTPIP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmailID.Properties)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grd_avbl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_avbl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLateComeSec.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEarlyComeSec.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEarlyGoingSec.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGracePeriodSec.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLateHalfDaySec.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEarlyGoingHalfDaySec.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.SpinEdit txtSanDayLimit;
        private System.Windows.Forms.Button btnUpdateSan;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private DevExpress.XtraEditors.TextEdit txtEmailID;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnUpdateNetwork;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit txtSMTPIP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit txtReportSerExeURL;
        private DevExpress.XtraEditors.TextEdit txtReportServiceURL;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.TextEdit txtServerWorkerIP;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraGrid.GridControl grd_avbl;
        private DevExpress.XtraGrid.Views.Grid.GridView gv_avbl;
        private DevExpress.XtraGrid.Columns.GridColumn SchTime;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private System.Windows.Forms.Button btnTimeAdd;
        private DevExpress.XtraEditors.TimeEdit txtTime;
        private System.Windows.Forms.Button btnTimeDel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private DevExpress.XtraEditors.TimeEdit txtAutoProccessTime;
        private System.Windows.Forms.Label label12;
        private DevExpress.XtraEditors.TextEdit txtAutoProcessWrkGrp;
        private System.Windows.Forms.Label label14;
        private DevExpress.XtraEditors.SpinEdit txtGracePeriodSec;
        private System.Windows.Forms.Label label17;
        private DevExpress.XtraEditors.SpinEdit txtEarlyGoingSec;
        private System.Windows.Forms.Label label16;
        private DevExpress.XtraEditors.SpinEdit txtEarlyComeSec;
        private System.Windows.Forms.Label label15;
        private DevExpress.XtraEditors.SpinEdit txtLateComeSec;
        private DevExpress.XtraEditors.SpinEdit txtLateHalfDaySec;
        private System.Windows.Forms.CheckBox chkLateHalfDayFlg;
        private System.Windows.Forms.CheckBox chkEarlyGoingHalfDayFlg;
        private DevExpress.XtraEditors.SpinEdit txtEarlyGoingHalfDaySec;
        private System.Windows.Forms.CheckBox chkGraceHalfDayFlg;
    }
}