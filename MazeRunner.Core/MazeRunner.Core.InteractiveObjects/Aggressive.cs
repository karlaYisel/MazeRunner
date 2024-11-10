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
            List<Character> possibleOponents = maze.GetPossibleOponents(this.X, this.Y, maze.Players[maze.Players.Count() - 1], TypeOfAttack.Large);
            if (possibleOponents.Count() > 0) 
            {
                this.Attack(possibleOponents[random.Next(0, possibleOponents.Count())]);
                return;
            }
            else
            {
                possibleOponents = maze.GetCharactersInCell(maze.Grid[this.X, this.Y]);
                for (int i = 0; i < possibleOponents.Count(); i++)
                {
                    if (this.Equals(possibleOponents[i]))
                    {
                        possibleOponents.Remove(possibleOponents[i]);
                        break;
                    }
                }
                List<(Cell cell, int distance)> cells;
                if (possibleOponents.Count() > 0)
                {
                    cells = maze.GetCellsInRange(this.X, this.Y, 2);
                    (Cell cell, int distance) actualCell =  cells[random.Next(0, cells.Count())];
                    this.Move(actualCell.cell.X, actualCell.cell.Y, actualCell.distance, maze);
                    this.Attack(possibleOponents[random.Next(0, possibleOponents.Count())]);
                    return;
                }
                cells = maze.GetCellsInRange(this.X, this.Y, this.Speed);
                foreach ((Cell cell, int distance) cell in cells)
                {
                    possibleOponents = maze.GetPossibleOponents(cell.cell.X, cell.cell.Y, maze.Players[maze.Players.Count() - 1], TypeOfAttack.Large);
                    for (int i = 0; i < possibleOponents.Count(); i++)
                    {
                        if (this.Equals(possibleOponents[i]))
                        {
                            possibleOponents.Remove(possibleOponents[i]);
                            break;
                        }
                    }
                    if (possibleOponents.Count() > 0)
                    {
                        this.Move(cell.cell.X, cell.cell.Y, cell.distance, maze);
                        this.Attack(possibleOponents[random.Next(0, possibleOponents.Count())]);
                        return;
                    }
                }
                this.RandomMove(maze);
            }
        }
    }
}