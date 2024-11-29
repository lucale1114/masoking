using System;
using UnityEngine;

namespace Jester
{
    public class JesterAnimator : MonoBehaviour
    {
        private static readonly int Moving = Animator.StringToHash("moving");
        private static readonly int Fire = Animator.StringToHash("fire");

        [Serializable]
        public class ColorToSprite
        {
            public WaveData.Color color;
            public Sprite sprite;
        }

        [SerializeField] private ColorToSprite[] colorToSprite;

        private Animator _animator;
        private Sprite _sprite;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            GetComponent<SpriteRenderer>().sprite = _sprite;
        }

        public void TriggerFire()
        {
            _animator.SetTrigger(Fire);
        }

        public void SetIdle()
        {
            _animator.SetBool(Moving, false);
        }

        public void SetMoving()
        {
            _animator.SetBool(Moving, true);
        }

        public void SetColor(WaveData.Color color)
        {
            foreach (var item in colorToSprite)
            {
                if (item.color == color)
                {
                    _sprite = item.sprite;
                }
            }
        }
    }
}