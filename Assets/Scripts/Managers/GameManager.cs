using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {

        public static int CurrentLevel = 3;

        public static void LoadMenu()
        {
            SceneManager.LoadScene(0);
        }

        public static void LoadIntro()
        {
            SceneManager.LoadScene(1);
        }

        public static void LoadTutorial()
        {
            SceneManager.LoadScene(2);
        }

        public static void LoadLevel()
        {
            SceneManager.LoadScene(CurrentLevel);
        }

        public static void Restart()
        {
            LoadLevel();
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