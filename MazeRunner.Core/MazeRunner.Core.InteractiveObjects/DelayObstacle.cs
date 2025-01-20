namespace MazeRunner.Core.InteractiveObjects
{
    public class DelayObstacle: Obstacle
    {
        public int Lapse  { get; private set; }

        public DelayObstacle(int Lapse, int Thickness)
        {
            this.ActualState = State.Inactive;
            if(Thickness > 4) Thickness = 4;
            if(Thickness < 2) Thickness = 2;
            this.Delay = Thickness;
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