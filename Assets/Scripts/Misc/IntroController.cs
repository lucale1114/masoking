using System.Collections;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace Misc
{
    public class IntroController : MonoBehaviour
    {
        private TextMeshProUGUI introText;
        AudioSource tSound;

        string[] texts =
        {
            "In a kingdom, far far away...",
            "There lives a King and a Queen.",
            "A Masoqueen and...",
            "A Masoking.",
            "A mashochistic man who used his power in order to...",
            "Get hot, get hit, get heavy!",
        };
        private void Start()
        {
            //_playableDirector = GetComponent<PlayableDirector>();
            //_playableDirector.stopped += _ => { GameManager.LoadLevel(3); };
            introText = GameObject.Find("Intro").GetComponent<TextMeshProUGUI>();
            tSound = GetComponent<AudioSource>();
            StartCoroutine(AnimationGo());
        }

        IEnumerator AnimationGo()
        {
            yield return new WaitForSecondsRealtime(2);
            StartCoroutine(FadeInImage(GameObject.Find("1").GetComponent<Image>()));
            yield return new WaitForSecondsRealtime(2);
            StartCoroutine(WriteOutText(texts[0]));
            yield return new WaitForSecondsRealtime(4);
            StartCoroutine(FadeInImage(GameObject.Find("2").GetComponent<Image>()));
            StartCoroutine(WriteOutText(texts[1]));
            yield return new WaitForSecondsRealtime(3);
            StartCoroutine(FadeInImage(GameObject.Find("3").GetComponent<Image>()));
            StartCoroutine(WriteOutText(texts[2]));
            yield return new WaitForSecondsRealtime(2);
            StartCoroutine(FadeInImage(GameObject.Find("4").GetComponent<Image>()));
            StartCoroutine(WriteOutText(texts[3]));
            yield return new WaitForSecondsRealtime(2);
            StartCoroutine(FadeInImage(GameObject.Find("5").GetComponent<Image>()));
            StartCoroutine(WriteOutText(texts[4]));
            yield return new WaitForSecondsRealtime(4);
            StartCoroutine(FadeInImage(GameObject.Find("6").GetComponent<Image>()));
            GameObject.Find("6").transform.DOPunchScale(GameObject.Find("6").transform.localScale, 0.5f, 20, 4);
            tSound.volume = 0;
            GameObject.Find("6").GetComponent<AudioSource>().Play();    
            StartCoroutine(WriteOutText(texts[5]));
            yield return new WaitForSecondsRealtime(4);
            for (int i = 0; i < 10; i++)
            {
                Color color = introText.color - new Color(0, 0, 0, 0.1f);
                introText.color = color;
                yield return new WaitForSecondsRealtime(0.1f);
            }
            yield return new WaitForSecondsRealtime(3);
            SceneManager.LoadScene(4);
        }

        IEnumerator FadeInImage(Image img)
        {
            for (int i = 0; i < 10; i++) {
                Color color = img.color + new Color(0,0,0,0.1f);
                img.color = color;
                yield return new WaitForSecondsRealtime(0.1f);
            }
            yield return new WaitForSecondsRealtime(3f);
            for (int i = 0; i < 10; i++)
            {
                Color color = img.color - new Color(0, 0, 0, 0.1f);
                img.color = color;
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        IEnumerator WriteOutText(string towrite)
        {
            introText.text = "";
            foreach (char c in towrite) {
                yield return new WaitForSecondsRealtime(0.05f);
                tSound.Play();
                introText.text += c;
            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                
            }
        }
    }
}