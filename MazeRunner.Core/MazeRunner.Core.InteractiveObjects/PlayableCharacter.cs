namespace MazeRunner.Core.InteractiveObjects
{
    public enum TypeOfPlayableCharacter
    {
        Hero,
    }

    public enum TypeOfAttack
    {
        Short,
        Large,
        Distance,
    }

    public abstract class PlayableCharacter: Character
    {
        public TypeOfAttack TypeOfAttack { get; protected set; }
        public int AbilityRecoveryTime { get; protected set; }
        public int LastTurnUsingAbility { get; protected set; }

        public bool ActivateAbility()
        {
            return false;
        }
    }
}