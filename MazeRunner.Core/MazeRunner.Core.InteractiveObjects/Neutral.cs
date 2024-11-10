using MazeRunner.Core.MazeGenerator;

namespace MazeRunner.Core.InteractiveObjects
{
    public class Neutral: NPC
    {
        public List<Character> TargedCharacters = new List<Character>();

        public Neutral (int x, int y, int MaxLife, int Defense, int Streng, int Ability, int Speed)
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
            List<Character> possibleOponents = maze.GetPossibleOponents(this.X, this.Y, maze.Players[maze.Players.Count() - 1], TypeOfAttack.Large);
            foreach (Character character in possibleOponents)
            {
                if (TargedCharacters.Contains(character)) 
                { 
                    this.Attack(character); 
                    return;
                }
            }
            this.RandomMove(maze);
        }
    }
}