namespace MazeRunner.Core.InteractiveObjects
{
    public class Hero: PlayableCharacter
    {
        public Hero (int x, int y, int MaxLife = 100, int Defense = 7, int Streng = 7, int Ability = 7, int Speed = 4)
        {
            this.ActualState = State.Active;
            this.X = x;
            this.Y = y;
            this.MaxLife = MaxLife;
            this.ActualLife = MaxLife;
            this.Defense = Defense;
            this.Streng = Streng;
            this.Ability = Ability;
            this.Speed = Speed;
            this.TypeOfAttack = TypeOfAttack.Short;
        }

        public bool ActivateAbility(Character oponent)
        {
            //Configurar para tener en cuenta los turnos
            //Espada Sagrada quitar da;o enemigos pero mucho m'as da;o y es constante
            //metodo de maze de obtener enemigos
            return true;
        }
    }
}