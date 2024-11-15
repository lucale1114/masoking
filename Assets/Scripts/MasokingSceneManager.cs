using UnityEngine;
using UnityEngine.SceneManagement;

public class MasokingSceneManager : MonoBehaviour
{
    private static MasokingSceneManager _instance;

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
        SceneManager.LoadScene(levelNr);
    }
}