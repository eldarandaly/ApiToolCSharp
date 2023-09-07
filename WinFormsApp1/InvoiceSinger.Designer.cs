namespace WinFormsApp1
{
    partial class InvoiceSinger
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InvoiceSinger));
            label2 = new Label();
            TokenPinLabel = new Label();
            TokenPinBox = new TextBox();
            TokenCertificateBox = new TextBox();
            Connectbtn = new Button();
            label3 = new Label();
            label4 = new Label();
            disconnectButton = new Button();
            label1 = new Label();
            label5 = new Label();
            pictureBox1 = new PictureBox();
            button1 = new Button();
            groupBox1 = new GroupBox();
            dataGridView1 = new DataGridView();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            flowLayoutPanel2 = new FlowLayoutPanel();
            sendBtn = new DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("HP Simplified Hans", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(3, 137);
            label2.Name = "label2";
            label2.Size = new Size(97, 23);
            label2.TabIndex = 1;
            label2.Text = "Certificate";
            // 
            // TokenPinLabel
            // 
            TokenPinLabel.AutoSize = true;
            TokenPinLabel.Font = new Font("HP Simplified Hans", 12F, FontStyle.Regular, GraphicsUnit.Point);
            TokenPinLabel.Location = new Point(6, 77);
            TokenPinLabel.Name = "TokenPinLabel";
            TokenPinLabel.Size = new Size(94, 23);
            TokenPinLabel.TabIndex = 0;
            TokenPinLabel.Text = "Token Pin";
            // 
            // TokenPinBox
            // 
            TokenPinBox.Font = new Font("HP Simplified Hans", 10.1999989F, FontStyle.Regular, GraphicsUnit.Point);
            TokenPinBox.Location = new Point(116, 74);
            TokenPinBox.Name = "TokenPinBox";
            TokenPinBox.PlaceholderText = "9999999";
            TokenPinBox.Size = new Size(195, 27);
            TokenPinBox.TabIndex = 2;
            TokenPinBox.KeyPress += TokenPinBox_KeyPress;
            // 
            // TokenCertificateBox
            // 
            TokenCertificateBox.AutoCompleteCustomSource.AddRange(new string[] { "Egypt Trust Sealing CA" });
            TokenCertificateBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            TokenCertificateBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            TokenCertificateBox.Font = new Font("HP Simplified Hans", 10.1999989F, FontStyle.Regular, GraphicsUnit.Point);
            TokenCertificateBox.Location = new Point(116, 133);
            TokenCertificateBox.Name = "TokenCertificateBox";
            TokenCertificateBox.PlaceholderText = "Egypt Trust Sealing CA";
            TokenCertificateBox.Size = new Size(195, 27);
            TokenCertificateBox.TabIndex = 3;
            // 
            // Connectbtn
            // 
            Connectbtn.BackColor = SystemColors.Control;
            Connectbtn.Font = new Font("HP Simplified Hans", 10.1999989F, FontStyle.Regular, GraphicsUnit.Point);
            Connectbtn.Location = new Point(27, 214);
            Connectbtn.Name = "Connectbtn";
            Connectbtn.Size = new Size(284, 28);
            Connectbtn.TabIndex = 4;
            Connectbtn.Text = "Connect";
            Connectbtn.UseVisualStyleBackColor = false;
            Connectbtn.Click += Connectbtn_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("HP Simplified Hans", 7.79999971F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(287, 414);
            label3.Name = "label3";
            label3.Size = new Size(92, 14);
            label3.TabIndex = 5;
            label3.Text = "Copyright 2023";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("HP Simplified Hans", 10.1999989F, FontStyle.Regular, GraphicsUnit.Point);
            label4.ForeColor = Color.Firebrick;
            label4.Location = new Point(38, 175);
            label4.Name = "label4";
            label4.Size = new Size(0, 19);
            label4.TabIndex = 6;
            // 
            // disconnectButton
            // 
            disconnectButton.BackColor = SystemColors.Control;
            disconnectButton.BackgroundImageLayout = ImageLayout.None;
            disconnectButton.Font = new Font("HP Simplified Hans", 10.1999989F, FontStyle.Regular, GraphicsUnit.Point);
            disconnectButton.ForeColor = SystemColors.ControlText;
            disconnectButton.Location = new Point(82, 293);
            disconnectButton.Name = "disconnectButton";
            disconnectButton.Size = new Size(143, 28);
            disconnectButton.TabIndex = 7;
            disconnectButton.Text = "Disconnect";
            disconnectButton.UseVisualStyleBackColor = false;
            disconnectButton.Click += disconnectButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("HP Simplified Hans", 14F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(27, 12);
            label1.Name = "label1";
            label1.Size = new Size(315, 27);
            label1.TabIndex = 8;
            label1.Text = "Document Signature Creation";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("HP Simplified Hans", 6F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(6, 417);
            label5.Name = "label5";
            label5.Size = new Size(97, 11);
            label5.TabIndex = 9;
            label5.Text = "Eldarandaly@Genesis";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = InvoiceSignerApi.Properties.Resources.splash1;
            pictureBox1.Location = new Point(2, 1);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1220, 430);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 10;
            pictureBox1.TabStop = false;
            // 
            // button1
            // 
            button1.BackColor = SystemColors.Control;
            button1.Font = new Font("HP Simplified Hans", 7.79999971F, FontStyle.Regular, GraphicsUnit.Point);
            button1.Location = new Point(325, 320);
            button1.Name = "button1";
            button1.Size = new Size(66, 47);
            button1.TabIndex = 11;
            button1.Text = "Set";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(dataGridView1);
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(808, 365);
            groupBox1.TabIndex = 12;
            groupBox1.TabStop = false;
            groupBox1.Text = "Settings";
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { sendBtn });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 23);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new Size(802, 339);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridView1.CurrentCellDirtyStateChanged += dataGridView1_CurrentCellDirtyStateChanged;
            // 
            // button2
            // 
            button2.Location = new Point(3, 3);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 0;
            button2.Text = "Add";
            button2.UseVisualStyleBackColor = true;
            button2.Click += Addrow_Click;
            // 
            // button3
            // 
            button3.Location = new Point(103, 3);
            button3.Name = "button3";
            button3.Size = new Size(94, 29);
            button3.TabIndex = 13;
            button3.Text = "delete";
            button3.UseVisualStyleBackColor = true;
            button3.Click += deleteButton_Click;
            // 
            // button4
            // 
            button4.Location = new Point(203, 3);
            button4.Name = "button4";
            button4.Size = new Size(94, 29);
            button4.TabIndex = 14;
            button4.Text = "Save";
            button4.UseVisualStyleBackColor = true;
            button4.Click += InsertDataIntoDatabaseButton_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(button3);
            flowLayoutPanel1.Controls.Add(button4);
            flowLayoutPanel1.Location = new Point(3, 374);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(805, 42);
            flowLayoutPanel1.TabIndex = 16;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.BackColor = Color.White;
            flowLayoutPanel2.Controls.Add(groupBox1);
            flowLayoutPanel2.Controls.Add(flowLayoutPanel1);
            flowLayoutPanel2.Location = new Point(397, 12);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(814, 419);
            flowLayoutPanel2.TabIndex = 17;
            // 
            // sendBtn
            // 
            sendBtn.HeaderText = "Select";
            sendBtn.MinimumWidth = 6;
            sendBtn.Name = "sendBtn";
            sendBtn.ReadOnly = true;
            sendBtn.Resizable = DataGridViewTriState.True;
            sendBtn.Text = "Send";
            sendBtn.Width = 125;
            // 
            // InvoiceSinger
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1222, 432);
            Controls.Add(flowLayoutPanel2);
            Controls.Add(button1);
            Controls.Add(label5);
            Controls.Add(label1);
            Controls.Add(disconnectButton);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(Connectbtn);
            Controls.Add(TokenCertificateBox);
            Controls.Add(TokenPinBox);
            Controls.Add(label2);
            Controls.Add(TokenPinLabel);
            Controls.Add(pictureBox1);
            Font = new Font("HP Simplified Hans", 10.1999989F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "InvoiceSinger";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Tax Signer API";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private Label TokenPinLabel;
        private TextBox TokenPinBox;
        private TextBox TokenCertificateBox;
        private Button Connectbtn;
        private Label label3;
        private Label label4;
        private Button disconnectButton;
        private Label label1;
        private Label label5;
        private PictureBox pictureBox1;
        private Button button1;
        private GroupBox groupBox1;
        private DataGridView dataGridView1;
        private Button button2;
        private Button button3;
        private Button button4;
        private FlowLayoutPanel flowLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel2;
        private DataGridViewButtonColumn sendBtn;
    }
}