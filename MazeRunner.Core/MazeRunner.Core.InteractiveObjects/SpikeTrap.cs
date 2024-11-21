using System.Threading;

namespace MazeRunner.Core.InteractiveObjects
{
    public class SpikeTrap: Trap
    {
        public int Damage;

        public SpikeTrap(int Damage)
        {
            this.ActualState = State.Active;
            this.IsStandTrap = false;
            this.Damage = Damage;
        }

        protected override bool Trigger(Character character)
        {
            Thread.Sleep(100);
            if (this.Damage - character.Defense/2 > 0 && random.Next(0, 11 - character.Speed) != 0)
            {
                character.ActualLife -= 3*(this.Damage - character.Defense/2);
                return true;
            }
            return false;
        }
    }
}