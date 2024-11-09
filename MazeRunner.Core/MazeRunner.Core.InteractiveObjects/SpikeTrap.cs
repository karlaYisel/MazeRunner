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

        protected override void Trigger(Character character)
        {
            if (!(random.Next(0, 11 - character.Speed) == 0))
            {
                character.ActualLife -= 3*(this.Damage - character.Defense/2);
            }
        }
    }
}