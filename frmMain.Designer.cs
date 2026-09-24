namespace PNM_Revision_Tool
{
    partial class frmMain
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
            txbRevNumber = new TextBox();
            label1 = new Label();
            label2 = new Label();
            txbDate = new TextBox();
            label3 = new Label();
            txbDrafterInit = new TextBox();
            label4 = new Label();
            txbDesc1 = new TextBox();
            label5 = new Label();
            txbDesc2 = new TextBox();
            label6 = new Label();
            txbDesc3 = new TextBox();
            label7 = new Label();
            txbCHKinit = new TextBox();
            label8 = new Label();
            txbOKDinit = new TextBox();
            label9 = new Label();
            txbAPPinit = new TextBox();
            cmbApplyShtSet = new Button();
            lblStatus = new Label();
            cbxStamp = new ComboBox();
            label11 = new Label();
            prgStatus = new ProgressBar();
            txtLog = new TextBox();
            trvSheets = new TreeView();
            cmbOpenSS = new Button();
            cmbSelectAll = new Button();
            cmbSelectNone = new Button();
            SuspendLayout();
            // 
            // txbRevNumber
            // 
            txbRevNumber.Location = new Point(305, 41);
            txbRevNumber.Name = "txbRevNumber";
            txbRevNumber.Size = new Size(98, 23);
            txbRevNumber.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(305, 23);
            label1.Name = "label1";
            label1.Size = new Size(98, 15);
            label1.TabIndex = 1;
            label1.Text = "Revision Number";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(409, 23);
            label2.Name = "label2";
            label2.Size = new Size(31, 15);
            label2.TabIndex = 5;
            label2.Text = "Date";
            // 
            // txbDate
            // 
            txbDate.Location = new Point(409, 41);
            txbDate.Name = "txbDate";
            txbDate.Size = new Size(98, 23);
            txbDate.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(513, 23);
            label3.Name = "label3";
            label3.Size = new Size(80, 15);
            label3.TabIndex = 7;
            label3.Text = "Drafter Initials";
            // 
            // txbDrafterInit
            // 
            txbDrafterInit.Location = new Point(513, 41);
            txbDrafterInit.Name = "txbDrafterInit";
            txbDrafterInit.Size = new Size(98, 23);
            txbDrafterInit.TabIndex = 2;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(305, 67);
            label4.Name = "label4";
            label4.Size = new Size(101, 15);
            label4.TabIndex = 9;
            label4.Text = "Description Line 1";
            // 
            // txbDesc1
            // 
            txbDesc1.Location = new Point(305, 85);
            txbDesc1.Name = "txbDesc1";
            txbDesc1.Size = new Size(306, 23);
            txbDesc1.TabIndex = 3;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(305, 111);
            label5.Name = "label5";
            label5.Size = new Size(101, 15);
            label5.TabIndex = 11;
            label5.Text = "Description Line 2";
            // 
            // txbDesc2
            // 
            txbDesc2.Location = new Point(305, 129);
            txbDesc2.Name = "txbDesc2";
            txbDesc2.Size = new Size(306, 23);
            txbDesc2.TabIndex = 4;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(305, 155);
            label6.Name = "label6";
            label6.Size = new Size(101, 15);
            label6.TabIndex = 13;
            label6.Text = "Description Line 3";
            // 
            // txbDesc3
            // 
            txbDesc3.Location = new Point(305, 173);
            txbDesc3.Name = "txbDesc3";
            txbDesc3.Size = new Size(306, 23);
            txbDesc3.TabIndex = 5;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(305, 199);
            label7.Name = "label7";
            label7.Size = new Size(68, 15);
            label7.TabIndex = 15;
            label7.Text = "CHK Initials";
            // 
            // txbCHKinit
            // 
            txbCHKinit.Location = new Point(305, 217);
            txbCHKinit.Name = "txbCHKinit";
            txbCHKinit.Size = new Size(98, 23);
            txbCHKinit.TabIndex = 6;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(409, 199);
            label8.Name = "label8";
            label8.Size = new Size(68, 15);
            label8.TabIndex = 17;
            label8.Text = "OKD Initials";
            // 
            // txbOKDinit
            // 
            txbOKDinit.Location = new Point(409, 217);
            txbOKDinit.Name = "txbOKDinit";
            txbOKDinit.Size = new Size(98, 23);
            txbOKDinit.TabIndex = 7;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(513, 199);
            label9.Name = "label9";
            label9.Size = new Size(66, 15);
            label9.TabIndex = 19;
            label9.Text = "APP Initials";
            // 
            // txbAPPinit
            // 
            txbAPPinit.Location = new Point(513, 217);
            txbAPPinit.Name = "txbAPPinit";
            txbAPPinit.Size = new Size(98, 23);
            txbAPPinit.TabIndex = 8;
            // 
            // cmbApplyShtSet
            // 
            cmbApplyShtSet.Location = new Point(305, 292);
            cmbApplyShtSet.Name = "cmbApplyShtSet";
            cmbApplyShtSet.Size = new Size(306, 23);
            cmbApplyShtSet.TabIndex = 11;
            cmbApplyShtSet.Text = "Apply to Selected Sheets";
            cmbApplyShtSet.UseVisualStyleBackColor = true;
            cmbApplyShtSet.Click += cmbApplyShtSet_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(305, 318);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(227, 15);
            lblStatus.TabIndex = 22;
            lblStatus.Text = "Note: fields left blank will not be updated.";
            // 
            // cbxStamp
            // 
            cbxStamp.FormattingEnabled = true;
            cbxStamp.Location = new Point(305, 263);
            cbxStamp.Name = "cbxStamp";
            cbxStamp.Size = new Size(306, 23);
            cbxStamp.TabIndex = 9;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(305, 243);
            label11.Name = "label11";
            label11.Size = new Size(88, 15);
            label11.TabIndex = 24;
            label11.Text = "Drawing Stamp";
            // 
            // prgStatus
            // 
            prgStatus.Location = new Point(305, 336);
            prgStatus.Name = "prgStatus";
            prgStatus.Size = new Size(306, 23);
            prgStatus.Style = ProgressBarStyle.Continuous;
            prgStatus.TabIndex = 25;
            // 
            // txtLog
            // 
            txtLog.Location = new Point(305, 365);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(306, 185);
            txtLog.TabIndex = 26;
            // 
            // trvSheets
            // 
            trvSheets.CheckBoxes = true;
            trvSheets.Location = new Point(12, 41);
            trvSheets.Name = "trvSheets";
            trvSheets.Size = new Size(287, 509);
            trvSheets.TabIndex = 27;
            trvSheets.AfterSelect += trvSheets_AfterSelect;
            trvSheets.AfterCheck +=new TreeViewEventHandler(trvSheets_AfterCheck);
            // 
            // cmbOpenSS
            // 
            cmbOpenSS.Location = new Point(12, 12);
            cmbOpenSS.Name = "cmbOpenSS";
            cmbOpenSS.Size = new Size(110, 23);
            cmbOpenSS.TabIndex = 28;
            cmbOpenSS.Text = "Open Sheet Set";
            cmbOpenSS.UseVisualStyleBackColor = true;
            cmbOpenSS.Click += cmbOpenSS_Click;
            // 
            // cmbSelectAll
            // 
            cmbSelectAll.Location = new Point(128, 12);
            cmbSelectAll.Name = "cmbSelectAll";
            cmbSelectAll.Size = new Size(75, 23);
            cmbSelectAll.TabIndex = 29;
            cmbSelectAll.Text = "Select All";
            cmbSelectAll.UseVisualStyleBackColor = true;
            cmbSelectAll.Click += cmbSelectAll_Click;
            // 
            // cmbSelectNone
            // 
            cmbSelectNone.Location = new Point(209, 12);
            cmbSelectNone.Name = "cmbSelectNone";
            cmbSelectNone.Size = new Size(90, 23);
            cmbSelectNone.TabIndex = 30;
            cmbSelectNone.Text = "Select None";
            cmbSelectNone.UseVisualStyleBackColor = true;
            cmbSelectNone.Click += cmbSelectNone_Click;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(623, 562);
            Controls.Add(cmbSelectNone);
            Controls.Add(cmbSelectAll);
            Controls.Add(cmbOpenSS);
            Controls.Add(trvSheets);
            Controls.Add(txtLog);
            Controls.Add(prgStatus);
            Controls.Add(label11);
            Controls.Add(cbxStamp);
            Controls.Add(lblStatus);
            Controls.Add(cmbApplyShtSet);
            Controls.Add(label9);
            Controls.Add(txbAPPinit);
            Controls.Add(label8);
            Controls.Add(txbOKDinit);
            Controls.Add(label7);
            Controls.Add(txbCHKinit);
            Controls.Add(label6);
            Controls.Add(txbDesc3);
            Controls.Add(label5);
            Controls.Add(txbDesc2);
            Controls.Add(label4);
            Controls.Add(txbDesc1);
            Controls.Add(label3);
            Controls.Add(txbDrafterInit);
            Controls.Add(label2);
            Controls.Add(txbDate);
            Controls.Add(label1);
            Controls.Add(txbRevNumber);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmMain";
            Text = "PNM Revision Tool";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txbRevNumber;
        private Label label1;
        private Label label2;
        private TextBox txbDate;
        private Label label3;
        private TextBox txbDrafterInit;
        private Label label4;
        private TextBox txbDesc1;
        private Label label5;
        private TextBox txbDesc2;
        private Label label6;
        private TextBox txbDesc3;
        private Label label7;
        private TextBox txbCHKinit;
        private Label label8;
        private TextBox txbOKDinit;
        private Label label9;
        private TextBox txbAPPinit;
        private Button cmbApplyShtSet;
        private Label lblStatus;
        private ComboBox cbxStamp;
        private Label label11;
        private ProgressBar prgStatus;
        private TextBox txtLog;
        private TreeView trvSheets;
        private Button cmbOpenSS;
        private Button cmbSelectAll;
        private Button cmbSelectNone;
    }
}