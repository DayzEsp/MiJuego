using System;

namespace FateMazeGame
{
    public class DivergingShotAbility : IAbility
    {
        public void Execute(Character character, Board board, char direction)
        {
            GameUI.DisplayTrapActivation($"{character.Name} ({character.Type}) realiza la Flecha de Emiya. Esta habilidad permite activar una trampa a distancia, despejando el camino.");

            int dx = 0, dy = 0;
            switch (direction)
            {
                case 'W': dx = -1; dy = 0; break;
                case 'A': dx = 0; dy = -1; break;
                case 'S': dx = 1; dy = 0; break;
                case 'D': dx = 0; dy = 1; break;
            }

            int range = 3; // Rango de ejemplo
            for (int i = 1; i <= range; i++)
            {
                int newX = character.X + (dx * i);
                int newY = character.Y + (dy * i);

                if (board.IsValid(newX, newY) && board.Grid[newX, newY] == "T")
                {
                    board.Grid[newX, newY] = " ";
                    GameUI.DisplayTrapActivation($"Trampa en ({newX}, {newY}) activada y despejada.");
                    return;
                }
            }
            GameUI.DisplayTrapActivation("No hay trampas dentro del rango para activar.");
        }
    }
}
