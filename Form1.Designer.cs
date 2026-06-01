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
            pctBoxImage = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            togglePlayBtn = new Button();
            prevBtn = new Button();
            nextBtn = new Button();
            likeBtn = new Button();
            ((System.ComponentModel.ISupportInitialize)pctBoxImage).BeginInit();
            SuspendLayout();
            // 
            // pctBoxImage
            // 
            pctBoxImage.BackgroundImage = Properties.Resources._0x1900_000000_80_0_0;
            pctBoxImage.BackgroundImageLayout = ImageLayout.Zoom;
            pctBoxImage.Cursor = Cursors.Hand;
            pctBoxImage.ImageLocation = "";
            pctBoxImage.Location = new Point(8, 9);
            pctBoxImage.Name = "pctBoxImage";
            pctBoxImage.Size = new Size(55, 71);
            pctBoxImage.TabIndex = 0;
            pctBoxImage.TabStop = false;
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
            // togglePlayBtn
            // 
            togglePlayBtn.BackgroundImage = Properties.Resources.stop;
            togglePlayBtn.BackgroundImageLayout = ImageLayout.Center;
            togglePlayBtn.Cursor = Cursors.Hand;
            togglePlayBtn.FlatAppearance.BorderColor = Color.DimGray;
            togglePlayBtn.FlatStyle = FlatStyle.Flat;
            togglePlayBtn.ForeColor = Color.WhiteSmoke;
            togglePlayBtn.Location = new Point(322, 50);
            togglePlayBtn.Name = "togglePlayBtn";
            togglePlayBtn.Size = new Size(30, 30);
            togglePlayBtn.TabIndex = 3;
            togglePlayBtn.UseVisualStyleBackColor = true;
            togglePlayBtn.Click += togglePlayBtn_Click;
            // 
            // prevBtn
            // 
            prevBtn.BackgroundImage = Properties.Resources.prev;
            prevBtn.BackgroundImageLayout = ImageLayout.Center;
            prevBtn.Cursor = Cursors.Hand;
            prevBtn.FlatAppearance.BorderColor = Color.DimGray;
            prevBtn.FlatStyle = FlatStyle.Flat;
            prevBtn.ForeColor = Color.WhiteSmoke;
            prevBtn.Location = new Point(288, 50);
            prevBtn.Name = "prevBtn";
            prevBtn.Size = new Size(30, 30);
            prevBtn.TabIndex = 4;
            prevBtn.UseVisualStyleBackColor = true;
            prevBtn.Click += prevBtn_Click;
            // 
            // nextBtn
            // 
            nextBtn.BackgroundImage = Properties.Resources.next;
            nextBtn.BackgroundImageLayout = ImageLayout.Center;
            nextBtn.Cursor = Cursors.Hand;
            nextBtn.FlatAppearance.BorderColor = Color.DimGray;
            nextBtn.FlatStyle = FlatStyle.Flat;
            nextBtn.ForeColor = Color.WhiteSmoke;
            nextBtn.Location = new Point(356, 50);
            nextBtn.Name = "nextBtn";
            nextBtn.Size = new Size(30, 30);
            nextBtn.TabIndex = 5;
            nextBtn.UseVisualStyleBackColor = true;
            nextBtn.Click += nextBtn_Click;
            // 
            // likeBtn
            // 
            likeBtn.BackgroundImage = Properties.Resources.musicLoveFix;
            likeBtn.BackgroundImageLayout = ImageLayout.Center;
            likeBtn.Cursor = Cursors.Hand;
            likeBtn.FlatAppearance.BorderColor = Color.DimGray;
            likeBtn.FlatStyle = FlatStyle.Flat;
            likeBtn.ForeColor = Color.WhiteSmoke;
            likeBtn.Location = new Point(254, 50);
            likeBtn.Name = "likeBtn";
            likeBtn.Size = new Size(30, 30);
            likeBtn.TabIndex = 6;
            likeBtn.UseVisualStyleBackColor = true;
            likeBtn.Click += likeBtn_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Desktop;
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(390, 85);
            Controls.Add(likeBtn);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pctBoxImage);
            Controls.Add(nextBtn);
            Controls.Add(prevBtn);
            Controls.Add(togglePlayBtn);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Form1";
            StartPosition = FormStartPosition.Manual;
            Text = "Form1";
            TopMost = true;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pctBoxImage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pctBoxImage;
        private Label label1;
        private Label label2;
        private Button togglePlayBtn;
        private Button prevBtn;
        private Button nextBtn;
        private Button likeBtn;
    }
}
