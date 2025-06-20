using System.Windows.Forms;

namespace EducationalPractice
{
    public interface IGameObjectFactory
    {
        PictureBox CreateEnemy(List<PictureBox> existingEnemies, Panel gamePanel);
    }
}