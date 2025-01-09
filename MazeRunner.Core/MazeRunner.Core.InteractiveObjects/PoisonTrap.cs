using System.Threading;

namespace MazeRunner.Core.InteractiveObjects
{
    public class PoisonTrap: Trap
    {
        public int Damage;
        public int MaxTurns;

        public PoisonTrap(int Damage, int IsStandTrap, int MaxTurns)
        {
            this.ActualState = State.Active;
            this.IsStandTrap = (IsStandTrap == 0) ? false: true;
            this.Damage = Damage;
            this.MaxTurns = MaxTurns;
        }

        protected override bool Trigger(Character character)
        {
            Thread.Sleep(100);
            if (this.Damage - character.Defense/2 > 0 && random.Next(0, 11 - character.Speed) != 0)
            {
                character.CurrentLife -= 3*(this.Damage - character.Defense/2);
                character.ChangeStates(0, 0, random.Next(1, MaxTurns + 1));
                return true;
            }
            return false;
        }
    }
}