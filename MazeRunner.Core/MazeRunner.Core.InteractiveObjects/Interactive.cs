namespace MazeRunner.Core.InteractiveObjects
{
    public enum TypeOfInteractive
    {
        Obstacle,
        Trap,
    }

    public enum State
    {
        Active,
        Inactive,
    }

    public abstract class Interactive
    {
        public State ActualState { get; protected set; }

        public void ChangeState()
        {
            if (this.ActualState == State.Active) {this.ActualState = State.Inactive; }
            else {this.ActualState = State.Active; }
        }
    }
}