using System;

namespace FateMazeGame
{
    public class AgileSpinAbility : IAbility
    {
        public void Execute(Character character, Board board, char direction)
        {
            GameUI.DisplayTrapActivation($"{character.Name} ({character.Type}) realiza el Giro de Lancer. Esta habilidad permite moverse a una casilla adyacente y desactivar cualquier trampa en dicha casilla.");

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

            if (board.IsValid(newX, newY))
            {
                if (board.Grid[newX, newY] == "T")
                {
                    board.Grid[newX, newY] = " ";
                    GameUI.DisplayTrapActivation($"Trampa en ({newX}, {newY}) desactivada.");
                }
                board.Grid[character.X, character.Y] = " ";
                character.SetPosition(newX, newY);
                board.Grid[newX, newY] = character.Symbol;
                GameUI.DisplayTrapActivation("Movimiento exitoso.");
            }
            else
            {
                GameUI.DisplayTrapActivation("Movimiento inválido. El Giro de Lancer falló.");
            }
        }
    }
}
