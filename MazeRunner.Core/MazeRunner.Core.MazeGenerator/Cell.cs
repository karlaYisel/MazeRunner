using System;
using MazeRunner.Core.InteractiveObjects;

namespace MazeRunner.Core.MazeGenerator
{
    public class Cell
    {
        public int X {get; private set;} 
        public int Y {get; private set;}
        public bool IsActive {get; private set;}
        public Interactive? Interactive {get; internal set;} 
        internal bool isVisited;
        public Dictionary<string, bool> Walls = new Dictionary<string, bool>
        {
            { "top", true },
            { "right", true },      
            { "bottom", true },
            { "left", true }
        };
    
        public Cell(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
            this.isVisited = false;
            this.IsActive = false;
            this.Interactive = null;
        }

        public void ChangeActivity()
        {
            if (this.IsActive) {this.IsActive = false; }
            else {this.IsActive = true; }
        }
    }
}
