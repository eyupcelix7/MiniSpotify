namespace MiniSpotify
{
    partial class Form1
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
            pictureBox1 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = Properties.Resources._0x1900_000000_80_0_0;
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox1.ImageLocation = "";
            pictureBox1.Location = new Point(8, 9);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(55, 71);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Figtree ExtraBold", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label1.ForeColor = Color.WhiteSmoke;
            label1.Location = new Point(65, 12);
            label1.Name = "label1";
            label1.Size = new Size(143, 20);
            label1.TabIndex = 1;
            label1.Text = "the world is yours";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Figtree SemiBold", 9.749999F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label2.ForeColor = Color.WhiteSmoke;
            label2.Location = new Point(65, 36);
            label2.Name = "label2";
            label2.Size = new Size(60, 16);
            label2.TabIndex = 2;
            label2.Text = "Organize";
            // 
            // button1
            // 
            button1.BackgroundImage = Properties.Resources.start;
            button1.BackgroundImageLayout = ImageLayout.Center;
            button1.FlatAppearance.BorderColor = Color.DimGray;
            button1.FlatStyle = FlatStyle.Flat;
            button1.ForeColor = Color.WhiteSmoke;
            button1.Location = new Point(288, 33);
            button1.Name = "button1";
            button1.Size = new Size(45, 45);
            button1.TabIndex = 3;
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.BackgroundImage = Properties.Resources.prev;
            button2.BackgroundImageLayout = ImageLayout.Center;
            button2.FlatAppearance.BorderColor = Color.DimGray;
            button2.FlatStyle = FlatStyle.Flat;
            button2.ForeColor = Color.WhiteSmoke;
            button2.Location = new Point(238, 33);
            button2.Name = "button2";
            button2.Size = new Size(45, 45);
            button2.TabIndex = 4;
            button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.BackgroundImage = Properties.Resources.next;
            button3.BackgroundImageLayout = ImageLayout.Center;
            button3.FlatAppearance.BorderColor = Color.DimGray;
            button3.FlatStyle = FlatStyle.Flat;
            button3.ForeColor = Color.WhiteSmoke;
            button3.Location = new Point(338, 33);
            button3.Name = "button3";
            button3.Size = new Size(45, 45);
            button3.TabIndex = 5;
            button3.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Desktop;
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(390, 85);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Form1";
            StartPosition = FormStartPosition.Manual;
            Text = "Form1";
            TopMost = true;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;
        private Button button1;
        private Button button2;
        private Button button3;
    }
}
