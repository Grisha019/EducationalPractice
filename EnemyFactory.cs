using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    Size = new Size(15, 15),
                    BackColor = Color.Red,
                    Location = new Point(rand.Next(gamePanel.Width - 15), rand.Next(gamePanel.Height - 15))
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
    }
}

