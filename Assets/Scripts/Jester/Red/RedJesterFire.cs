using Projectile;
using UnityEngine;
using Wave.Jesters.Red;

namespace Jester.Red
{
    public class RedJesterFire : MonoBehaviour
    {
        [SerializeField] private GameObject throwProjectilePrefab;
        [SerializeField] private GameObject throwAndRollProjectilePrefab;

        private GameObject _player;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        public void Throw(RedShotDataObject shotData)
        {
            var throwProjectile = Instantiate(throwProjectilePrefab, transform.position, Quaternion.identity);
            throwProjectile.GetComponent<Pin>().SetShotData(shotData, _player.transform.position);
        }

        public void ThrowAndRoll(RedShotDataObject shotData)
        {
            var throwProjectile = Instantiate(throwAndRollProjectilePrefab, transform.position, Quaternion.identity);
            throwProjectile.GetComponent<BallProjectile>().SetShotData(shotData, _player.transform.position);
        }
    }
}