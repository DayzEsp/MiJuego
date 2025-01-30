using System;

namespace FateMazeGame
{
    public class ChargeAbility : IAbility
    {
        public void Execute(Character character, Board board, char direction)
        {
            GameUI.DisplayTrapActivation($"{character.Name} ({character.Type}) realiza la Arremetida de Berserker. Esta habilidad permite destruir un obstáculo adyacente y moverse a esa casilla.");

            int dx = 0, dy = 0;
            switch (direction)
            {
                case 'W': dx = -1; dy = 0; break;
                case 'A': dx = 0; dy = -1; break;
                case 'S': dx = 1; dy = 0; break;
                case 'D': dx = 0; dy = 1; break;
            }

            int newX = character.X + dx;
            int newY = character.Y + dy;

            if (board.IsValid(newX, newY) && board.Grid[newX, newY] == "■")
            {
                board.Grid[character.X, character.Y] = " "; // Borra la posición anterior
                board.Grid[newX, newY] = " ";
                character.SetPosition(newX, newY);
                board.Grid[newX, newY] = character.Symbol;
                GameUI.DisplayTrapActivation("Obstáculo destruido y movimiento exitoso.");
            }
            else
            {
                GameUI.DisplayTrapActivation("Movimiento inválido. La Arremetida de Berserker falló.");
            }
        }
    }
}
