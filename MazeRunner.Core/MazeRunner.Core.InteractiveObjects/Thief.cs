using System.Threading;
using MazeRunner.Core.MazeGenerator;

namespace MazeRunner.Core.InteractiveObjects
{
    public class Thief: PlayableCharacter
    {
        public Thief (int x, int y, int MaxLife = 60, int Defense = 6, int Strength = 6, int Ability = 9, int Speed = 6, int abilityRecoveryTime = 2)
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
            return false;
        }

        public bool ActivateAbility(int turn, List<(Cell cell, int distance)> cells)
        {
            if (LastTurnUsingAbility + AbilityRecoveryTime > turn) return false;
            Thread.Sleep(100);
            foreach((Cell cell, int distance) cell in cells)
            {
                if(cell.cell.Interactive is Trap trap) trap.ChangeVisibility(true);
            }
            LastTurnUsingAbility = turn;
            return true;
        }

        public bool ActivateAbility(int turn, Trap? trap = null)
        {
            if (LastTurnUsingAbility + AbilityRecoveryTime > turn) return false;
            if(trap is not null) trap.ChangeState();
            LastTurnUsingAbility = turn;
            return true;
        }
    }
}