using Jester.Red;
using UnityEngine;

namespace Projectile
{
    public class SpriteStorage : MonoBehaviour
    {
        [SerializeField] private Sprite[] throwAndRollSprites;

        public Sprite[] knifeSprites;

        public Sprite GetThrowAndRollSprite(ThrowAndRollType type)
        {
            return throwAndRollSprites[(int)type];
        }
    }
}