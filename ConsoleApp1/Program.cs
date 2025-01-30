using FateMazeGame;
using Spectre.Console;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            AnsiConsole.Clear();
            GameUI.DisplayMainMenu(); // Mostrar el menú principal

            // Si llegamos aquí, el usuario eligió "Empezar la guerra"
            GameUI.DisplayTitle();
            Board board = new Board(21); // Aumentar el tamaño del laberinto a 21

            // Mostrar el tablero inicial
            board.DisplayBoard();

            // Aquí puedes agregar lógica para actualizar el tablero en el lugar
            // Por ejemplo, después de cada movimiento o acción del jugador
            while (true)
            {
                // Lógica del juego, por ejemplo, mover personajes, activar trampas, etc.
                // Después de cada acción, limpiar la consola y mostrar el tablero actualizado
                board.DisplayBoard();

                // Aquí puedes agregar una condición para salir del bucle interno
                // Por ejemplo, si el juego ha terminado o el jugador ha ganado
                if (GameHasEnded(board)) // condición para terminar el juego
                {
                    break;
                }

                // Lógica para procesar el turno del jugador
                // Por ejemplo, mover personajes, activar trampas, etc.
                // Después de cada acción, limpiar la consola y mostrar el tablero actualizado
                ProcessPlayerTurn(board);
            }

            // Si el jugador no quiere jugar otra partida, salimos del bucle
            if (!GameUI.AskToPlayAgain())
            {
                break;
            }
        }
    }

    static bool GameHasEnded(Board board)
    {
        // Lógica para determinar si el juego ha terminado
        return board.CheckVictory(board.Player1Character) || board.CheckVictory(board.Player2Character);
    }

    static void ProcessPlayerTurn(Board board)
    {
        // Lógica para procesar el turno del jugador
        // Por ejemplo, mover personajes, activar trampas, etc.
        // Después de cada acción, limpiar la consola y mostrar el tablero actualizado
        foreach (var character in board.Characters)
        {
            if (board.Traps.TryGetValue((character.X, character.Y), out TrapBase trap))
            {
                trap.Activate(character, board);
                AnsiConsole.MarkupLine($"[red]¡{character.Name} ha activado una trampa![/]");
            }
        }
    }
}
