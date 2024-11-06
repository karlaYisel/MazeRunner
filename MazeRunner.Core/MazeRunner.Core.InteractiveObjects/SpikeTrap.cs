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

        //Jugador
        protected override void Trigger(object player)
        {
            
        }
    }
}