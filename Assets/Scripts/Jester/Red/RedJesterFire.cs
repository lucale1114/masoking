using Player;
using Projectile;
using UnityEngine;

namespace Jester.Red
{
    public class RedJesterFire : MonoBehaviour
    {
        [SerializeField] private GameObject throwProjectilePrefab;
        [SerializeField] private GameObject throwAndRollProjectilePrefab;
        [SerializeField] private GameObject throwAndExplodeProjectilePrefab;
        [SerializeField] private AudioClip[] grunts;

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
            SoundFXManager.Instance.PlayRandomSoundFXNoPitch(grunts, 0.7f);
        }

        public void ThrowAndRoll(RedShotDataObject shotData)
        {
            var throwProjectile = Instantiate(throwAndRollProjectilePrefab, GetSpawnPoint(), Quaternion.identity);
            throwProjectile.GetComponent<BallProjectile>().SetShotData(shotData, _player.transform.position);
            throwProjectile.GetComponentInChildren<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("GameController").GetComponent<SpriteStorage>().GetThrowAndRollSprite(shotData.throwAndRollType);
            SoundFXManager.Instance.PlayRandomSoundFXNoPitch(grunts, 0.7f);

        }

        public void ThrowAndExplode(RedShotDataObject shotData)
        {
            var throwProjectile = Instantiate(throwAndExplodeProjectilePrefab, GetSpawnPoint(), Quaternion.identity);
            throwProjectile.GetComponent<BombProjectile>().SetShotData(shotData, _player.transform.position);
            SoundFXManager.Instance.PlayRandomSoundFXNoPitch(grunts, 0.7f);

        }
    }
}