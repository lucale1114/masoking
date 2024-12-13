using System;
using System.Linq;

namespace Jester.Red
{
    public class RedJesterBehaviour : AbstractStandingJesterBehaviour<RedJesterCommand>
    {
        private RedJesterFire _redJesterFire;

        private new void Start()
        {
            base.Start();
            _redJesterFire = GetComponent<RedJesterFire>();
        }

        protected override void CalculateLeaveTime()
        {
            LeaveTime = jesterCommands
                .Select(command => 0.3f + command.timestamp)
                .Prepend(0f)
                .Max();
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
                case RedJesterActions.ThrowAndExplode:
                    ThrowAndExplode(data);
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

        private void ThrowAndExplode(RedShotDataObject data)
        {
            JesterAnimator.TriggerFire();
            _redJesterFire.ThrowAndExplode(data);
        }
    }
}