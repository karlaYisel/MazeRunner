using System.Threading;

namespace MazeRunner.Core.InteractiveObjects
{
    public class Paladin: PlayableCharacter
    {
        public Paladin (int x, int y, int MaxLife = 100, int Defense = 7, int Strength = 7, int Ability = 5, int Speed = 2, int abilityRecoveryTime = 2)
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
            TypeOfAttack = TypeOfAttack.Large;
            AbilityRecoveryTime = abilityRecoveryTime;
            LastTurnUsingAbility = 0;
        }

        public override bool ActivateAbility(int turn, Character opponent)
        {
            return false;
        }

        public bool ActivateAbility(int turn)
        {
            if (LastTurnUsingAbility + AbilityRecoveryTime > turn) return false;
            Thread.Sleep(100);
            this.Defense += (this.Ability);
            LastTurnUsingAbility = turn;
            return true;
        }

        public void CheckAbility(int turn)
        {
            if(LastTurnUsingAbility != 0 && LastTurnUsingAbility - AbilityRecoveryTime == turn) 
            {
                this.Defense -= (this.Ability);
            }
        }
    }
}