using System.Collections;
using UnityEngine;
using static WaveData;

namespace Projectile
{
    public class DirectProjectile : MonoBehaviour, IProjectile
    {
        private Vector3 sniperTrigger;
        private float sniperM;

        public GameObject projectileRef;
        public GameObject player;

        private float spinSpeed;
        private Rigidbody2D rb;

        public int burst;
        public float burstTimer;
        public float gravityTimer;
        public float gravityDir;
        public int flipAmount = 1;
        public bool spin;
        public bool sniper;
        public float damageMod = 1;

        public float frequency;
        public float amp;

        private ShotDataObject _data;

        // Code for projectile behavior.
        void Start()
        {
            transform.localScale *= _data.size + 1;
            rb = GetComponent<Rigidbody2D>();
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

            if (sniper)
            {
                /*gameObject.GetComponent<Collider2D>().enabled = false;
                sniperTrigger = new Vector3(x, y);*/
                //InvokeRepeating("Sniper", 0, 0.0001f);
            }

            if (spin || _data.spin)
            {
                spinSpeed = Random.Range(6.0f, 7.0f) * (Random.Range(0, 2) * 2 - 1);
                InvokeRepeating("Spin", 0, 0.005f);
            }
        }
        /*void Sniper()
        {
            if (!sniper)
            {
                return;
            }
            Vector3 pos = transform.position;
            if ((pos - sniperTrigger).magnitude < sniperM) {
                gameObject.GetComponent<Collider2D>().enabled = true;
                Destroy(target);
                sniper = false;
            }
        }*/

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
                if (shot.TryGetComponent<PolygonCollider2D>(out PolygonCollider2D col))
                {
                    col.enabled = false;
                }
                projectile.SetShotData(_data);
                projectile.damageMod = 0.5f;
                projectile.burstTimer = 0;
                shot.transform.localScale *= 0.25f;
                Destroy(shot, 10);
            }

            Destroy(gameObject);
        }

        public ShotDataObject GetShotData()
        {
            return _data;
        }

        public void SetShotData(ShotDataObject data)
        {
            _data = data;
        }
    }
}