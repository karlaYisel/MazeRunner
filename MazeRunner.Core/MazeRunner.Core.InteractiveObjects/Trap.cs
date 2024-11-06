namespace MazeRunner.Core.InteractiveObjects
{
    public enum TypeOfTrap
    {
        SpikeTrap,
    }

    public abstract class Trap: Interactive
    {
        //IsStandTrap == true if the trap only get trigged when the player stand over it
        //IsStandTrap == false if get trigged when the player walks over it
        public bool IsStandTrap { get; protected set; }

        //Configurar para el jugador
        public bool TryToTrigger(object player)
        {
            if (this.ActualState == State.Active)
            {
                this.Trigger(player);
                return true;
            }
            else
            {
                return false;
            }
        }

        //Jugador
        protected virtual void Trigger (object player)
        {

        }
    }
}