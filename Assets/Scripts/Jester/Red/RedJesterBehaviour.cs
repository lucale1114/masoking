using System;
using System.Linq;
using Jester.Blue;
using Unity.VisualScripting;
using UnityEngine;
using Wave.Jesters.Red;

namespace Jester.Red
{
    public class RedJesterBehaviour : AbstractStandingJesterBehaviour<RedJesterCommand>
    {
        private RedJesterFire _redJesterFire;

        private void Start()
        {

            _redJesterFire = GetComponent<RedJesterFire>();
        }

        protected override void CalculateLeaveTime()
        {
            var largestTime = jesterCommands
                .Select(command => command.shotData)
                .Select(data => data.amount + (data.amount == 0 ? 1 : 0) * data.fireBetween)
                .Prepend(0f)
                .Max();

            LeaveTime = enterTimestamp + largestTime + 0.2f;
        }

        protected override void OnCommandTime(RedJesterCommand command)
        {
            JesterAnimator.SetIdle();

            var data = command.shotData;

            switch (command.action)
            {
                case RedJesterActions.Throw:
                    Throw(data);
                    break;
                case RedJesterActions.ThrowAndRoll:
                    ThrowAndRoll(data);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Throw(RedShotDataObject data)
        {
            JesterAnimator.TriggerFire();
            _redJesterFire.Throw(data);
        }

        private void ThrowAndRoll(RedShotDataObject data)
        {
            JesterAnimator.TriggerFire();
            _redJesterFire.ThrowAndRoll(data);
        }
    }
}