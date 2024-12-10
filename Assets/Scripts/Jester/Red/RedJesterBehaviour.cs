using System;
using UnityEngine;
using Wave.Jesters.Red;

namespace Jester.Red
{
    public class RedJesterBehaviour : AbstractStandingJesterBehaviour<RedJesterCommand>
    {
        protected override void CalculateLeaveTime()
        {
            foreach (var command in jesterCommands)
            {
                var data = command.shotData;
                var additionIfOnlyFb = 0;
                if (data.amount == 0)
                {
                    additionIfOnlyFb++;
                }

                LeaveTime = Mathf.Max(enterTimestamp + command.timestamp + 1f,
                    enterTimestamp + command.timestamp + (data.amount + additionIfOnlyFb) * data.fireBetween + 0.5f);
            }
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