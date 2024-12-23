using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Misc
{
    public class OpeningController : MonoBehaviour
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
            yield return new WaitForSecondsRealtime(4);
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
            yield return new WaitForSecondsRealtime(1.8f);
            SceneManager.LoadScene(4);
        }

        IEnumerator FadeInImage(Image img)
        {
            for (int i = 0; i < 10; i++) {
                Color color = img.color + new Color(0,0,0,0.1f);
                img.color = color;
                yield return new WaitForSecondsRealtime(0.1f);
            }
            yield return new WaitForSecondsRealtime(2.6f);
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

        private void DelayStart()
        {
            SceneManager.LoadScene(4);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                GameObject.Find("Frames").GetComponent<Animator>().enabled = true;
                tSound.GetComponent<AudioSource>().DOFade(0, 1);
                GameObject.Find("6").GetComponent<AudioSource>().DOFade(0, 1);
                Invoke("DelayStart", 3);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {

            }
        }
    }
}