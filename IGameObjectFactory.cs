using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalPractice
{
    public interface IGameObjectFactory
    {
        PictureBox CreateEnemy(List<PictureBox> existingEnemies, Panel gamePanel);
    }
}
