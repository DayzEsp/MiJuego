using System;

namespace FateMazeGame
{
    public class SprintAbility : IAbility
    {
        public void Execute(Character character, Board board, char direction)
        {
            GameUI.DisplayTrapActivation($"{character.Name} ({character.Type}) usa la Velocidad de Rider. Esta habilidad permite moverse dos casillas en línea recta, siempre y cuando no choque con un obstáculo.");

            int dx = 0, dy = 0;
            switch (direction)
            {
                case 'W': dx = -2; dy = 0; break;
                case 'A': dx = 0; dy = -2; break;
                case 'S': dx = 2; dy = 0; break;
                case 'D': dx = 0; dy = 2; break;
            }

            int newX = character.X + dx;
            int newY = character.Y + dy;

            // Verificar si el camino está despejado y la casilla final está vacía
            if (board.IsValid(newX, newY) && board.Grid[character.X + dx / 2, character.Y + dy / 2] == " " && board.Grid[newX, newY] == " ")
            {
                board.Grid[character.X, character.Y] = " "; // Borra la posición anterior
                character.SetPosition(newX, newY);
                board.Grid[newX, newY] = character.Symbol;
                GameUI.DisplayTrapActivation("Movimiento exitoso.");
            }
            else
            {
                GameUI.DisplayTrapActivation("Movimiento inválido. La Velocidad de Rider falló.");
            }
        }
    }
}
