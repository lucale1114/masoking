using UnityEngine;

namespace Misc
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T Instance;

        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<T>();
                DontDestroyOnLoad(Instance);
            }
            else
            {
                Destroy(gameObject);
            }
        }

    }
}