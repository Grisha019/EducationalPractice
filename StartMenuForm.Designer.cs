namespace EducationalPractice
{
    partial class StartMenuForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // 
            // StartMenuForm
            // 
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Text = "Encubesulation - Выбор персонажа";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.LightBlue;

            this.ResumeLayout(false);
        }
    }
}