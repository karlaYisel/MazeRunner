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

        public bool TryToTrigger(Character character)
        {
            if (this.ActualState == State.Active)
            {
                this.Trigger(character);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual void Trigger (Character character)
        {

        }
    }
}