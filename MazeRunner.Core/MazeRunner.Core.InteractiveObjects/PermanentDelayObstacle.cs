namespace MazeRunner.Core.InteractiveObjects
{
    public class PermanentDelayObstacle: Obstacle
    {
        public int Lapse  { get; private set; }

        public PermanentDelayObstacle(int Thickness)
        {
            if(Thickness > 4) Thickness = 4;
            if(Thickness < 2) Thickness = 2;
            this.ActualState = State.Active;
            this.Delay = Thickness;
        }
    }
}