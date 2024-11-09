namespace MazeRunner.Core.InteractiveObjects
{
    public class TemporalWall: Obstacle
    {
        public int Lapse  { get; private set; }

        public TemporalWall(int Lapse)
        {
            this.ActualState = State.Active;
            this.Delay = 5;
            this.Lapse = Lapse;
        }

        //Poner algo para el Lapse con los turnos
    }
}