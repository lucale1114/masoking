using Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        private static GameManager _instance;

        private new void Awake()
        {
            base.Awake();
        }

        public static void LoadMenu()
        {
            LoadLevel(0);
        }

        public static void LoadLevel(int levelNr)
        {
            WaveData.ResetTime();
            SceneManager.LoadScene(levelNr);
        }

        public static void Restart()
        {
            LoadLevel(SceneManager.GetActiveScene().buildIndex);
        }

        public static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif

            Application.Quit();
        }
    }
}