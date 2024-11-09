using MazeRunner.Core.MazeGenerator;

namespace MazeRunner.Core.InteractiveObjects
{
    public class Aggressive: NPC
    {
        public Aggressive (int x, int y, int MaxLife, int Defense, int Streng, int Ability, int Speed)
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
            //Usar m'etodo de obtener oponentes
            this.RandomMove(maze);
        }
    }
}