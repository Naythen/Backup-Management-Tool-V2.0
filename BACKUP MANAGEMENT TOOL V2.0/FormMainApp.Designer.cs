namespace BackupManagementTool
{
    partial class FormMainApp
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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMainApp));
            sqlCommand1 = new Microsoft.Data.SqlClient.SqlCommand();
            checkedListBox_columns = new CheckedListBox();
            groupBox1 = new GroupBox();
            advanceddataGridViewBackups = new Zuby.ADGV.AdvancedDataGridView();
            groupBox2 = new GroupBox();
            button_export = new Button();
            SignOutButton = new Button();
            labelRedRow = new Label();
            labelGreenRow = new Label();
            contorGreen = new Label();
            contorRed = new Label();
            contorPC = new Label();
            label2 = new Label();
            toolTip = new ToolTip(components);
            refreshButton = new Button();
            labelUserName = new Label();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)advanceddataGridViewBackups).BeginInit();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // sqlCommand1
            // 
            sqlCommand1.CommandTimeout = 30;
            sqlCommand1.EnableOptimizedParameterBinding = false;
            // 
            // checkedListBox_columns
            // 
            checkedListBox_columns.BackColor = SystemColors.Info;
            checkedListBox_columns.FormattingEnabled = true;
            checkedListBox_columns.Location = new Point(6, 15);
            checkedListBox_columns.Name = "checkedListBox_columns";
            checkedListBox_columns.Size = new Size(191, 814);
            checkedListBox_columns.TabIndex = 7;
            checkedListBox_columns.ItemCheck += CheckedListBox_columns_ItemCheck;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(advanceddataGridViewBackups);
            groupBox1.Location = new Point(12, 53);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1667, 837);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "Data";
            // 
            // advanceddataGridViewBackups
            // 
            advanceddataGridViewBackups.AllowUserToOrderColumns = true;
            advanceddataGridViewBackups.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            advanceddataGridViewBackups.BackgroundColor = SystemColors.Info;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            advanceddataGridViewBackups.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            advanceddataGridViewBackups.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            advanceddataGridViewBackups.DefaultCellStyle = dataGridViewCellStyle2;
            advanceddataGridViewBackups.FilterAndSortEnabled = true;
            advanceddataGridViewBackups.FilterStringChangedInvokeBeforeDatasourceUpdate = true;
            advanceddataGridViewBackups.Location = new Point(6, 13);
            advanceddataGridViewBackups.MaxFilterButtonImageHeight = 23;
            advanceddataGridViewBackups.MultiSelect = false;
            advanceddataGridViewBackups.Name = "advanceddataGridViewBackups";
            advanceddataGridViewBackups.RightToLeft = RightToLeft.No;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            advanceddataGridViewBackups.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            advanceddataGridViewBackups.Size = new Size(1655, 816);
            advanceddataGridViewBackups.SortStringChangedInvokeBeforeDatasourceUpdate = true;
            advanceddataGridViewBackups.TabIndex = 12;
            advanceddataGridViewBackups.VirtualMode = true;
            advanceddataGridViewBackups.CellDoubleClick += AdvanceddataGridViewBackups_CellDoubleClick;
            advanceddataGridViewBackups.CellEndEdit += AdvanceddataGridViewBackups_CellEndEdit;
            advanceddataGridViewBackups.DataBindingComplete += AdvanceddataGridViewBackups_DataBindingComplete;
            advanceddataGridViewBackups.RowsAdded += AdvanceddataGridViewBackups_RowsAdded;
            advanceddataGridViewBackups.RowsRemoved += AdvanceddataGridViewBackups_RowsRemoved;
            advanceddataGridViewBackups.UserAddedRow += AdvanceddataGridViewBackups_UserAddedRow;
            advanceddataGridViewBackups.UserDeletedRow += AdvanceddataGridViewBackups_UserDeletedRow;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(checkedListBox_columns);
            groupBox2.Location = new Point(1685, 53);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(203, 837);
            groupBox2.TabIndex = 10;
            groupBox2.TabStop = false;
            groupBox2.Text = "Add columns";
            // 
            // button_export
            // 
            button_export.BackColor = SystemColors.Info;
            button_export.Location = new Point(1685, 896);
            button_export.Name = "button_export";
            button_export.Size = new Size(203, 41);
            button_export.TabIndex = 11;
            button_export.Text = "Verify Data";
            toolTip.SetToolTip(button_export, "Buton de verificare backup-uri dispozitive in QNAP");
            button_export.UseVisualStyleBackColor = false;
            button_export.Click += Button_Verify_Click;
            // 
            // SignOutButton
            // 
            SignOutButton.BackColor = SystemColors.Info;
            SignOutButton.Location = new Point(18, 896);
            SignOutButton.Name = "SignOutButton";
            SignOutButton.Size = new Size(203, 41);
            SignOutButton.TabIndex = 13;
            SignOutButton.Text = "Sign out";
            toolTip.SetToolTip(SignOutButton, "Buton de iesire din aplicatie si intoarcere la meniul de log in");
            SignOutButton.UseVisualStyleBackColor = false;
            SignOutButton.Click += SignOutButton_Click;
            // 
            // labelRedRow
            // 
            labelRedRow.AutoSize = true;
            labelRedRow.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            labelRedRow.ForeColor = Color.LimeGreen;
            labelRedRow.Location = new Point(1143, 906);
            labelRedRow.Name = "labelRedRow";
            labelRedRow.Size = new Size(420, 25);
            labelRedRow.TabIndex = 14;
            labelRedRow.Text = "Dispozitive ce au backup conform cu cerintele";
            labelRedRow.TextAlign = ContentAlignment.MiddleRight;
            // 
            // labelGreenRow
            // 
            labelGreenRow.AutoSize = true;
            labelGreenRow.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            labelGreenRow.ForeColor = Color.PaleVioletRed;
            labelGreenRow.Location = new Point(1109, 931);
            labelGreenRow.Name = "labelGreenRow";
            labelGreenRow.Size = new Size(454, 25);
            labelGreenRow.TabIndex = 15;
            labelGreenRow.Text = "Dispozitive ce NU au backup conform cu cerintele";
            labelGreenRow.TextAlign = ContentAlignment.MiddleRight;
            // 
            // contorGreen
            // 
            contorGreen.AutoSize = true;
            contorGreen.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            contorGreen.ForeColor = Color.LimeGreen;
            contorGreen.Location = new Point(1569, 903);
            contorGreen.Name = "contorGreen";
            contorGreen.Size = new Size(23, 25);
            contorGreen.TabIndex = 16;
            contorGreen.Text = "0";
            contorGreen.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // contorRed
            // 
            contorRed.AutoSize = true;
            contorRed.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            contorRed.ForeColor = Color.PaleVioletRed;
            contorRed.Location = new Point(1569, 931);
            contorRed.Name = "contorRed";
            contorRed.Size = new Size(23, 25);
            contorRed.TabIndex = 17;
            contorRed.Text = "0";
            contorRed.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // contorPC
            // 
            contorPC.AutoSize = true;
            contorPC.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            contorPC.ForeColor = Color.Blue;
            contorPC.Location = new Point(1569, 956);
            contorPC.Name = "contorPC";
            contorPC.Size = new Size(23, 25);
            contorPC.TabIndex = 18;
            contorPC.Text = "0";
            contorPC.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            label2.ForeColor = Color.Blue;
            label2.Location = new Point(1406, 956);
            label2.Name = "label2";
            label2.Size = new Size(157, 25);
            label2.TabIndex = 19;
            label2.Text = "Total Dispozitive";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // refreshButton
            // 
            refreshButton.BackColor = SystemColors.Info;
            refreshButton.Location = new Point(1685, 946);
            refreshButton.Name = "refreshButton";
            refreshButton.Size = new Size(203, 41);
            refreshButton.TabIndex = 21;
            refreshButton.Text = "Refresh";
            toolTip.SetToolTip(refreshButton, "Buton refresh informatii din baza de date");
            refreshButton.UseVisualStyleBackColor = false;
            refreshButton.Click += RefreshButton_Click;
            // 
            // labelUserName
            // 
            labelUserName.AutoSize = true;
            labelUserName.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            labelUserName.ForeColor = Color.Blue;
            labelUserName.Location = new Point(1691, 22);
            labelUserName.Name = "labelUserName";
            labelUserName.Size = new Size(50, 25);
            labelUserName.TabIndex = 20;
            labelUserName.Text = "user";
            labelUserName.TextAlign = ContentAlignment.MiddleRight;
            // 
            // FormMainApp
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Ivory;
            ClientSize = new Size(1900, 1037);
            Controls.Add(refreshButton);
            Controls.Add(labelUserName);
            Controls.Add(label2);
            Controls.Add(contorPC);
            Controls.Add(contorRed);
            Controls.Add(contorGreen);
            Controls.Add(labelGreenRow);
            Controls.Add(labelRedRow);
            Controls.Add(SignOutButton);
            Controls.Add(button_export);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormMainApp";
            Text = "Backup Management Tool";
            WindowState = FormWindowState.Maximized;
            FormClosing += FormMainApp_FormClosing;
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)advanceddataGridViewBackups).EndInit();
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Microsoft.Data.SqlClient.SqlCommand sqlCommand1;
        private CheckedListBox checkedListBox_columns;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button button_export;
        private Zuby.ADGV.AdvancedDataGridView advanceddataGridViewBackups;
        private Button SignOutButton;
        private Label labelRedRow;
        private Label labelGreenRow;
        private Label contorGreen;
        private Label contorRed;
        private Label contorPC;
        private Label label2;
        private ToolTip toolTip;
        private Label labelUserName;
        private Button refreshButton;
    }
}