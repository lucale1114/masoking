using System.Collections;
using Managers;
using UnityEditor.Rendering;
using UnityEngine;

namespace Menu
{
    public class MoveLoad : MonoBehaviour
    {
        private Rigidbody2D _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            StartCoroutine(LoadNextScene());
        }

        private void Update()
        {
            _rb.velocity = Vector2.right * 3f;
        }

        private static IEnumerator LoadNextScene()
        {
            yield return new WaitForSeconds(6f);

            GameManager.LoadFirstLevel();
        }
    }
}