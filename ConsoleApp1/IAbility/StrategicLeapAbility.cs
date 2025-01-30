using System;

namespace FateMazeGame
{
    public class StrategicLeapAbility : IAbility
    {
        public void Execute(Character character, Board board, char direction)
        {
            GameUI.DisplayTrapActivation($"{character.Name} ({character.Type}) realiza el Salto de Shirou Emiya. Esta habilidad permite saltar obstáculos y aterrizar en la casilla detrás del obstáculo.");

            int dx = 0, dy = 0;
            switch (direction)
            {
                case 'W': dx = -2; dy = 0; break; // Salta dos filas hacia arriba
                case 'A': dx = 0; dy = -2; break; // Salta dos columnas hacia la izquierda
                case 'S': dx = 2; dy = 0; break; // Salta dos filas hacia abajo
                case 'D': dx = 0; dy = 2; break; // Salta dos columnas hacia la derecha
            }

            int newX = character.X + dx;
            int newY = character.Y + dy;

            // Verificar si la casilla detrás del obstáculo está vacía
            if (board.IsValid(newX, newY) && board.Grid[character.X + dx / 2, character.Y + dy / 2] == "■" && board.Grid[newX, newY] == " ")
            {
                board.Grid[character.X, character.Y] = " "; // Borra la posición anterior
                character.SetPosition(newX, newY);
                board.Grid[newX, newY] = character.Symbol;
                GameUI.DisplayTrapActivation("Movimiento exitoso.");
            }
            else
            {
                GameUI.DisplayTrapActivation("Movimiento inválido. El Salto de Shirou Emiya falló.");
            }
        }
    }
}
