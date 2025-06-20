using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace EducationalPractice
{
    public partial class GameMain : Form
    {
        private PictureBox player;
        private List<PictureBox> enemies = new();
        private readonly Random random = new();

        private int score = 0;
        private int timeLeft = 60;
        private int trapsLeft = 10;
        private const int spawnInterval = 2000;
        private bool isGameOver = false;

        private Timer gameTimer;
        private Timer spawnTimer;

        private readonly IGameObjectFactory enemyFactory = new EnemyFactory();

        public GameMain() : this(Color.Blue) { }

        public GameMain(Color playerColor)
        {
            InitializeComponent();
            InitializeCustomUI(playerColor);
            KeyPreview = true;
            KeyDown += GameMain_KeyDown;
        }

        private void InitializeComponent()
        {
            this.Text = "Educational Practice Game";
            this.Size = new Size(800, 450);
        }

        private void InitializeCustomUI(Color playerColor)
        {
            // Инициализация игровой панели
            gamePanel = new Panel
            {
                Location = new Point(388, 12),
                Size = new Size(400, 400),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(gamePanel);

            // Метки
            lblScore = new Label { Text = "Очки: 0", Location = new Point(154, 227), AutoSize = true };
            lblTime = new Label { Text = "Время: 60", Location = new Point(154, 262), AutoSize = true };
            lblTraps = new Label { Text = $"Бомбы: {trapsLeft}", Location = new Point(154, 297), AutoSize = true };

            btnStart = new Button
            {
                Text = "Начать игру",
                Location = new Point(111, 188),
                Size = new Size(134, 23)
            };
            btnStart.Click += btnStart_Click;

            this.Controls.AddRange(new Control[] { lblScore, lblTime, lblTraps, btnStart });

            InitializeGame(playerColor);
        }

        private void InitializeGame(Color playerColor)
        {
            player = new PictureBox
            {
                Size = new Size(20, 20),
                BackColor = playerColor,
                Location = new Point(gamePanel.Width / 2 - 10, gamePanel.Height / 2 - 10)
            };
            gamePanel.Controls.Add(player);

            gameTimer = new Timer { Interval = 1000 };
            gameTimer.Tick += GameTimer_Tick;

            spawnTimer = new Timer { Interval = spawnInterval };
            spawnTimer.Tick += SpawnTimer_Tick;

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

            Timer moveTimer = new Timer { Interval = 200 };
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
                if (enemy.Bounds.IntersectsWith(player.Bounds))
                {
                    GameOver();
                    return;
                }

                foreach (Control control in gamePanel.Controls)
                {
                    if (control is PictureBox trap && trap.Tag as string == "trap")
                    {
                        if (enemy.Bounds.IntersectsWith(trap.Bounds))
                        {
                            gamePanel.Controls.Remove(trap);
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
            lblTraps.Text = $"Бомбы: {trapsLeft}";
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

        private void PlaceTrap()
        {
            PictureBox trap = new PictureBox
            {
                Size = new Size(20, 20),
                BackColor = Color.Black,
                Location = player.Location,
                Tag = "trap"
            };

            gamePanel.Controls.Add(trap);
            trap.BringToFront();

            trapsLeft--;
            UpdateTrapsUI();
        }

        private void GameMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (!gameTimer.Enabled) return;

            int speed = 5;
            int dx = 0, dy = 0;

            if (e.KeyCode == Keys.Space && trapsLeft > 0)
            {
                PlaceTrap();
                return;
            }

            if (e.KeyCode == Keys.W) dy -= speed;
            if (e.KeyCode == Keys.S) dy += speed;
            if (e.KeyCode == Keys.A) dx -= speed;
            if (e.KeyCode == Keys.D) dx += speed;

            Point newLocation = new(player.Left + dx, player.Top + dy);
            newLocation.X = Math.Clamp(newLocation.X, 0, gamePanel.Width - player.Width);
            newLocation.Y = Math.Clamp(newLocation.Y, 0, gamePanel.Height - player.Height);

            player.Location = newLocation;
            CheckCollisions();
        }

        private Panel gamePanel;
        private Label lblScore;
        private Label lblTime;
        private Label lblTraps;
        private Button btnStart;
    }
}