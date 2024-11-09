using MazeRunner.Core.MazeGenerator;

namespace MazeRunner.Core.InteractiveObjects
{
    public class Passive: NPC
    {
        public Passive (int x, int y, int MaxLife = 50, int Defense = 5, int Streng = 6, int Ability = 5, int Speed = 4)
        {
            this.ActualState = State.Active;
            this.X = x;
            this.Y = y;
            this.MaxLife = MaxLife;
            this.ActualLife = MaxLife;
            this.Defense = Defense;
            this.Streng = Streng;
            this.Ability = Ability;
            this.Speed = Speed;
        }

        override public void TakeTurn (Maze maze)
        {
            this.RandomMove(maze);
        }
    }
}