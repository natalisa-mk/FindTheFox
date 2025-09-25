using UnityEngine.SceneManagement;

namespace Level
{
    public class CoreLevelStarter : ILevelStarter
    {
        private readonly ILevelLoader _levelLoader;
    
        public CoreLevelStarter(ILevelLoader levelLoader)
        {
            _levelLoader = levelLoader;
        }
    
        public void StartLevel(int levelId)
        {
            if(_levelLoader.TryLoadLevel(levelId, out var levelData))
            {
                CrossScenesData.CurrentLevelData = levelData;
            
                SceneManager.LoadScene("Game");
            }
            else
            {
                //todo Make some popup with warning;
            }
        }
    }
}

