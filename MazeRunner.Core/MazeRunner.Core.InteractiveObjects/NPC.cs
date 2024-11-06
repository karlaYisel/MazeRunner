namespace MazeRunner.Core.InteractiveObjects
{
    public class NPC: Obstacle
    {
        //Hacer parecido a las fichas del jugador y agregar el movimiento (Si pienso en hacer más, 
        //agregar diferentes tipos de NPC, pasivos, neutrales y agresivos y se les cambia parámetros y NPC abstract)
        public NPC()
        {
            this.ActualState = State.Active;
        }
    }
}