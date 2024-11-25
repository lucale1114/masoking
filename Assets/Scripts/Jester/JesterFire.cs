using Projectile;
using Unity.VisualScripting;
using UnityEngine;
using static WaveData;
using Collision = Projectile.Collision;

namespace Jester
{
    public class JesterFire : MonoBehaviour
    {
        [SerializeField] private GameObject throwProjectilePrefab;
        [SerializeField] private GameObject throwAndRollProjectilePrefab;

        private GameObject player;
        public GameObject projectile;

        // A shoot function for the jesters that can be called whenever.
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // Fires a basic projectile towards the player based on inaccuracy and speed. Set to 0 when using for a perfectly aimed shot.
        public DirectProjectile ShootBasicProjectile(float speed, ShotDataObject data)
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
            Destroy(shot, 10);
            return projectileScript;
        }

        public DirectProjectile ShootBasicProjectile(float speed, ShotDataObject data, float forceX, float forceY)
        {
            Vector3 dir = (new Vector3(forceX, forceY) - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;

            GameObject shot = Instantiate(projectile, transform.position,
                Quaternion.Euler(new Vector3(0, 0, angle + Random.Range(-data.inaccuracy, data.inaccuracy))));
            shot.GetComponent<Rigidbody2D>().velocity = -shot.transform.up * speed;
            DirectProjectile projectileScript = shot.GetComponent<DirectProjectile>();
            projectileScript.SetShotData(data);
            projectileScript.player = player;
            Destroy(shot, 10);
            return projectileScript;
        }

        public void ShootBurstShot(float speed, float time, int burst, ShotDataObject data)
        {
            DirectProjectile shot = ShootBasicProjectile(speed, data);
            shot.burstTimer = time;
            shot.burst = burst;
        }

        public GameObject Snipe(ShotDataObject data, float x, float y, GameObject target)
        {
            DirectProjectile shot = ShootBasicProjectile(data.speed, data, x, y);
            shot.gameObject.GetComponent<Collision>().enabled = false;
            shot.sniper = true;
            return shot.gameObject;
        }

        public void ShootRow(float speed, float radius, int amount, ShotDataObject data)
        {
            Vector3 dir = (player.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90 +
                          Random.Range(-data.inaccuracy, data.inaccuracy);
            for (float i = angle - radius; i < angle + radius; i += (radius / amount) * 2)
            {
                GameObject shot = Instantiate(projectile, transform.position, Quaternion.Euler(new Vector3(0, 0, i)));
                shot.GetComponent<DirectProjectile>().SetShotData(data);
                shot.GetComponent<Rigidbody2D>().velocity = -shot.transform.up * speed;
            }
        }

        public void ShootWavyShot(float speed, float frequency, int amp, ShotDataObject data)
        {
            DirectProjectile shot = ShootBasicProjectile(speed, data);
            shot.spin = true;
            shot.frequency = frequency;
            shot.amp = amp;
        }

        public void ShootCurvedShot(float speed, float time, float dir, int wave, ShotDataObject data)
        {
            DirectProjectile shot = ShootBasicProjectile(speed, data);
            shot.spin = true;
            shot.gravityTimer = time;
            shot.gravityDir = dir;
            shot.flipAmount = wave;
        }

        public void Throw(ShotDataObject shotData)
        {
            var throwProjectile = Instantiate(throwProjectilePrefab, transform.position, Quaternion.identity);
            throwProjectile.GetComponent<CurvedProjectile>().SetShotData(shotData, player.transform.position);
        }

        public void ThrowAndRoll(ShotDataObject shotData)
        {
            var throwProjectile = Instantiate(throwAndRollProjectilePrefab, transform.position, Quaternion.identity);
            throwProjectile.GetComponent<BallProjectile>().SetShotData(shotData, player.transform.position);
        }
    }
}