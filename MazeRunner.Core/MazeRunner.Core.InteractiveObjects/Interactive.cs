namespace MazeRunner.Core.InteractiveObjects
{
    public enum TypeOfInteractive
    {
        Obstacle,
        Trap,
        Character,
    }

    public enum State
    {
        Active,
        Inactive,
    }

    public abstract class Interactive
    {
        public State ActualState { get; protected set; }
        protected Random random = new Random();

        public void ChangeState()
        {
            if (this.ActualState == State.Active) {this.ActualState = State.Inactive; }
            else {this.ActualState = State.Active; }
        }
    }
}