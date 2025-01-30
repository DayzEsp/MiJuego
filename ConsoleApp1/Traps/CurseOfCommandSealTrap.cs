using System;

namespace FateMazeGame
{
    public class CurseOfCommandSealTrap : TrapBase
    {
        public override void Activate(Character character, Board board)
        {
            GameUI.DisplayTrapActivation("Maldición del Sello de Comando");
            GameUI.DisplayGameStatus($"{character.Name} no puede usar habilidades durante los próximos 5 turnos.");

            character.CurrentCooldown += 5;
            board.Grid[character.X, character.Y] = character.Symbol;
            board.RemoveTrap(character.X, character.Y);
        }
    }
}

