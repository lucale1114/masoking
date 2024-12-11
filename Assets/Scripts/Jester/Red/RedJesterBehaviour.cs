using System;
using System.Linq;
using UnityEngine;
using Wave.Jesters.Red;

namespace Jester.Red
{
    public class RedJesterBehaviour : AbstractStandingJesterBehaviour<RedJesterCommand>
    {
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
            JesterFire.Throw(data);
        }

        private void ThrowAndRoll(RedShotDataObject data)
        {
            JesterAnimator.TriggerFire();
            JesterFire.ThrowAndRoll(data);
        }
    }
}