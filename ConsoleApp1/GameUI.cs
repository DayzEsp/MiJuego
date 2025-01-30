using Spectre.Console;
using System.Data;
using System.Drawing;

namespace FateMazeGame
{
    public class GameUI
    {
        // Estilos predefinidos para la interfaz
        private static readonly Style ServantNameStyle = new Style(foreground: Spectre.Console.Color.Gold1, decoration: Decoration.Bold);
        private static readonly Style HeaderStyle = new Style(foreground: Spectre.Console.Color.Red, decoration: Decoration.Bold);
        private static readonly Style TrapStyle = new Style(foreground: Spectre.Console.Color.Red3);
        private static readonly Style ObstacleStyle = new Style(foreground: Spectre.Console.Color.Grey);
        private static readonly Style GrialStyle = new Style(foreground: Spectre.Console.Color.Gold1, decoration: Decoration.SlowBlink);

        public static void DisplayMainMenu()
        {
            string[] logo = {
                @" ███████╗ █████╗ ████████╗███████╗     ██████╗ ██╗   ██╗██╗     ███████╗███████╗",
                @" ██╔════╝██╔══██╗╚══██╔══╝██╔════╝     ██╔══██╗██║   ██║██║     ██╔════╝██╔════╝",
                @" █████╗  ███████║   ██║   █████╗       ██████╔╝██║   ██║██║     █████╗  ███████╗",
                @" ██╔══╝  ██╔══██║   ██║   ██╔══╝       ██╔══██╗██║   ██║██║     ██╔══╝  ╚════██║",
                @" ██║     ██║  ██║   ██║   ███████╗     ██║  ██║╚██████╔╝███████╗███████╗███████║",
                @" ╚═╝     ╚═╝  ╚═╝   ╚═╝   ╚══════╝     ╚═╝  ╚═╝ ╚═════╝ ╚══════╝╚══════╝╚══════╝",
                @"",
                @"                   ▀▄▄▄▄▄▄▀▀▀▀▀▀▀▄▄▄▄▄▄▀                   ",
                @"                   █  Guerra por el  █                   ",
                @"                   █  Santo Grial    █                   ",
                @"                   ▄▀▀▀▀▀▀▀▄▄▄▄▄▄▄▀▀▀▀▀▀▀▄                   ",
                @"",
                @" ███████╗██╗   ██╗███████╗████████╗███████╗███╗   ███╗",
                @" ██╔════╝╚██╗ ██╔╝██╔════╝╚══██╔══╝██╔════╝████╗ ████║",
                @" ███████╗ ╚████╔╝ ███████╗   ██║   █████╗  ██╔████╔██║",
                @" ╚════██║  ╚██╔╝  ╚════██║   ██║   ██╔══╝  ██║╚██╔╝██║",
                @" ███████║   ██║   ███████║   ██║   ███████╗██║ ╚═╝ ██║",
                @" ╚══════╝   ╚═╝   ╚══════╝   ╚═╝   ╚══════╝╚═╝     ╚═╝"
            };

            int selectedOption = 0;
            string[] options = { "Empezar la guerra", "Salir" };
            ConsoleColor titleColor = ConsoleColor.Red;
            ConsoleColor optionColor = ConsoleColor.Yellow;

            while (true)
            {
                Console.Clear();

                // Mostrar logo con color
                Console.ForegroundColor = titleColor;
                foreach (string line in logo)
                {
                    Console.WriteLine(line);
                }
                Console.ResetColor();
                Console.WriteLine();

                // Mostrar opciones del menú
                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selectedOption)
                    {
                        Console.ForegroundColor = optionColor;
                        Console.WriteLine($"             > {options[i]} <");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"               {options[i]}");
                    }
                }

                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedOption = (selectedOption == 0) ? options.Length - 1 : selectedOption - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedOption = (selectedOption == options.Length - 1) ? 0 : selectedOption + 1;
                        break;
                    case ConsoleKey.Enter:
                        if (selectedOption == 0)
                            return; // Empezar el juego
                        else if (selectedOption == 1)
                            Environment.Exit(0); // Salir
                        break;
                }
            }
        }

        public static void DisplayTitle()
        {
            Console.Clear();
            AnsiConsole.Write(new Spectre.Console.Rule("[red]Guerra por el Santo Grial[/]").RuleStyle(Style.Parse("red")).DoubleBorder());
        }

        public static void DisplayBoard(Board board)
        {
            // Crear un nuevo canvas
            var canvas = new Canvas(board.Size * 2, board.Size);

            // Dibujar el tablero en el canvas
            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    var cell = board.Grid[i, j];
                    var color = GetCellColor(cell);

                    // Dibujamos cada celda como un bloque de 2x1 para mantener el aspecto cuadrado
                    canvas.SetPixel(j * 2, i, color);
                    canvas.SetPixel(j * 2 + 1, i, color);
                }
            }

            // Limpiamos la consola y mostramos el canvas
            Console.Clear();
            AnsiConsole.Write(canvas);
        }

        private static Spectre.Console.Color GetCellColor(string cell)
        {
            return cell switch
            {
                "■" => Spectre.Console.Color.White,
                "T" => Spectre.Console.Color.Red,
                "G" => Spectre.Console.Color.Gold1,
                "S" => Spectre.Console.Color.Blue,
                "A" => Spectre.Console.Color.Yellow,
                "R" => Spectre.Console.Color.Green,
                "L" => Spectre.Console.Color.Aqua,
                "B" => Spectre.Console.Color.Red,
                _ => Spectre.Console.Color.Black
            };
        }

        private static string GetCellStyle(string cell)
        {
            return cell switch
            {
                "■" => "■",  // Obstáculos en blanco
                "T" => "[red]■[/]",  // Trampas en rojo
                "G" => "[gold1]■[/]",  // Grial en dorado
                "S" => "[grey]■[/]",  // Saber en azul
                "A" => "[yellow]■[/]",  // Archer en amarillo
                "R" => "[green]■[/]",  // Rider en verde
                "L" => "[aqua]■[/]",  // Lancer en aqua
                "B" => "[red]■[/]",  // Berserker en rojo
                _ => "  "  // Espacio vacío (dos espacios para mantener el aspecto cuadrado)
            };
        }

        private static (string topLeft, string topRight, string bottomLeft, string bottomRight) GetPixelStyle(string cell)
        {
            // Ya no necesitamos este método, pero lo mantenemos por compatibilidad
            // y lo redirigimos al nuevo sistema
            string style = GetCellStyle(cell);
            return (style, style, style, style);
        }

        public static Character SelectCharacter(List<Character> characters)
        {
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]Selecciona tu Servant:[/]")
                    .PageSize(10)
                    .AddChoices(characters.Select(c => $"{c.Name} ({c.Type}) - {c.AbilityDescription}")));

            int index = characters.FindIndex(c =>
                selection.StartsWith($"{c.Name} ({c.Type})"));

            var selectedCharacter = characters[index];
            characters.RemoveAt(index);

            AnsiConsole.MarkupLine($"Has seleccionado a [gold1]{selectedCharacter.Name}[/]");
            return selectedCharacter;
        }

        public static void DisplayTurn(Character character)
        {
            var panel = new Panel($"Turno de {character.Name}")
            {
                Border = BoxBorder.Rounded,
                Padding = new Padding(1, 0, 1, 0),
            };
            panel.BorderStyle = ServantNameStyle;
            AnsiConsole.Write(panel);
        }

        public static void DisplayAbilityCooldown(Character character)
        {
            if (character.CurrentCooldown > 0)
            {
                AnsiConsole.MarkupLine($"[red]Habilidad en cooldown: {character.CurrentCooldown} turnos restantes[/]");
            }
        }

        public static void DisplayGameStatus(string message, Style? style = null)
        {
            style ??= new Style(foreground: Spectre.Console.Color.White);
            var panel = new Panel(message)
            {
                Border = BoxBorder.Rounded,
                Padding = new Padding(1, 0, 1, 0),
            };
            panel.BorderStyle = style;
            AnsiConsole.Write(panel);
        }

        public static char GetPlayerAction()
        {
            var choices = new[]
            {
                "W - Mover Arriba",
                "A - Mover Izquierda",
                "S - Mover Abajo",
                "D - Mover Derecha",
                "H - Usar Habilidad"
            };

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]Elige tu acción:[/]")
                    .PageSize(5)
                    .AddChoices(choices));

            return choice[0];
        }

        public static char GetAbilityDirection()
        {
            var choices = new[]
            {
                "W - Arriba",
                "A - Izquierda",
                "S - Abajo",
                "D - Derecha"
            };

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[gold1]Elige la dirección para tu habilidad:[/]")
                    .PageSize(4)
                    .AddChoices(choices));

            return choice[0];
        }

        public static bool AskToPlayAgain()
        {
            return AnsiConsole.Confirm("¿Deseas jugar otra partida?");
        }

        public static void ShowVictoryMessage(Character winner)
        {
            AnsiConsole.Clear();
            var font = FigletFont.Default;
            AnsiConsole.Write(
                new FigletText(font, "¡Victoria!")
                    .Centered()
                    .Color(Spectre.Console.Color.Gold1));

            var panel = new Panel($"{winner.Name} ha obtenido el Santo Grial")
            {
                Border = BoxBorder.Double,
                Padding = new Padding(2, 1, 2, 1),
            };
            panel.BorderStyle = new Style(foreground: Spectre.Console.Color.Gold1);
            AnsiConsole.Write(panel);
        }

        public static void DisplayTrapActivation(string trapName)
        {
            AnsiConsole.Write(new Spectre.Console.Rule($"[red]{trapName} Activada![/]").RuleStyle(Style.Parse("red")));
        }

        public static string GetPiAnswer()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Ingresa los primeros 6 decimales de Pi (puedes usar . o , para decimales):")
                    .ValidationErrorMessage("[red]Por favor ingresa un valor numérico válido[/]")
                    .Validate(answer =>
                    {
                        answer = answer.Replace(",", ".");
                        return answer.All(c => char.IsDigit(c) || c == '.');
                    }));
        }
    }
}