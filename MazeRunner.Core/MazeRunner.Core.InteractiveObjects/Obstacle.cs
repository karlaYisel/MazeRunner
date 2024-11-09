namespace MazeRunner.Core.InteractiveObjects
{
    public enum TypeOfObstacle
    {
        TemporalWall,
    }

    public abstract class Obstacle: Interactive
    {
        //Delay between 2 and 4 is the amount of speed needed to pass throw it
        //Delay equal to 5 means that the obstacle can not be passed, but this kind is temporal
        public int Delay { get; protected set; }
    }
}