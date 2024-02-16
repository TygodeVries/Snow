using System.Collections.Generic;
namespace Snow.Levels
{
    internal class LevelManager
    {
        private List<Level> levels = new List<Level>();

        public LevelManager()
        {   
        }

        public void LoadLevels()
        {

        }

        public Level GetLevel(int id)
        {
            return levels[id];
        }
    }
}
