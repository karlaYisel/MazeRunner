using System.Threading;

namespace MazeRunner.Core.InteractiveObjects
{
    public class Archer: PlayableCharacter
    {
        public Archer (int x, int y, int MaxLife = 100, int Defense = 5, int Strength = 5, int Ability = 8, int Speed = 5, int abilityRecoveryTime = 2)
        {
            ActualState = State.Active;
            X = x;
            Y = y;
            IsTargeted =false;
            this.MaxLife = MaxLife;
            CurrentLife = MaxLife;
            this.Defense = Defense;
            this.Strength = Strength;
            this.Ability = Ability;
            this.Speed = Speed;
            TypeOfAttack = TypeOfAttack.Distance;
            AbilityRecoveryTime = abilityRecoveryTime;
            LastTurnUsingAbility = 0;
        }

        public override bool ActivateAbility(int turn, Character opponent) //The point about this Character is that he can shoot his ability in a range of 10 blocks
        {
            if (LastTurnUsingAbility + AbilityRecoveryTime > turn) return false;
            Thread.Sleep(100);
            opponent.CurrentLife -= 3*(this.Strength - opponent.Defense/3);
            LastTurnUsingAbility = turn;
            return true;
        }
    }
}