using Level;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Context
{
    public class ProjectContext : MonoBehaviour
    {
        public static ServiceProvider Services { get; private set; }
        
        [SerializeField] private AudioManager audioManagerPrefab;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        
            Services = new ServiceProvider();
            
            var audioManager = Instantiate(audioManagerPrefab);
            DontDestroyOnLoad(audioManager.gameObject);

            Services.Register<IAudioManager>(audioManager);

            //Create core level data
            var coreLevelsLoader = new DummyLevelLoader();
            var coreLevelStarter = new CoreLevelStarter(coreLevelsLoader);
            
            Services.Register<ILevelStarter>(coreLevelStarter);
            Services.Register<CoreLevelStarter>(coreLevelStarter);

        
            SceneManager.LoadScene("Menu");
        }
    }
}
