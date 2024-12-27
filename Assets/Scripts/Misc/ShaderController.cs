using System.Collections;
using Player;
using UnityEngine;

namespace Misc
{
    public class ShaderController : MonoBehaviour
    {
        private static readonly int Twirl = Shader.PropertyToID("_Twirl");
        private static readonly int Speed = Shader.PropertyToID("_Speed");
        private static readonly int MoveX = Shader.PropertyToID("_MoveX");
        private static readonly int MoveY = Shader.PropertyToID("_MoveY");

        [SerializeField] private float twirlMultiplier = 5;
        [SerializeField] private float speedMultiplier = 5;
        [SerializeField] private float moveXMultiplier = 5;
        [SerializeField] private float moveYMultiplier = 5;
        [SerializeField] private float duration = 1f;

        private HeatSystem _heatSystem;
        private Material _material;

        private bool _inprogress;

        private void Start()
        {
            _heatSystem = FindObjectOfType<HeatSystem>();
            _material = GetComponent<MeshRenderer>().material;
/*
        _heatSystem.TakenDamage += () =>
        {
            if (!_inprogress)
            {
                StartCoroutine(ChangeRoutine());
            }
        };

        */
        }

        private IEnumerator ChangeRoutine()
        {
            _inprogress = true;

            var twirl = _material.GetFloat(Twirl);
            var speed = _material.GetFloat(Speed);
            var moveX = _material.GetFloat(MoveX);
            var moveY = _material.GetFloat(MoveY);

            _material.SetFloat(Twirl, twirl * twirlMultiplier);
            _material.SetFloat(Speed, speed * speedMultiplier);
            _material.SetFloat(MoveX, moveX * moveXMultiplier);
            _material.SetFloat(MoveY, moveY * moveYMultiplier);

            yield return new WaitForSeconds(duration);

            _material.SetFloat(Twirl, twirl);
            _material.SetFloat(Speed, speed);
            _material.SetFloat(MoveX, moveX);
            _material.SetFloat(MoveY, moveY);

            _inprogress = false;
        }
    }
}