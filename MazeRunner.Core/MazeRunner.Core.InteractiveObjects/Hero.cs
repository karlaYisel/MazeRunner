using System.Threading;

namespace MazeRunner.Core.InteractiveObjects
{
    public class Hero: PlayableCharacter
    {
        public Hero (int x, int y, int MaxLife = 100, int Defense = 7, int Strength = 7, int Ability = 7, int Speed = 4, int abilityRecoveryTime = 2)
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
            if (LastTurnUsingAbility + AbilityRecoveryTime > turn) return false;
            Thread.Sleep(100);
            if (opponent is NPC NonPlayable && NonPlayable.TargedCharacters is not null && !NonPlayable.TargedCharacters.Contains(this))
            {
                NonPlayable.TargedCharacters.Add(this);
            }
            opponent.CurrentLife -= 5*(this.Strength - opponent.Defense/3);
            LastTurnUsingAbility = turn;
            return true;
        }
    }
}