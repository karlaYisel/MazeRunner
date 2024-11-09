using MazeRunner.Core.MazeGenerator;

namespace MazeRunner.Core.InteractiveObjects
{   
    public enum TypeOfNPC
    {
        Passive,
        Neutral,
        Aggressive,
    }

    public abstract class NPC: Character
    { 
        protected void RandomMove (Maze maze)
        {
            List<(Cell cell, int distance)> cells = maze.GetCellsInRange(this.X, this.Y, this.Speed);
            if (random.Next(0, cells.Count + 1) == 0)
            {
                return;
            }
            else
            {
                (Cell cell, int distance) cellWithDistance = cells[random.Next(0, cells.Count)];
                this.Move(cellWithDistance.cell.X, cellWithDistance.cell.Y, cellWithDistance.distance, maze);
            }
        }

        virtual public void TakeTurn (Maze maze)
        {}
    }
}