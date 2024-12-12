using Projectile;
using UnityEngine;

namespace Jester.Red
{
    public class RedJesterFire : MonoBehaviour
    {
        [SerializeField] private GameObject throwProjectilePrefab;
        [SerializeField] private GameObject throwAndRollProjectilePrefab;
        [SerializeField] private GameObject throwAndExplodeProjectilePrefab;

        private GameObject _player;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        private Vector3 GetSpawnPoint()
        {
            return transform.GetChild(0).transform.position;
        }

        public void Throw(RedShotDataObject shotData)
        {
            var throwProjectile = Instantiate(throwProjectilePrefab, GetSpawnPoint(), Quaternion.identity);
            throwProjectile.GetComponent<Pin>().SetShotData(shotData, _player.transform.position);
        }

        public void ThrowAndRoll(RedShotDataObject shotData)
        {
            var throwProjectile = Instantiate(throwAndRollProjectilePrefab, GetSpawnPoint(), Quaternion.identity);
            throwProjectile.GetComponent<BallProjectile>().SetShotData(shotData, _player.transform.position);
            throwProjectile.GetComponent<SpriteRenderer>().sprite = SpriteStorage.GetThrowAndRollSprite(shotData.throwAndRollType);
        }

        public void ThrowAndExplode(RedShotDataObject shotData)
        {
            var throwProjectile = Instantiate(throwAndExplodeProjectilePrefab, GetSpawnPoint(), Quaternion.identity);
            throwProjectile.GetComponent<BombProjectile>().SetShotData(shotData, _player.transform.position);
        }
    }
}