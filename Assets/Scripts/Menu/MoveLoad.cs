using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveLoad : MonoBehaviour
{

    [SerializeField]  float speed;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(LoadNextScene());

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector2.right * speed;
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(5f);
        GameManager.CurrentLevel = 3;
        GameManager.LoadLevel();
    }
}
