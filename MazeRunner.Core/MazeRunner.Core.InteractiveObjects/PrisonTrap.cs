using System.Threading;

namespace MazeRunner.Core.InteractiveObjects
{
    public class PrisonTrap: Trap
    {
        public PrisonTrap()
        {
            this.ActualState = State.Active;
            this.IsStandTrap = false;
        }

        protected override bool Trigger(Character character)
        {
            Thread.Sleep(100);
            if (random.Next(0, 11 - character.Speed) != 0)
            {
                return true;
            }
            return false;
        }
    }
}