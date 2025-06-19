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
        private List<PictureBox> enemies = new();
        private List<PictureBox> objects = new();
        private readonly Random random = new();

        private int score = 0;
        private int timeLeft = 60;
        private int trapsLeft = 10;
        private const int spawnInterval = 2000;
        private bool isGameOver = false;

        private Timer gameTimer;
        private Timer spawnTimer;

        private readonly IGameObjectFactory enemyFactory = new EnemyFactory();

        public Form1()
        {
            InitializeComponent();
            KeyPreview = true;
            KeyDown += Form1_KeyDown;
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Инициализация игрока
            player = new PictureBox
            {
                Size = new Size(20, 20),
                BackColor = Color.Blue,
                Location = new Point(gamePanel.Width / 2 - 10, gamePanel.Height / 2 - 10)
            };
            gamePanel.Controls.Add(player);

            // Таймер игры
            gameTimer = new Timer { Interval = 1000 };
            gameTimer.Tick += GameTimer_Tick;

            // Таймер появления врагов
            spawnTimer = new Timer { Interval = spawnInterval };
            spawnTimer.Tick += SpawnTimer_Tick;

            // Обработка стрелок
            gamePanel.PreviewKeyDown += (s, e) => e.IsInputKey = true;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            lblTime.Text = $"Время: {timeLeft}";

            if (timeLeft <= 0)
                EndGame();
        }

        private void SpawnTimer_Tick(object sender, EventArgs e)
        {
            var newEnemy = enemyFactory.CreateEnemy(enemies, gamePanel);
            enemies.Add(newEnemy);
            gamePanel.Controls.Add(newEnemy);

            Timer moveTimer = new() { Interval = 200 };
            moveTimer.Tick += (_, _) =>
            {
                MoveEnemyRandomly(newEnemy);
                CheckCollisions();
            };
            moveTimer.Start();
        }

        private void MoveEnemyRandomly(PictureBox enemy)
        {
            int dx = random.Next(-1, 2) * 5;
            int dy = random.Next(-1, 2) * 5;
            Point newPos = new(enemy.Left + dx, enemy.Top + dy);

            if (newPos.X >= 0 && newPos.Y >= 0 &&
                newPos.X <= gamePanel.Width - enemy.Width &&
                newPos.Y <= gamePanel.Height - enemy.Height)
            {
                enemy.Location = newPos;
            }
        }

        private void CheckCollisions()
        {
            if (isGameOver) return;

            foreach (var enemy in enemies.ToArray())
            {
                // Столкновение с игроком
                if (enemy.Bounds.IntersectsWith(player.Bounds))
                {
                    GameOver();
                    return;
                }

                // Столкновение с ловушкой
                foreach (Control control in gamePanel.Controls)
                {
                    if (control is PictureBox trap && trap.Tag as string == "trap")
                    {
                        if (enemy.Bounds.IntersectsWith(trap.Bounds))
                        {
                            gamePanel.Controls.Remove(enemy);
                            enemies.Remove(enemy);
                            score++;
                            UpdateScore();
                            break;
                        }
                    }
                }
            }
        }

        private void GameOver()
        {
            if (isGameOver) return;

            isGameOver = true;
            gameTimer.Stop();
            spawnTimer.Stop();
            MessageBox.Show("Вы проиграли!");
            btnStart.Enabled = true;
        }

        private void EndGame()
        {
            gameTimer.Stop();
            spawnTimer.Stop();
            MessageBox.Show($"Игра окончена!\nВаш счет: {score}", "Результат");
            btnStart.Enabled = true;
        }

        private void UpdateScore()
        {
            lblScore.Text = $"Очки: {score}";
        }

        private void UpdateTrapsUI()
        {
            lblScore.Text = $"Ловушки: {trapsLeft}";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            isGameOver = false;

            gamePanel.Controls.Clear();
            enemies.Clear();
            trapsLeft = 10;
            score = 0;
            timeLeft = 60;

            UpdateScore();
            UpdateTrapsUI();

            player.Location = new Point(gamePanel.Width / 2 - 10, gamePanel.Height / 2 - 10);
            gamePanel.Controls.Add(player);

            gameTimer.Start();
            spawnTimer.Start();
            ActiveControl = gamePanel;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!gameTimer.Enabled) return;

            int speed = 5;
            Point newLocation = player.Location;

            if (e.KeyCode == Keys.Space && trapsLeft > 0)
            {
                PictureBox trap = new()
                {
                    Size = new Size(20, 20),
                    BackColor = Color.Black,
                    Location = player.Location,
                    Tag = "trap"
                };
                gamePanel.Controls.Add(trap);
                trap.BringToFront();
                player.BringToFront();

                trapsLeft--;
                UpdateTrapsUI();
                return;
            }

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
                    return;
            }

            newLocation.X = Math.Clamp(newLocation.X, 0, gamePanel.Width - player.Width);
            newLocation.Y = Math.Clamp(newLocation.Y, 0, gamePanel.Height - player.Height);

            player.Location = newLocation;
            CheckCollisions();
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e) { }
        private void Form1_Load(object sender, EventArgs e) { }
    }
   
}
