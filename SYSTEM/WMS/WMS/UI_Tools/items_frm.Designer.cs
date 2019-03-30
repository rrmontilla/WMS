namespace WMS
{
    partial class items_frm
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
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.rdNo = new System.Windows.Forms.RadioButton();
            this.rdBYes = new System.Windows.Forms.RadioButton();
            this.label21 = new System.Windows.Forms.Label();
            this.txtSSLevel = new System.Windows.Forms.TextBox();
            this.txtLTDelivery = new System.Windows.Forms.TextBox();
            this.txtSupName = new System.Windows.Forms.TextBox();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.txtBrand = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtIName = new System.Windows.Forms.TextBox();
            this.txtICcodeTag = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.txtBoxUpload = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(881, 230);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 8;
            this.button1.Text = "&Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(8, 23);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(999, 233);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(16, 366);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(1015, 264);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Items";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtBoxUpload);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.rdNo);
            this.groupBox1.Controls.Add(this.rdBYes);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.txtSSLevel);
            this.groupBox1.Controls.Add(this.txtLTDelivery);
            this.groupBox1.Controls.Add(this.txtSupName);
            this.groupBox1.Controls.Add(this.txtUnit);
            this.groupBox1.Controls.Add(this.txtBrand);
            this.groupBox1.Controls.Add(this.txtDescription);
            this.groupBox1.Controls.Add(this.txtIName);
            this.groupBox1.Controls.Add(this.txtICcodeTag);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(16, 70);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(1015, 288);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Items Input Details";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(773, 230);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 28);
            this.button2.TabIndex = 62;
            this.button2.Text = "&New";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // rdNo
            // 
            this.rdNo.AutoSize = true;
            this.rdNo.Location = new System.Drawing.Point(245, 208);
            this.rdNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdNo.Name = "rdNo";
            this.rdNo.Size = new System.Drawing.Size(48, 23);
            this.rdNo.TabIndex = 61;
            this.rdNo.TabStop = true;
            this.rdNo.Text = "No";
            this.rdNo.UseVisualStyleBackColor = true;
            // 
            // rdBYes
            // 
            this.rdBYes.AutoSize = true;
            this.rdBYes.Location = new System.Drawing.Point(185, 208);
            this.rdBYes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdBYes.Name = "rdBYes";
            this.rdBYes.Size = new System.Drawing.Size(50, 23);
            this.rdBYes.TabIndex = 60;
            this.rdBYes.TabStop = true;
            this.rdBYes.Text = "Yes";
            this.rdBYes.UseVisualStyleBackColor = true;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(33, 208);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(92, 19);
            this.label21.TabIndex = 59;
            this.label21.Text = "Inventoriable:";
            // 
            // txtSSLevel
            // 
            this.txtSSLevel.Location = new System.Drawing.Point(621, 166);
            this.txtSSLevel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSSLevel.Name = "txtSSLevel";
            this.txtSSLevel.Size = new System.Drawing.Size(327, 26);
            this.txtSSLevel.TabIndex = 7;
            // 
            // txtLTDelivery
            // 
            this.txtLTDelivery.Location = new System.Drawing.Point(185, 166);
            this.txtLTDelivery.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtLTDelivery.Name = "txtLTDelivery";
            this.txtLTDelivery.Size = new System.Drawing.Size(287, 26);
            this.txtLTDelivery.TabIndex = 3;
            // 
            // txtSupName
            // 
            this.txtSupName.Location = new System.Drawing.Point(621, 132);
            this.txtSupName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSupName.Name = "txtSupName";
            this.txtSupName.Size = new System.Drawing.Size(327, 26);
            this.txtSupName.TabIndex = 6;
            // 
            // txtUnit
            // 
            this.txtUnit.Location = new System.Drawing.Point(185, 132);
            this.txtUnit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(287, 26);
            this.txtUnit.TabIndex = 2;
            // 
            // txtBrand
            // 
            this.txtBrand.Location = new System.Drawing.Point(621, 57);
            this.txtBrand.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(327, 26);
            this.txtBrand.TabIndex = 5;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(185, 60);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(287, 61);
            this.txtDescription.TabIndex = 1;
            // 
            // txtIName
            // 
            this.txtIName.Location = new System.Drawing.Point(621, 22);
            this.txtIName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtIName.Name = "txtIName";
            this.txtIName.Size = new System.Drawing.Size(327, 26);
            this.txtIName.TabIndex = 4;
            // 
            // txtICcodeTag
            // 
            this.txtICcodeTag.Location = new System.Drawing.Point(185, 22);
            this.txtICcodeTag.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtICcodeTag.Name = "txtICcodeTag";
            this.txtICcodeTag.Size = new System.Drawing.Size(287, 26);
            this.txtICcodeTag.TabIndex = 0;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(489, 169);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(121, 19);
            this.label20.TabIndex = 51;
            this.label20.Text = "Safety Stock Level:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(33, 169);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(140, 19);
            this.label19.TabIndex = 50;
            this.label19.Text = "Lead Time of Delivery";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(489, 134);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(101, 19);
            this.label18.TabIndex = 49;
            this.label18.Text = "Supplier Name:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(33, 134);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(111, 19);
            this.label17.TabIndex = 48;
            this.label17.Text = "Unit of Measure:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(489, 60);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(48, 19);
            this.label16.TabIndex = 47;
            this.label16.Text = "Brand:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(33, 60);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(81, 19);
            this.label15.TabIndex = 46;
            this.label15.Text = "Description:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(489, 26);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(80, 19);
            this.label14.TabIndex = 45;
            this.label14.Text = "Item Name:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(33, 26);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(100, 19);
            this.label13.TabIndex = 43;
            this.label13.Text = "Item Code Tag:";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Location = new System.Drawing.Point(-1, 9);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1049, 54);
            this.panel1.TabIndex = 8;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(37, 231);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 28);
            this.button3.TabIndex = 63;
            this.button3.Text = "&Open File";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Location = new System.Drawing.Point(621, 231);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 28);
            this.button4.TabIndex = 64;
            this.button4.Text = "&Upload";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // txtBoxUpload
            // 
            this.txtBoxUpload.Location = new System.Drawing.Point(185, 237);
            this.txtBoxUpload.Margin = new System.Windows.Forms.Padding(4);
            this.txtBoxUpload.Name = "txtBoxUpload";
            this.txtBoxUpload.ReadOnly = true;
            this.txtBoxUpload.Size = new System.Drawing.Size(287, 26);
            this.txtBoxUpload.TabIndex = 65;
            // 
            // items_frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1047, 638);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "items_frm";
            this.Text = "items_frm";
            this.Load += new System.EventHandler(this.items_frm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rdNo;
        private System.Windows.Forms.RadioButton rdBYes;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtSSLevel;
        private System.Windows.Forms.TextBox txtLTDelivery;
        private System.Windows.Forms.TextBox txtSupName;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.TextBox txtBrand;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtIName;
        private System.Windows.Forms.TextBox txtICcodeTag;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txtBoxUpload;
    }
}