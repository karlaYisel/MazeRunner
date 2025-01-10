namespace MazeRunner.Core.InteractiveObjects
{
    public enum TypeOfTrap
    {
        SpikeTrap,
        PrisonTrap,
        FireTrap,
        IceTrap,
        PoisonTrap,
    }

    public abstract class Trap: Interactive
    {
        //IsStandTrap == true if the trap only get trigged when the player stand over it
        //IsStandTrap == false if get trigged when the player walks over it
        public bool IsStandTrap { get; protected set; }
        public bool IsVisible { get; protected set; } = false;

        public void ChangeVisibility(bool visibility)
        {
            IsVisible = visibility;
        }

        public bool TryToTrigger(Character character)
        {
            if (this.ActualState == State.Active)
            {
                return this.Trigger(character);
            }
            else
            {
                return false;
            }
        }

        protected virtual bool Trigger (Character character)
        {
            return false;
        }
    }
}