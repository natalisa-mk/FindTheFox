using Level;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Context
{
    public class ProjectContext : MonoBehaviour
    {
        public static ServiceProvider Services { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        
            Services = new ServiceProvider();

            //Create core level data
            var coreLevelsLoader = new DummyLevelLoader();
            var coreLevelStarter = new CoreLevelStarter(coreLevelsLoader);
            
            Services.Register<ILevelStarter>(coreLevelStarter);
            Services.Register<CoreLevelStarter>(coreLevelStarter);

        
            SceneManager.LoadScene("Menu");
        }
    }
}
