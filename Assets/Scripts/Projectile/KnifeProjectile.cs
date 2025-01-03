using System.Collections;
using Jester.Blue;
using UnityEngine;

namespace Projectile
{
    public class DirectProjectile : MonoBehaviour, IProjectile
    {
        public GameObject projectileRef;
        public GameObject player;

        private float spinSpeed;
        private Rigidbody2D rb;

        public int burst;
        public float burstTimer;
        public float gravityTimer;
        public float gravityDir;
        public bool axe;
        public int flipAmount = 1;
        public bool spin;
        public float damageMod = 1;

        public float frequency;
        public float amp;

        private BlueShotDataObject _data;

        public bool _canHit = true;
        private int _numberOfBounces;

        // Code for projectile behavior.
        void Start()
        {
            transform.localScale *= _data.scale;
            rb = GetComponent<Rigidbody2D>();
            if (spin || _data.spin)
            {
                spinSpeed = Random.Range(6.0f, 7.0f) * (Random.Range(0, 2) * 2 - 1);
                InvokeRepeating("Spin", 0, 0.005f);
            }
            if (burstTimer > 0)
            {
                StartCoroutine(Burst());
                GetComponent<Collision>().noStabbing = true;
            }

            if (gravityTimer > 0)
            {
                StartCoroutine(GravityShift());
            }

            if (frequency > 0)
            {
                StartCoroutine(WavyShot());
            }

            if (_data.breakable)
            {
                GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("GameController").GetComponent<SpriteStorage>().GetBreakableSprite();
                spinSpeed *= 0.5f;
                transform.localScale *= 3;
                GetComponent<Collision>().noStabbing = true;
            } else if (axe)
            {
                GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("GameController").GetComponent<SpriteStorage>().GetAxeSprite();
                transform.localScale *= 3;
                spinSpeed *= 0.5f;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("GameController").GetComponent<SpriteStorage>().GetKnifeSprite();
            }
        }

        IEnumerator WavyShot()
        {
            float baseY = transform.position.y;
            while (gameObject != null)
            {
                yield return new WaitForSeconds(0.01f);
                float y = Mathf.Cos(Time.time * frequency) * amp;
                transform.position = new Vector3(transform.position.x, baseY + y, 0);
                transform.Translate(Vector3.down * Time.deltaTime * _data.speed);
            }
        }

        public float GetDamageMod()
        {
            return damageMod;
        }

        public bool CanHitThings()
        {
            return _canHit;
        }

        public int GetNumberOfBounces()
        {
            return _numberOfBounces;
        }

        public void AttemptBounce(Vector2 normal)
        {
            var component = GetComponent<Rigidbody2D>();

            if (!(normal.x * component.velocity.x >= 0 && normal.y * component.velocity.y >= 0))
            {
                _numberOfBounces--;
                component.velocity = Vector2.Reflect(component.velocity, normal);
            }
        }

        void Spin()
        {
            transform.rotation *= Quaternion.Euler(0, 0, spinSpeed);
        }

        IEnumerator GravityShift()
        {
            int mult = 2;
            for (int i = 0; i < flipAmount; i++)
            {
                rb.gravityScale = gravityDir;
                yield return new WaitForSeconds(gravityTimer / mult);
                rb.gravityScale = -gravityDir;
                mult = 1;
                yield return new WaitForSeconds(gravityTimer);
            }
        }

        IEnumerator Burst()
        {
            yield return new WaitForSeconds(burstTimer);
            float angle = 360 / burst;
            for (float i = 0; i < 360; i += angle)
            {
                GameObject shot = Instantiate(projectileRef, transform.position, Quaternion.Euler(0, 0, i));
                shot.GetComponent<Rigidbody2D>().velocity = -shot.transform.up * (_data.speed2 + _data.speed);
                DirectProjectile projectile = shot.GetComponent<DirectProjectile>();
                shot.GetComponent<Collision>().noStabbing = false;
                shot.GetComponent<Rigidbody2D>().gravityScale = 0;

                projectile.SetShotData(_data);
                projectile.damageMod = 0.5f;
                projectile.burstTimer = 0;
                shot.transform.localScale *= 0.25f;
                Destroy(shot, 10);
            }

            Destroy(gameObject);
        }

        public IShotData GetShotData()
        {
            return _data;
        }

        public void SetShotData(BlueShotDataObject data)
        {
            _data = data;
            _numberOfBounces = _data.numberOfBounces;
        }
    }
}
