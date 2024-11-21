using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static WaveData;

namespace Projectile
{
    public class Projectile : MonoBehaviour
    {
        private Vector3 sniperTrigger;

        public GameObject projectileRef;
        public GameObject player;

        private float spinSpeed;
        private Rigidbody2D rb;

        public int burst;
        public float burstTimer;
        public float gravityTimer;
        public float gravityDir;
        public int flipAmount = 1;
        public float speed;
        public bool spin;
        public bool sniper;

        public float frequency;
        public float amp;

        public float x;
        public float y;

        public ShotDataObject data;
        public GameObject target;

        // Code for projectile behavior.
        void Start()
        {
            transform.localScale *= data.size + 1;
            rb = GetComponent<Rigidbody2D>();
            if (burstTimer > 0)
            {
                StartCoroutine(Burst());
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
                gameObject.GetComponent<Collider2D>().enabled = false;
                sniperTrigger = new Vector3(x, y);
                InvokeRepeating("Sniper", 0, 0.0001f);
            }

            if (spin || data.spin)
            {
                spinSpeed = Random.Range(6.0f, 7.0f) * (Random.Range(0, 2) * 2 - 1);
                InvokeRepeating("Spin", 0, 0.005f);
            }
        }
        void Sniper()
        {
            if (!sniper)
            {
                return;
            }
            Vector3 pos = transform.position;
            if ((pos - sniperTrigger).magnitude < 3) {
                gameObject.GetComponent<Collider2D>().enabled = true;
                Destroy(target);
                sniper = false;
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
                transform.Translate(Vector3.down * Time.deltaTime * speed);
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
                shot.GetComponent<Rigidbody2D>().velocity = -shot.transform.up * (data.speed2 + speed);
                shot.GetComponent<Projectile>().data.damage = data.damage / 2;
                shot.GetComponent<Projectile>().burstTimer = 0;
                shot.GetComponent<Projectile>().data.size = data.size * 0.75f;
                Destroy(shot, 10);
            }
            Destroy(gameObject);
        }
    }
}