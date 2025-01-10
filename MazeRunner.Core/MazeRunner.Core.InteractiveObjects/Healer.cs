using System.Threading;

namespace MazeRunner.Core.InteractiveObjects
{
    public class Healer: PlayableCharacter
    {
        public Healer (int x, int y, int MaxLife = 70, int Defense = 5, int Strength = 5, int Ability = 8, int Speed = 4, int abilityRecoveryTime = 2)
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
            TypeOfAttack = TypeOfAttack.Short;
            AbilityRecoveryTime = abilityRecoveryTime;
            LastTurnUsingAbility = 0;
        }

        public override bool ActivateAbility(int turn, Character opponent) 
        {
            if (LastTurnUsingAbility + AbilityRecoveryTime > turn) return false;
            Thread.Sleep(100);
            opponent.CurrentLife += 2*(this.Ability)/3;
            LastTurnUsingAbility = turn;
            return true;
        }
    }
}