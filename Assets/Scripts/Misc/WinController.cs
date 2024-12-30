using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Misc
{
    public class WinController : MonoBehaviour
    {
        private InputAction _selectAction;

        private void Start()
        {
            _selectAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("Select");
        }

        private void Update()
        {
            if (_selectAction.IsPressed())
            {
                GameManager.LoadMenu();
            }
        }
    }
}