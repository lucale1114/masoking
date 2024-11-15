using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;

        private void Awake()
        {
            if (_instance == null && _instance != this)
            {
                _instance = this;
                DontDestroyOnLoad(_instance);
            }
            else
            {
                Destroy(gameObject);
            }
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