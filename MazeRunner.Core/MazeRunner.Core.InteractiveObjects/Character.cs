using MazeRunner.Core.MazeGenerator;

namespace MazeRunner.Core.InteractiveObjects
{
    public enum TypeOfCharacter
    {
        PlayableCharacter,
        NPC,
    }

    public abstract class Character: Interactive
    {
        public int X  { get; protected set; }
        public int Y { get; protected set;}
        public int MaxLife { get; protected set; }
        public int ActualLife;
        public int Defense { get; protected set; }
        public int Streng { get; protected set; }
        public int Ability { get; protected set; }
        public int Speed { get; protected set; }

        public void Move(int x, int y, int distance, Maze maze)
        {
            Stack<Cell> optimalPath = maze.GetOptimalPath(this.X, this.Y, x, y, distance);
            for (int i = 0; i < optimalPath.Count(); i++)
            {
                Cell cell = optimalPath.Pop();
                this.ChangePosition(cell.X, cell.Y);
                //En dependencia de la interfaz aquí va un espacio para representar el movimiento
                if (cell.Interactive is Trap trap && (trap.IsStandTrap == false || optimalPath.Count() == 0)) {trap.TryToTrigger(this);}
            }
        }

        public void ChangePosition(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public bool Attack(Character oponent)
        {
            if (this.Ability - oponent.Speed/2 > 1 && random.Next(0, this.Ability - oponent.Speed/2) != 0 && this.Streng - oponent.Defense/2 > 0)
            {
                if (oponent is Neutral neutral && !neutral.TargedCharacters.Contains(this))
                {
                    neutral.TargedCharacters.Add(this);
                }
                oponent.ActualLife -= 5*(this.Streng - oponent.Defense/2);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}