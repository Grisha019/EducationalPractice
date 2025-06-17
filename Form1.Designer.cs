namespace EducationalPractice
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
            lblScore = new Label();
            gamePanel = new Panel();
            lblTime = new Label();
            btnStart = new Button();
            SuspendLayout();
            // 
            // lblScore
            // 
            lblScore.AutoSize = true;
            lblScore.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblScore.Location = new Point(154, 227);
            lblScore.Name = "lblScore";
            lblScore.Size = new Size(42, 13);
            lblScore.TabIndex = 0;
            lblScore.Text = "Счет: 0";
            // 
            // gamePanel
            // 
            gamePanel.BackColor = Color.White;
            gamePanel.BorderStyle = BorderStyle.FixedSingle;
            gamePanel.Location = new Point(388, 12);
            gamePanel.Name = "gamePanel";
            gamePanel.Size = new Size(400, 400);
            gamePanel.TabIndex = 1;
            gamePanel.Paint += panel1_Paint_1;
            // 
            // lblTime
            // 
            lblTime.AutoSize = true;
            lblTime.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblTime.Location = new Point(154, 262);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(58, 13);
            lblTime.TabIndex = 2;
            lblTime.Text = "Время: 60";
            // 
            // btnStart
            // 
            btnStart.Location = new Point(111, 188);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(134, 23);
            btnStart.TabIndex = 3;
            btnStart.Text = "Начать игру";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnStart);
            Controls.Add(lblTime);
            Controls.Add(gamePanel);
            Controls.Add(lblScore);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblScore;
        private Panel gamePanel;
        private Label lblTime;
        private Button btnStart;
    }
}
