using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        private void Awake()
        {
            GameObject.Find("PlayBtn").GetComponent<Button>().onClick.AddListener(() => GameManager.LoadLevel(1));
        }
    }
}