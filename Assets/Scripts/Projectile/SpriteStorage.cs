using UnityEngine;

namespace Projectile
{
    public class SpriteStorage : MonoBehaviour
    {
        [SerializeField] private Sprite[] knifeSprites;
        [SerializeField] private Sprite[] breakableSprites;
        [SerializeField] private Sprite[] axeSprites;
        [SerializeField] private Sprite[] throwSprites;
        [SerializeField] private Sprite[] throwAndRollSprites;

        public Sprite GetKnifeSprite()
        {
            return knifeSprites[Random.Range(0, knifeSprites.Length)];
        }

        public Sprite GetAxeSprite()
        {
            return axeSprites[Random.Range(0, axeSprites.Length)];
        }

        public Sprite GetBreakableSprite()
        {
            return breakableSprites[Random.Range(0, breakableSprites.Length)];
        }

        public Sprite GetThrowSprite()
        {
            return throwSprites[Random.Range(0, throwSprites.Length)];
        }

        public Sprite GetThrowAndRollSprite()
        {
            return throwAndRollSprites[Random.Range(0, throwAndRollSprites.Length)];
        }
    }
}