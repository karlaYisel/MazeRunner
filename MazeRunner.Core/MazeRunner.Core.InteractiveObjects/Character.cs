using System.Threading;

namespace MazeRunner.Core.InteractiveObjects
{
    public abstract class Character: Interactive
    {
        public int X  { get; protected set; }
        public int Y { get; protected set;}
        public bool IsTargeted { get; protected set;}
        public int MaxLife { get; protected set; }
        public int CurrentLife; 
        public int Defense { get; protected set; }
        public int Strength { get; protected set; }
        public int Ability { get; protected set; }
        public int Speed { get; protected set; }

        public void ChangeTargetStatus()
        {
            if (IsTargeted) IsTargeted = false;
            else IsTargeted = true;
        }

        public void ChangePosition(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public bool Attack(Character opponent)
        {
            Thread.Sleep(100);
            if (this.Ability - opponent.Speed/2 > 1 && random.Next(0, this.Ability - opponent.Speed/2) != 0 && this.Strength - opponent.Defense/2 > 0)
            {
                if (opponent is NPC neutral && neutral.TargedCharacters is not null && !neutral.TargedCharacters.Contains(this))
                {
                    neutral.TargedCharacters.Add(this);
                }
                opponent.CurrentLife -= 5*(this.Strength - opponent.Defense/2);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}