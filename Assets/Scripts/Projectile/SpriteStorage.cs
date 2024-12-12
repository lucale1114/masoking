using Jester.Red;
using Misc;
using UnityEngine;

namespace Projectile
{
    public class SpriteStorage : Singleton<SpriteStorage>
    {
        [SerializeField] private Sprite[] throwAndRollSprites;

        public Sprite[] knifeSprites;

        public static Sprite GetThrowAndRollSprite(ThrowAndRollType type)
        {
            return Instance.throwAndRollSprites[(int)type];
        }
    }
}