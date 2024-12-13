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

        public static void LoadIntro()
        {
            SceneManager.LoadScene(1);
        }

        public static void LoadTutorial()
        {
            SceneManager.LoadScene(2);
        }

        public static void LoadLevel(int buildIndex)
        {
            SceneManager.LoadScene(buildIndex);
        }

        public static void Restart()
        {
            LoadLevel(SceneManager.GetActiveScene().buildIndex);
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