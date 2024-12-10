using Player;
using Projectile;
using UnityEngine;
using Wave.Jesters.Blue;
using Wave.Jesters.Red;

namespace Jester
{
    public class JesterFire : MonoBehaviour
    {
        [SerializeField] private GameObject throwProjectilePrefab;
        [SerializeField] private GameObject throwAndRollProjectilePrefab;
        public AudioClip[] frow;

        private GameObject player;
        public GameObject projectile;

        // A shoot function for the jesters that can be called whenever.
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // Fires a basic projectile towards the player based on inaccuracy and speed. Set to 0 when using for a perfectly aimed shot.
        public DirectProjectile ShootBasicProjectile(float speed, BlueShotDataObject data)
        {
            float angle = -90;
            if (data.straight)
            {
                if (player.transform.position.x > transform.position.x)
                {
                    angle = 90;
                }
            }
            else
            {
                float x = player.transform.position.x;
                float y = player.transform.position.y;
                if (data.x != 0 || data.y != 0)
                {
                    x = data.x;
                    y = data.y;
                }

                if (data.randomY)
                {
                    y = Random.Range(-40, 40) / 10;
                }

                if (data.randomX)
                {
                    x = Random.Range(-50, 50) / 10;
                }

                Vector3 dir = (new Vector3(x, y) - transform.position).normalized;
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
            }

            GameObject shot = Instantiate(projectile, transform.position,
                Quaternion.Euler(new Vector3(0, 0, angle + Random.Range(-data.inaccuracy, data.inaccuracy))));
            shot.GetComponent<Rigidbody2D>().velocity = -shot.transform.up * speed;
            DirectProjectile projectileScript = shot.GetComponent<DirectProjectile>();
            projectileScript.SetShotData(data);
            projectileScript.player = player;
            SoundFXManager.Instance.PlayRandomSoundFX(frow, 1f);
            Destroy(shot, 10);
            return projectileScript;
        }

        public DirectProjectile ShootBasicProjectile(float speed, BlueShotDataObject data, float forceX, float forceY)
        {
            Vector3 dir = (new Vector3(forceX, forceY) - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;

            GameObject shot = Instantiate(projectile, transform.position,
                Quaternion.Euler(new Vector3(0, 0, angle + Random.Range(-data.inaccuracy, data.inaccuracy))));
            shot.GetComponent<Rigidbody2D>().velocity = -shot.transform.up * speed;
            DirectProjectile projectileScript = shot.GetComponent<DirectProjectile>();
            projectileScript.SetShotData(data);
            projectileScript.player = player;
            SoundFXManager.Instance.PlayRandomSoundFX(frow, 1f);
            Destroy(shot, 10);
            return projectileScript;
        }

        public void ShootBurstShot(float speed, float time, int burst, BlueShotDataObject data)
        {
            DirectProjectile shot = ShootBasicProjectile(speed, data);
            SoundFXManager.Instance.PlayRandomSoundFX(frow, 1f);
            shot.burstTimer = time;
            shot.burst = burst;
        }

        public GameObject Snipe(BlueShotDataObject data, float x, float y, GameObject target)
        {
            DirectProjectile shot = ShootBasicProjectile(data.speed, data, x, y);
            SoundFXManager.Instance.PlayRandomSoundFX(frow, 1f);
            shot._canHit = false;
            return shot.gameObject;
        }

        public void ShootRow(float speed, float radius, int amount, BlueShotDataObject data)
        {
            float angle = -90;
            if (data.straight)
            {
                if (player.transform.position.x > transform.position.x)
                {
                    angle = 90;
                }
            }
            else
            {
                float x = player.transform.position.x;
                float y = player.transform.position.y;
                if (data.x != 0 || data.y != 0)
                {
                    x = data.x;
                    y = data.y;
                }

                if (data.randomY)
                {
                    y = Random.Range(-40, 40) / 10;
                }

                if (data.randomX)
                {
                    x = Random.Range(-50, 50) / 10;
                }

                Vector3 dir = (new Vector3(x, y) - transform.position).normalized;
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90 + Random.Range(-data.inaccuracy, data.inaccuracy);
            }
            for (float i = angle - radius; i < angle + radius; i += (radius / amount) * 2)
            {
                GameObject shot = Instantiate(projectile, transform.position, Quaternion.Euler(new Vector3(0, 0, i)));
                shot.GetComponent<DirectProjectile>().SetShotData(data);
                shot.GetComponent<Rigidbody2D>().velocity = -shot.transform.up * speed;
                SoundFXManager.Instance.PlayRandomSoundFX(frow, 1f);
            }
        }

        public void ShootWavyShot(float speed, float frequency, int amp, BlueShotDataObject data)
        {
            DirectProjectile shot = ShootBasicProjectile(speed, data);
            shot.spin = true;
            shot.axe = true;
            shot.frequency = frequency;
            shot.amp = amp;
            SoundFXManager.Instance.PlayRandomSoundFX(frow, 1f);
        }

        public void ShootCurvedShot(float speed, float time, float dir, int wave, BlueShotDataObject data)
        {
            DirectProjectile shot = ShootBasicProjectile(speed, data);
            shot.spin = true;
            shot.gravityTimer = time;
            shot.gravityDir = dir;
            shot.axe = true;
            shot.flipAmount = wave;
            SoundFXManager.Instance.PlayRandomSoundFX(frow, 1f);
        }

        public void Throw(RedShotDataObject shotData)
        {
            var throwProjectile = Instantiate(throwProjectilePrefab, transform.position, Quaternion.identity);
            throwProjectile.GetComponent<Pin>().SetShotData(shotData, player.transform.position);
        }

        public void ThrowAndRoll(RedShotDataObject shotData)
        {
            var throwProjectile = Instantiate(throwAndRollProjectilePrefab, transform.position, Quaternion.identity);
            throwProjectile.GetComponent<BallProjectile>().SetShotData(shotData, player.transform.position);
        }
    }
}