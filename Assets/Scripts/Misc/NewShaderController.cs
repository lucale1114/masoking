using Player;
using UnityEngine;

namespace Misc
{
    public class NewShaderController : MonoBehaviour
    {

        private static readonly int Twirl = Shader.PropertyToID("_Twirl");
        private static readonly int Speed = Shader.PropertyToID("_Speed");
        private static readonly int MoveX = Shader.PropertyToID("_MoveX");
        private static readonly int MoveY = Shader.PropertyToID("_MoveY");

        private float baseTwirl;
        private float baseSpeed;
        private float xSpeed;
        private float ySpeed;

        public float twirlMultiplier = 1;
        public float speedMultiplier = 1;

        private HeatSystem _heatSystem;
        private Material _material;

        // Start is called before the first frame update
        void Start()
        {
            _heatSystem = FindObjectOfType<HeatSystem>();
            _material = GetComponent<MeshRenderer>().material;

            _heatSystem.TakenDamage +=  Increase;

            baseTwirl = _material.GetFloat(Twirl);
            baseSpeed = _material.GetFloat(Speed);
            xSpeed = _material.GetFloat(MoveX);
            ySpeed = _material.GetFloat(MoveY);

        }

        // Update is called once per frame
        void Update()
        {
            var _twirl = _material.GetFloat(Twirl);
            var _speed = _material.GetFloat(Speed);
            var _moveX = _material.GetFloat(MoveX);
            var _moveY = _material.GetFloat(MoveY);

            _twirl = Mathf.Lerp(_twirl, baseTwirl, (Time.deltaTime * 6) * speedMultiplier);
            _material.SetFloat(Twirl, _twirl);
        }

        public void Increase(float amount){
            var _twirl = _material.GetFloat(Twirl);
            _material.SetFloat(Twirl, _twirl + (amount * twirlMultiplier));
           // var _moveX = _material.GetFloat(MoveX);
           // _material.SetFloat(MoveX, _moveX + 0.1f);
        }
    }
}
