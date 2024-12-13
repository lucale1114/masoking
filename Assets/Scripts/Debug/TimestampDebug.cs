using System.Globalization;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wave.Handler;

namespace Debug
{
    public class TimestampDebug : MonoBehaviour
    {
        [SerializeField] private HeatSystem heatSystem;

        private Button _pauseButton;
        private Button _invincibilityBtn;
        private TMP_InputField _timestampText;

        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;

            _pauseButton = GameObject.Find("PauseBtn").GetComponent<Button>();
            _pauseButton.onClick.AddListener(Pause);

            _invincibilityBtn = GameObject.Find("InvincibilityBtn").GetComponent<Button>();
            _invincibilityBtn.onClick.AddListener(SetInvincible);

            _timestampText = GameObject.Find("TimestampText").GetComponent<TMP_InputField>();

            if (gameObject.activeInHierarchy)
            {
                heatSystem.invincible = PlayerPrefs.GetInt("Invi", 0) == 0;
                SetInvincible();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (WaveHandler.Paused)
            {
                if (_timestampText.text != "") {
                    WaveHandler.Timestamp = float.Parse(_timestampText.text);
                }
            }
            else
            {
                _timestampText.text = WaveHandler.Timestamp.ToString(CultureInfo.InvariantCulture);
            }

            if (Input.GetMouseButtonDown(0))
            {
                UnityEngine.Debug.Log(_camera!.ScreenToWorldPoint(Input.mousePosition));    
            }
        }

        private void SetInvincible()
        {
            if (heatSystem.invincible)
            {
                heatSystem.invincible = false;
                _invincibilityBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Invi: OFF";
            }
            else
            {
                heatSystem.invincible = true;
                _invincibilityBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Invi: ON";
            }

            PlayerPrefs.SetInt("Invi", heatSystem.invincible ? 1 : 0);
        }

        private void Pause()
        {
            WaveHandler.Paused = !WaveHandler.Paused;
            _pauseButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                WaveHandler.Paused ? "Paused" : "Unpaused";
        }
    }
}