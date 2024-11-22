using System.Threading;

namespace MazeRunner.Core.InteractiveObjects
{
    public class Hero: PlayableCharacter
    {
        public Hero (int x, int y, int MaxLife = 100, int Defense = 7, int Streng = 7, int Ability = 7, int Speed = 4, int abilityRecoveryTime = 2)
        {
            ActualState = State.Active;
            X = x;
            Y = y;
            IsTargeted =false;
            this.MaxLife = MaxLife;
            ActualLife = MaxLife;
            this.Defense = Defense;
            this.Streng = Streng;
            this.Ability = Ability;
            this.Speed = Speed;
            TypeOfAttack = TypeOfAttack.Large;
            AbilityRecoveryTime = abilityRecoveryTime;
            LastTurnUsingAbility = 0;
        }

        public override bool ActivateAbility(int turn, Character oponent)
        {
            if (LastTurnUsingAbility + AbilityRecoveryTime > turn) return false;
            Thread.Sleep(100);
            if (oponent is NPC NonPlayable && NonPlayable.TargedCharacters is not null && !NonPlayable.TargedCharacters.Contains(this))
            {
                NonPlayable.TargedCharacters.Add(this);
            }
            oponent.ActualLife -= 5*(this.Streng - oponent.Defense/3);
            LastTurnUsingAbility = turn;
            return true;
        }
    }
}