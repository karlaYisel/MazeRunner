using System.Threading;

namespace MazeRunner.Core.InteractiveObjects
{
    public class FireTrap: Trap
    {
        public int Damage;
        public int MaxSteps;

        public FireTrap(int Damage, int IsStandTrap, int MaxSteps)
        {
            this.ActualState = State.Active;
            this.IsStandTrap = (IsStandTrap == 0) ? false: true;
            this.Damage = Damage;
            this.MaxSteps = MaxSteps;
        }

        protected override bool Trigger(Character character)
        {
            Thread.Sleep(100);
            if (this.Damage - character.Defense/2 > 0 && random.Next(0, 11 - character.Speed) != 0)
            {
                character.CurrentLife -= 3*(this.Damage - character.Defense/2);
                character.ChangeStates(random.Next(1, MaxSteps + 1), 0, 0);
                return true;
            }
            return false;
        }
    }
}