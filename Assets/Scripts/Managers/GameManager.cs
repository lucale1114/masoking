using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static void LoadMenu()
        {
            SceneManager.LoadScene(0);
        }

        public static void LoadOpening()
        {
            SceneManager.LoadScene(1);
        }

        public static void LoadLoadingScreen()
        {
            SceneManager.LoadScene(2);
        }

        public static void LoadTutorial()
        {
            SceneManager.LoadScene(3);
        }

        public static void LoadFirstLevel()
        {
            SceneManager.LoadScene(4);
        }

        public static void LoadNextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public static void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public static void Quit()
        {
            #if UNITY_EDITOR
                EditorApplication.isPlaying = false;
            #endif

            Application.Quit();
        }
    }
}