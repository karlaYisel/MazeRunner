namespace MazeRunner.Core.InteractiveObjects
{
    public class PermanentDelayObstacle: Obstacle
    {
        public int Lapse  { get; private set; }
        public int Thickness  { get; private set; }

        public PermanentDelayObstacle()
        {
            this.ActualState = State.Active;
            this.Delay = 2;
        }
    }
}