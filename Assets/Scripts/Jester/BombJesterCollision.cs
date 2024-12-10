using Player;
using UnityEngine;

namespace Jester
{
    public class BombJesterCollision : MonoBehaviour
    {
        private Movement _movement;
        private Transform _bomb;

        public bool HasDashed { get; private set; }

        private void Start()
        {
            _movement = GameObject.Find("Player").GetComponent<Movement>();
            _bomb = gameObject.transform.GetChild(0);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && _movement.IsCurrentlyDashing)
            {
                HasDashed = true;
                _bomb.parent = null;
            }
        }
    }
}