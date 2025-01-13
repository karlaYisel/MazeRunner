namespace MazeRunner.Core.InteractiveObjects
{
    public class TemporalWall: Obstacle
    {
        public int Lapse  { get; private set; }

        public TemporalWall(int Lapse)
        {
            this.ActualState = State.Inactive;
            this.Delay = 5;
            this.Lapse = Lapse;
        }

        public async Task StabilizeWall(int turn)
        {
            await Task.Delay(100);
            turn %= 2*Delay;
            if (turn < 0) turn += 2*Delay;
            if ((turn < Delay && ActualState != State.Inactive) || (turn >= Delay && ActualState != State.Active)) ChangeState();
        }
    }
}