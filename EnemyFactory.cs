using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EducationalPractice
{
    public class EnemyFactory : IGameObjectFactory
    {
        private Random rand = new Random();

        public PictureBox CreateEnemy(List<PictureBox> existingEnemies, Panel gamePanel)
        {
            PictureBox enemy;
            bool overlap;

            do
            {
                overlap = false;
                enemy = new PictureBox
                {
                    Size = new Size(40, 40),
                    BackColor = Color.Red,
                    Location = GetRandomLocation(gamePanel)
                };

                foreach (var e in existingEnemies)
                {
                    if (e.Bounds.IntersectsWith(enemy.Bounds))
                    {
                        overlap = true;
                        break;
                    }
                }

            } while (overlap);

            return enemy;
        }

        private Point GetRandomLocation(Panel panel)
        {
            int x = rand.Next(0, panel.Width - 40);
            int y = rand.Next(0, panel.Height - 40);
            return new Point(x, y);
        }
    }
}