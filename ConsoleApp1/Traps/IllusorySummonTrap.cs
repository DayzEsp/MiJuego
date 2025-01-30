using System;

namespace FateMazeGame
{
    public class IllusorySummonTrap : TrapBase
    {
        public override void Activate(Character character, Board board)
        {
            GameUI.DisplayTrapActivation("Trampa de Invocación Ilusoria");

            string answer = GameUI.GetPiAnswer();

            if (answer == "141592")
            {
                GameUI.DisplayGameStatus("Respuesta correcta. No pasa nada.");
            }
            else
            {
                GameUI.DisplayGameStatus("Respuesta incorrecta. El laberinto se reconfigura.");

                board.Grid[character.X, character.Y] = " ";
                board.RemoveTrap(character.X, character.Y);
                board.DecreaseIllusoryTrapCount();
                board.Reconfigure();

                GameUI.DisplayGameStatus("El laberinto se ha reconfigurado, y la trampa de ilusión ha desaparecido.");
            }
        }
    }
}