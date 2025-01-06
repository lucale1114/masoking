using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static int LastLevelCompleted;
        private static bool isTutorialMode = false; // To track if we need to show the tutorial first.

        public static void LoadMenu()
        {
            LastLevelCompleted = 0;
            isTutorialMode = false; // Reset tutorial mode flag.
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
           // SceneManager.LoadScene(3);
            isTutorialMode = true; // Set flag to show tutorial.
            SceneManager.LoadScene(3); // Load the Tutorial Scene (scene 3)
        }

        public static void LoadNextLevel()
        {
            if (isTutorialMode)
            {
                // After tutorial, we want to load the next level (level 4)
                LastLevelCompleted = 3; // Assuming the tutorial is level 3.
                SceneManager.LoadScene(4); // Load the next level after the tutorial.
                return;
            }

            if (LastLevelCompleted == 0 )
            {
                    
                LastLevelCompleted = 3;
            }

            SceneManager.LoadScene(LastLevelCompleted + 1);

            
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