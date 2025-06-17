using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace EducationalPractice
{
    public partial class Form1 : Form
    {
        private PictureBox player;
        private List<PictureBox> objects = new List<PictureBox>();
        private Random random = new Random();

        private int score = 0;
        private int timeLeft = 60;
        private int spawnInterval = 2000;

        private Timer gameTimer;
        private Timer spawnTimer;

        public Form1()
        {
            InitializeComponent();

            // Ключевое исправление: подписываемся на события клавиатуры
            this.KeyPreview = true; // Форма будет получать события клавиш первой
            this.KeyDown += Form1_KeyDown;

            InitializeGame();
        }

        private void InitializeGame()
        {
            player = new PictureBox
            {
                Size = new Size(20, 20),
                BackColor = Color.Blue,
                Location = new Point(gamePanel.Width / 2 - 10, gamePanel.Height / 2 - 10)
            };
            gamePanel.Controls.Add(player);

            gameTimer = new Timer { Interval = 1000 };
            gameTimer.Tick += GameTimer_Tick;

            spawnTimer = new Timer { Interval = spawnInterval };
            spawnTimer.Tick += SpawnTimer_Tick;

            // Разрешаем обработку стрелок на игровой панели
            gamePanel.PreviewKeyDown += (s, e) => {
                e.IsInputKey = true; // Помечаем все клавиши как обрабатываемые
            };
        }

        // Остальной код без изменений...
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            lblTime.Text = $"Время: {timeLeft}";

            if (timeLeft <= 0)
            {
                EndGame();
            }
        }

        private void SpawnTimer_Tick(object sender, EventArgs e)
        {
            SpawnObject();

            if (spawnInterval > 500)
            {
                spawnInterval -= 100;
                spawnTimer.Interval = spawnInterval;
            }
        }

        private void SpawnObject()
        {
            var obj = new PictureBox
            {
                Size = new Size(15, 15),
                BackColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256)),
                Location = new Point(
                    random.Next(gamePanel.Width - 15),
                    random.Next(gamePanel.Height - 15))
            };

            gamePanel.Controls.Add(obj);
            objects.Add(obj);
            obj.BringToFront();
            player.BringToFront();
        }

        private void CheckCollisions()
        {
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (player.Bounds.IntersectsWith(objects[i].Bounds))
                {
                    gamePanel.Controls.Remove(objects[i]);
                    objects.RemoveAt(i);
                    score++;
                    lblScore.Text = $"Счет: {score}";
                }
            }
        }

        private void EndGame()
        {
            gameTimer.Stop();
            spawnTimer.Stop();
            MessageBox.Show($"Игра окончена!\nВаш счет: {score}", "Результат");
            btnStart.Enabled = true;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            score = 0;
            timeLeft = 60;
            spawnInterval = 2000;

            foreach (var obj in objects)
            {
                gamePanel.Controls.Remove(obj);
            }
            objects.Clear();

            lblScore.Text = "Счет: 0";
            lblTime.Text = "Время: 60";
            btnStart.Enabled = false;

            player.Location = new Point(gamePanel.Width / 2 - 10, gamePanel.Height / 2 - 10);

            gameTimer.Start();
            spawnTimer.Interval = spawnInterval;
            spawnTimer.Start();

            SpawnObject();

            // Устанавливаем фокус на форму для приема клавиш
            this.Focus();
        }

        // Исправленный обработчик клавиш
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!gameTimer.Enabled) return;

            int speed = 5;
            Point newLocation = player.Location;

            switch (e.KeyCode)
            {
                case Keys.Left:
                    newLocation.X -= speed;
                    break;
                case Keys.Right:
                    newLocation.X += speed;
                    break;
                case Keys.Up:
                    newLocation.Y -= speed;
                    break;
                case Keys.Down:
                    newLocation.Y += speed;
                    break;
                default:
                    return; // Игнорируем другие клавиши
            }

            // Проверка границ
            newLocation.X = Math.Clamp(newLocation.X, 0, gamePanel.Width - player.Width);
            newLocation.Y = Math.Clamp(newLocation.Y, 0, gamePanel.Height - player.Height);

            player.Location = newLocation;
            CheckCollisions();
        }

        // Пустые методы можно удалить, но если нужны - оставляем
        private void panel1_Paint_1(object sender, PaintEventArgs e) { }
        private void Form1_Load(object sender, EventArgs e) { }
    }
}