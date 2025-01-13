namespace MazeRunner.Core.InteractiveObjects
{
    public enum TypeOfPlayableCharacter
    {
        Hero,
        Thief,
        Healer,
        Paladin,
        Archer,
    }

    public enum TypeOfAttack
    {
        Short,
        Large,
        Distance,
    }

    public enum OptionsByToken
    {
        Move,
        Attack,
        UseAbility,
    }

    public abstract class PlayableCharacter: Character
    {
        public TypeOfAttack TypeOfAttack { get; protected set; }
        public bool HasMoved { get; protected set; }
        public bool HasAttacked { get; protected set; }
        public int AbilityRecoveryTime { get; protected set; }
        public int LastTurnUsingAbility { get; protected set; }

        public virtual bool ActivateAbility(int turn, Character oponent)
        {
            return false;
        }

        public async Task NewTurn(int turn)
        {
            await Task.Delay(100);
            if(turn > 0)
            {
                HasMoved = false;
                HasAttacked = false;
            }
        }

        public void Moved()
        {
            HasMoved = true;
        }

        public void Attacked()
        {
            HasAttacked = true;
        }
    }
}