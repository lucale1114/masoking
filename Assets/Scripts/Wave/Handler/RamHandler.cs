using System.Collections.Generic;
using UnityEngine;
using Wave.Hazards.Ram;

namespace Wave.Handler
{
    public class RamHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject _batteringRam;

        const float RIGHT_X = 6;
        const float LEFT_X = -6;
        const float UP_Y = 5.2f;
        const float DOWN_Y = -5.2f;

        private List<BatteringRamData> rams;
        readonly List<GameObject> currentRams = new();

        public void SetRams(List<BatteringRamData> newRams)
        {
            rams = newRams;
        }

        public void StartWave()
        {
            currentRams.Clear();
            foreach (var item in rams)
            {
                currentRams.Add(SpawnBatteringRam(item));
            }
        }

        public GameObject SpawnBatteringRam(BatteringRamData _data)
        {
            GameObject newRam = Instantiate(_batteringRam);
            BatteringRam ramScript = newRam.transform.GetChild(0).GetComponent<BatteringRam>();
            ramScript.data = _data;

            float x = _data.x;
            float y = _data.y;

            ramScript.dirY = 0;
            ramScript.dirX = 1;

            switch (_data.side)
            {
                case WallSides.Left:
                    x = LEFT_X;
                    newRam.transform.rotation = Quaternion.Euler(0, 0, 180);
                    newRam.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                    ramScript.dirX = -1;
                    break;

                case WallSides.Down:
                    y = DOWN_Y;
                    newRam.transform.rotation = Quaternion.Euler(0, 0, -90);
                    ramScript.dirX = 0;
                    ramScript.dirY = -1;
                    break;

                case WallSides.Up:
                    y = UP_Y;
                    newRam.transform.rotation = Quaternion.Euler(0, 0, 90);
                    ramScript.dirX = 0;
                    ramScript.dirY = 1;
                    break;
            }

            newRam.transform.position = new Vector3 (x, y, 0);
            return newRam;
        }
    }
}