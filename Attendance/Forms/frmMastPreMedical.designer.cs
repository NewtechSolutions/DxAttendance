namespace Attendance.Forms
{
    partial class frmMastPreMedical
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
            this.txtSrNo = new DevExpress.XtraEditors.TextEdit();
            this.label42 = new System.Windows.Forms.Label();
            this.txtAge = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRemark = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMedRes = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtMedChkDt = new DevExpress.XtraEditors.DateEdit();
            this.label10 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtContCode = new DevExpress.XtraEditors.TextEdit();
            this.label17 = new System.Windows.Forms.Label();
            this.txtBirthDT = new DevExpress.XtraEditors.DateEdit();
            this.label20 = new System.Windows.Forms.Label();
            this.txtAdharNo = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.txtEmpName = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txtWrkGrpDesc = new DevExpress.XtraEditors.TextEdit();
            this.txtWrkGrpCode = new DevExpress.XtraEditors.TextEdit();
            this.btnAdd = new System.Windows.Forms.Button();
            this.grpUserRights = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.GrpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSrNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAge.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMedRes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMedChkDt.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMedChkDt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtContCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBirthDT.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBirthDT.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAdharNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWrkGrpDesc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWrkGrpCode.Properties)).BeginInit();
            this.grpUserRights.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // GrpMain
            // 
            this.GrpMain.Controls.Add(this.label6);
            this.GrpMain.Controls.Add(this.txtSrNo);
            this.GrpMain.Controls.Add(this.label42);
            this.GrpMain.Controls.Add(this.txtAge);
            this.GrpMain.Controls.Add(this.label4);
            this.GrpMain.Controls.Add(this.txtRemark);
            this.GrpMain.Controls.Add(this.label3);
            this.GrpMain.Controls.Add(this.txtMedRes);
            this.GrpMain.Controls.Add(this.txtMedChkDt);
            this.GrpMain.Controls.Add(this.label10);
            this.GrpMain.Controls.Add(this.label2);
            this.GrpMain.Controls.Add(this.txtContCode);
            this.GrpMain.Controls.Add(this.label17);
            this.GrpMain.Controls.Add(this.txtBirthDT);
            this.GrpMain.Controls.Add(this.label20);
            this.GrpMain.Controls.Add(this.txtAdharNo);
            this.GrpMain.Controls.Add(this.label5);
            this.GrpMain.Controls.Add(this.txtEmpName);
            this.GrpMain.Controls.Add(this.label1);
            this.GrpMain.Controls.Add(this.txtWrkGrpDesc);
            this.GrpMain.Controls.Add(this.txtWrkGrpCode);
            this.GrpMain.Location = new System.Drawing.Point(12, 3);
            this.GrpMain.Name = "GrpMain";
            this.GrpMain.Size = new System.Drawing.Size(831, 192);
            this.GrpMain.TabIndex = 0;
            this.GrpMain.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 14);
            this.label6.TabIndex = 115;
            this.label6.Text = "Sr.No :";
            // 
            // txtSrNo
            // 
            this.txtSrNo.Location = new System.Drawing.Point(112, 24);
            this.txtSrNo.Name = "txtSrNo";
            this.txtSrNo.Properties.Mask.EditMask = "d";
            this.txtSrNo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtSrNo.Properties.MaxLength = 2;
            this.txtSrNo.Properties.ReadOnly = true;
            this.txtSrNo.Size = new System.Drawing.Size(89, 20);
            this.txtSrNo.TabIndex = 0;
            this.txtSrNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSrNo_KeyDown);
            this.txtSrNo.Validated += new System.EventHandler(this.txtSrNo_Validated);
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(269, 104);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(37, 14);
            this.label42.TabIndex = 113;
            this.label42.Text = "Age :";
            // 
            // txtAge
            // 
            this.txtAge.Location = new System.Drawing.Point(306, 102);
            this.txtAge.Name = "txtAge";
            this.txtAge.Properties.Mask.EditMask = "d";
            this.txtAge.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtAge.Properties.MaxLength = 3;
            this.txtAge.Properties.ReadOnly = true;
            this.txtAge.Size = new System.Drawing.Size(108, 20);
            this.txtAge.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 14);
            this.label4.TabIndex = 111;
            this.label4.Text = "Remark :";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(112, 155);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRemark.Properties.Appearance.Options.UseFont = true;
            this.txtRemark.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtRemark.Properties.AppearanceReadOnly.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtRemark.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            this.txtRemark.Properties.Mask.EditMask = "[A-Z .]+";
            this.txtRemark.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtRemark.Properties.MaxLength = 100;
            this.txtRemark.Size = new System.Drawing.Size(302, 20);
            this.txtRemark.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(243, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 109;
            this.label3.Text = "Med. Sts :";
            // 
            // txtMedRes
            // 
            this.txtMedRes.Location = new System.Drawing.Point(306, 129);
            this.txtMedRes.Name = "txtMedRes";
            this.txtMedRes.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtMedRes.Properties.Items.AddRange(new object[] {
            "-",
            "FIT",
            "UNFIT",
            "HOLD-ER"});
            this.txtMedRes.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.txtMedRes.Size = new System.Drawing.Size(108, 20);
            this.txtMedRes.TabIndex = 9;
            // 
            // txtMedChkDt
            // 
            this.txtMedChkDt.EditValue = null;
            this.txtMedChkDt.Location = new System.Drawing.Point(112, 128);
            this.txtMedChkDt.Name = "txtMedChkDt";
            this.txtMedChkDt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtMedChkDt.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtMedChkDt.Properties.CalendarTimeProperties.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4);
            this.txtMedChkDt.Properties.CalendarTimeProperties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.Default;
            this.txtMedChkDt.Size = new System.Drawing.Size(90, 20);
            this.txtMedChkDt.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(23, 131);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(71, 14);
            this.label10.TabIndex = 108;
            this.label10.Text = "Medical Dt :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(426, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 14);
            this.label2.TabIndex = 105;
            this.label2.Text = "Cont Code :";
            // 
            // txtContCode
            // 
            this.txtContCode.Location = new System.Drawing.Point(509, 76);
            this.txtContCode.Name = "txtContCode";
            this.txtContCode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtContCode.Properties.Appearance.Options.UseFont = true;
            this.txtContCode.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtContCode.Properties.AppearanceReadOnly.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtContCode.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            this.txtContCode.Properties.Mask.EditMask = "[A-Z .]+";
            this.txtContCode.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtContCode.Properties.MaxLength = 100;
            this.txtContCode.Size = new System.Drawing.Size(100, 20);
            this.txtContCode.TabIndex = 4;
            this.txtContCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtContCode_KeyDown);
            this.txtContCode.Validated += new System.EventHandler(this.txtContCode_Validated);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(23, 104);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(74, 14);
            this.label17.TabIndex = 103;
            this.label17.Text = "Birth Date  :";
            // 
            // txtBirthDT
            // 
            this.txtBirthDT.EditValue = null;
            this.txtBirthDT.Location = new System.Drawing.Point(112, 102);
            this.txtBirthDT.Name = "txtBirthDT";
            this.txtBirthDT.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBirthDT.Properties.Appearance.Options.UseFont = true;
            this.txtBirthDT.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtBirthDT.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtBirthDT.Properties.CalendarTimeProperties.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4);
            this.txtBirthDT.Properties.CalendarTimeProperties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.Default;
            this.txtBirthDT.Size = new System.Drawing.Size(90, 20);
            this.txtBirthDT.TabIndex = 5;
            this.txtBirthDT.Validated += new System.EventHandler(this.txtBirthDT_Validated);
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(426, 104);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(73, 18);
            this.label20.TabIndex = 92;
            this.label20.Text = "Aadhar No :";
            // 
            // txtAdharNo
            // 
            this.txtAdharNo.EditValue = "123456789012";
            this.txtAdharNo.Location = new System.Drawing.Point(509, 102);
            this.txtAdharNo.Name = "txtAdharNo";
            this.txtAdharNo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAdharNo.Properties.Appearance.Options.UseFont = true;
            this.txtAdharNo.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtAdharNo.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            this.txtAdharNo.Properties.Mask.EditMask = "[0-9]+";
            this.txtAdharNo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtAdharNo.Properties.MaxLength = 12;
            this.txtAdharNo.Size = new System.Drawing.Size(100, 20);
            this.txtAdharNo.TabIndex = 7;
            this.txtAdharNo.Validated += new System.EventHandler(this.txtAdharNo_Validated);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 14);
            this.label5.TabIndex = 62;
            this.label5.Text = "Person Name :";
            // 
            // txtEmpName
            // 
            this.txtEmpName.Location = new System.Drawing.Point(112, 76);
            this.txtEmpName.Name = "txtEmpName";
            this.txtEmpName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmpName.Properties.Appearance.Options.UseFont = true;
            this.txtEmpName.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtEmpName.Properties.AppearanceReadOnly.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtEmpName.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            this.txtEmpName.Properties.Mask.EditMask = "[A-Z .]+";
            this.txtEmpName.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtEmpName.Properties.MaxLength = 100;
            this.txtEmpName.Size = new System.Drawing.Size(302, 20);
            this.txtEmpName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 14);
            this.label1.TabIndex = 4;
            this.label1.Text = "WrkGrpCode :";
            // 
            // txtWrkGrpDesc
            // 
            this.txtWrkGrpDesc.Location = new System.Drawing.Point(206, 50);
            this.txtWrkGrpDesc.Name = "txtWrkGrpDesc";
            this.txtWrkGrpDesc.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWrkGrpDesc.Properties.Appearance.Options.UseFont = true;
            this.txtWrkGrpDesc.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtWrkGrpDesc.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            this.txtWrkGrpDesc.Properties.Mask.EditMask = "\\w{3,50}";
            this.txtWrkGrpDesc.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtWrkGrpDesc.Properties.Mask.ShowPlaceHolders = false;
            this.txtWrkGrpDesc.Properties.ReadOnly = true;
            this.txtWrkGrpDesc.Size = new System.Drawing.Size(208, 20);
            this.txtWrkGrpDesc.TabIndex = 2;
            this.txtWrkGrpDesc.TabStop = false;
            // 
            // txtWrkGrpCode
            // 
            this.txtWrkGrpCode.Location = new System.Drawing.Point(112, 50);
            this.txtWrkGrpCode.Name = "txtWrkGrpCode";
            this.txtWrkGrpCode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWrkGrpCode.Properties.Appearance.Options.UseFont = true;
            this.txtWrkGrpCode.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtWrkGrpCode.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            this.txtWrkGrpCode.Properties.Mask.EditMask = "\\w{3,10}";
            this.txtWrkGrpCode.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtWrkGrpCode.Properties.Mask.ShowPlaceHolders = false;
            this.txtWrkGrpCode.Properties.ReadOnly = true;
            this.txtWrkGrpCode.Size = new System.Drawing.Size(87, 20);
            this.txtWrkGrpCode.TabIndex = 1;
            this.txtWrkGrpCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtWrkGrpCode_KeyDown);
            this.txtWrkGrpCode.Validated += new System.EventHandler(this.txtWrkGrpCode_Validated);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.Cornsilk;
            this.btnAdd.Location = new System.Drawing.Point(231, 13);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 30);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // grpUserRights
            // 
            this.grpUserRights.Controls.Add(this.btnClose);
            this.grpUserRights.Controls.Add(this.btnCancel);
            this.grpUserRights.Controls.Add(this.btnDelete);
            this.grpUserRights.Controls.Add(this.btnUpdate);
            this.grpUserRights.Controls.Add(this.btnAdd);
            this.grpUserRights.Location = new System.Drawing.Point(12, 201);
            this.grpUserRights.Name = "grpUserRights";
            this.grpUserRights.Size = new System.Drawing.Size(831, 49);
            this.grpUserRights.TabIndex = 2;
            this.grpUserRights.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Cornsilk;
            this.btnClose.Location = new System.Drawing.Point(554, 13);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 30);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Clos&e";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Cornsilk;
            this.btnCancel.Location = new System.Drawing.Point(474, 13);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Cornsilk;
            this.btnDelete.Location = new System.Drawing.Point(393, 13);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 30);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.Cornsilk;
            this.btnUpdate.Location = new System.Drawing.Point(312, 13);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 30);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "&Update";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grid);
            this.groupBox1.Location = new System.Drawing.Point(12, 256);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(831, 264);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Aadhar Card History";
            // 
            // grid
            // 
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(3, 18);
            this.grid.MainView = this.gridView1;
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(825, 243);
            this.grid.TabIndex = 1;
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
            // 
            // frmMastPreMedical
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 532);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpUserRights);
            this.Controls.Add(this.GrpMain);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Name = "frmMastPreMedical";
            this.Text = "Pre-Medical Master";
            this.Load += new System.EventHandler(this.frmMastPreMedical_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMastPreMedical_KeyDown);
            this.GrpMain.ResumeLayout(false);
            this.GrpMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSrNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAge.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMedRes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMedChkDt.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMedChkDt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtContCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBirthDT.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBirthDT.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAdharNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWrkGrpDesc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWrkGrpCode.Properties)).EndInit();
            this.grpUserRights.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GrpMain;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtWrkGrpDesc;
        private DevExpress.XtraEditors.TextEdit txtWrkGrpCode;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.GroupBox grpUserRights;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.TextEdit txtEmpName;
        private System.Windows.Forms.Label label20;
        private DevExpress.XtraEditors.TextEdit txtAdharNo;
        private System.Windows.Forms.Label label17;
        private DevExpress.XtraEditors.DateEdit txtBirthDT;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit txtContCode;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit txtRemark;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.ComboBoxEdit txtMedRes;
        private DevExpress.XtraEditors.DateEdit txtMedChkDt;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label42;
        private DevExpress.XtraEditors.TextEdit txtAge;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit txtSrNo;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
    }
}