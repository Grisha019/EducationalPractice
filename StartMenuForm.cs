using System.Drawing;
using System.Windows.Forms;

namespace EducationalPractice
{
    public partial class StartMenuForm : Form
    {
        private Color selectedColor = Color.Blue;

        public StartMenuForm()
        {
            InitializeComponent();
            InitializeCustomUI();
            // <-- Инициализация динамических элементов
        }

        private void InitializeCustomUI()
        {
            Label title = new Label
            {
                Text = "Выберите цвет персонажа",
                Font = new Font("Arial", 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(90, 20)
            };
            this.Controls.Add(title);

            Label teamName = new Label
            {
                Text = "Команда: Encubesulation",
                Font = new Font("Arial", 10),
                AutoSize = true,
                Location = new Point(120, 50)
            };
            this.Controls.Add(teamName);

            int x = 80;
            foreach (var color in new[] { Color.Blue, Color.Green, Color.Yellow, Color.Magenta, Color.Cyan })
            {
                Button btn = new Button
                {
                    BackColor = color,
                    Size = new Size(40, 40),
                    Location = new Point(x, 100),
                    FlatStyle = FlatStyle.Flat,
                    FlatAppearance = { BorderSize = 1 }
                };
                btn.Click += (s, e) =>
                {
                    selectedColor = btn.BackColor;
                    OpenGameForm();
                };
                this.Controls.Add(btn);
                x += 50;
            }

            Label instruction = new Label
            {
                Text = "WASD — ходьба\nSpace — установить бомбу",
                Font = new Font("Arial", 9),
                AutoSize = true,
                Location = new Point(100, 170)
            };
            this.Controls.Add(instruction);
        }

        private void OpenGameForm()
        {
            this.Hide();
            var gameForm = new GameMain(selectedColor);
            gameForm.FormClosed += (s, e) => this.Close();
            gameForm.Show();
        }
    }
}