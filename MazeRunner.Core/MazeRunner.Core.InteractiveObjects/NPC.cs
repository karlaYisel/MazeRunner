namespace MazeRunner.Core.InteractiveObjects
{   
    public enum TypeOfNPC
    {
        Passive,
        Neutral,
        Aggressive,
    }

    public class NPC: Character
    { 
        public TypeOfNPC TypeNPC { get; private set; }
        public List<Character>? TargedCharacters { get; private set; }
        public NPC (int x, int y, TypeOfNPC type, int MaxLife = 50, int Defense = 5, int Strength = 6, int Ability = 5, int Speed = 4)
        {
            ActualState = State.Active;
            X = x;
            Y = y;
            TypeNPC = type;
            if (type == TypeOfNPC.Neutral) TargedCharacters = new List<Character> ();
            IsTargeted = false;
            this.MaxLife = MaxLife;
            CurrentLife = MaxLife;
            this.Defense = Defense;
            this.Strength = Strength;
            this.Ability = Ability;
            this.Speed = Speed;
        }
    }
}