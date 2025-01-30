using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FateMazeGame
{
    public class Board
    {
        private int size;
        private string[,] grid;
        private Random random;
        private List<Character> characters;
        private Character player1Character;
        private Character player2Character;
        private Dictionary<(int, int), TrapBase> traps;
        private int illusoryTrapCount;
        private int commandSealTrapCount;
        private int chainTrapCount;
        private int grialX;
        private int grialY;

        public int Size => size;
        public string[,] Grid => grid;
        public List<Character> Characters => characters;
        public Character Player1Character => player1Character;
        public Character Player2Character => player2Character;
        public Dictionary<(int, int), TrapBase> Traps => traps;
        public Board(int size)
        {
            this.size = size;
            grid = new string[size, size];
            random = new Random();
            traps = new Dictionary<(int, int), TrapBase>();
            grialX = size / 2;
            grialY = size / 2;
            characters = new List<Character>(); // Inicializar la lista de personajes
            player1Character = null!; // Inicializar player1Character
            player2Character = null!; // Inicializar player2Character
            InitializeBoard();
            AddBorders();
            GenerateAccessibleBoard();
            InitializeCharacters();
            SelectCharacters();
            PlaceCharacters();
            PlaceGrial();
            PlaceTraps();
            StartWar();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    grid[i, j] = " ";
                }
            }
        }

        private void AddBorders()
        {
            for (int i = 0; i < size; i++)
            {
                grid[0, i] = "■";
                grid[size - 1, i] = "■";
                grid[i, 0] = "■";
                grid[i, size - 1] = "■";
            }
        }

        private void AddObstacles()
        {
            int numberOfObstacles = (size * size) / 5;
            for (int i = 0; i < numberOfObstacles; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(1, size - 1);
                    y = random.Next(1, size - 1);
                } while (grid[x, y] != " ");
                grid[x, y] = "■";
            }
        }

        private bool IsAccessible()
        {
            bool[,] visited = new bool[size, size];
            Queue<(int x, int y)> queue = new Queue<(int x, int y)>();
            int emptySpaces = 0;
            int visitedSpaces = 0;

            // Encontrar el punto de inicio y contar espacios vacíos
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (grid[i, j] == " ")
                    {
                        emptySpaces++;
                        if (queue.Count == 0)
                        {
                            queue.Enqueue((i, j));
                            visited[i, j] = true;
                            visitedSpaces++;
                        }
                    }
                }
            }

            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };

            while (queue.Count > 0)
            {
                var (x, y) = queue.Dequeue();

                for (int i = 0; i < 4; i++)
                {
                    int newX = x + dx[i];
                    int newY = y + dy[i];

                    if (IsValid(newX, newY) && !visited[newX, newY] && grid[newX, newY] == " ")
                    {
                        queue.Enqueue((newX, newY));
                        visited[newX, newY] = true;
                        visitedSpaces++;
                    }
                }
            }

            return visitedSpaces == emptySpaces;
        }

        public bool IsValid(int row, int col)
        {
            return (row >= 0 && row < size && col >= 0 && col < size);
        }

        private void GenerateAccessibleBoard()
        {
            bool accessible;
            do
            {
                InitializeBoard();
                AddBorders();
                AddObstacles();
                accessible = IsAccessible();
            } while (!accessible);
        }

        private void InitializeCharacters()
        {
            characters = new List<Character>
            {
                new Character("Arthur", CharacterType.Saber, "S", new StrategicLeapAbility(), 3, 2, "Salto de Shirou Emiya"),
                new Character("Gilgamesh", CharacterType.Archer, "A", new DivergingShotAbility(), 3, 2, "Flecha de Emiya"),
                new Character("Medusa", CharacterType.Rider, "R", new SprintAbility(), 3, 2, "Velocidad de Rider"),
                new Character("Cu Chulainn", CharacterType.Lancer, "L", new AgileSpinAbility(), 3, 2, "Giro de Lancer"),
                new Character("Hercules", CharacterType.Berserker, "B", new ChargeAbility(), 3, 2, "Arremetida de Berserker")
            };
        }

        private void SelectCharacters()
        {
            Console.WriteLine("Master 1, selecciona tu Servant:");
            player1Character = SelectCharacter();

            Console.WriteLine("Master 2, selecciona tu Servant:");
            player2Character = SelectCharacter();
        }

        private Character SelectCharacter()
        {
            return GameUI.SelectCharacter(characters);
        }

        private void PlaceCharacters()
        {
            (int, int)[] positions = new (int, int)[]
            {
                (1, 1),
                (size - 2, size - 2),
                (1, size - 2),
                (size - 2, 1)
            };

            int index = random.Next(2) * 2;
            (int x1, int y1) = positions[index];
            player1Character.SetPosition(x1, y1);
            grid[x1, y1] = player1Character.Symbol;

            (int x2, int y2) = positions[index + 1];
            player2Character.SetPosition(x2, y2);
            grid[x2, y2] = player2Character.Symbol;
        }

        private void PlaceGrial()
        {
            grid[grialX, grialY] = "G";
        }

        private void PlaceTraps()
        {
            // Calcular la cantidad de trampas
            int totalTraps = (size * size) / 20;
            int trapCount = totalTraps / 3;
            illusoryTrapCount = trapCount;
            commandSealTrapCount = trapCount;
            chainTrapCount = trapCount;

            for (int i = 0; i < totalTraps; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(1, size - 1);
                    y = random.Next(1, size - 1);
                } while (grid[x, y] != " " || traps.ContainsKey((x, y)) || IsBlockingPath(x, y));

                TrapBase trap;
                if (illusoryTrapCount > 0)
                {
                    trap = new IllusorySummonTrap();
                    illusoryTrapCount--;
                }
                else if (commandSealTrapCount > 0)
                {
                    trap = new CurseOfCommandSealTrap();
                    commandSealTrapCount--;
                }
                else if (chainTrapCount > 0)
                {
                    trap = new EnkiduChainTrap();
                    chainTrapCount--;
                }
                else
                {
                    continue; // Si no quedan trampas disponibles, continuar
                }

                traps[(x, y)] = trap;
                grid[x, y] = "T";
            }
        }

        private bool IsBlockingPath(int x, int y)
        {
            string original = grid[x, y];
            grid[x, y] = "T";
            bool isBlocked = !IsAccessible();
            grid[x, y] = original;
            return isBlocked;
        }

        public void DisplayBoard()
        {
            Console.Clear(); // Aseguramos que la consola está limpia
            GameUI.DisplayBoard(this);
        }

        public void StartWar()
        {
            GameUI.DisplayTitle();
            Console.WriteLine("¡La Guerra por el Santo Grial ha comenzado!");

            while (true)
            {
                DisplayBoard();
                GameUI.DisplayTurn(player1Character);
                ProcessPlayerTurn(player1Character);
                if (CheckVictory(player1Character)) break;

                player1Character.ReduceCooldown();
                player1Character.ReduceImmobilization();

                DisplayBoard();
                GameUI.DisplayTurn(player2Character);
                ProcessPlayerTurn(player2Character);
                if (CheckVictory(player2Character)) break;

                player2Character.ReduceCooldown();
                player2Character.ReduceImmobilization();
            }
        }

        private void ProcessPlayerTurn(Character character)
        {
            if (character.IsImmobilized())
            {
                GameUI.DisplayGameStatus($"{character.Name} está inmovilizado por {character.ImmobilizedTurns} turno(s) más.");
                character.ReduceImmobilization();
                return;
            }

            GameUI.DisplayAbilityCooldown(character);
            char action = GameUI.GetPlayerAction();

            if (action == 'H')
            {
                if (character.CanUseAbility())
                {
                    char abilityDirection = GameUI.GetAbilityDirection();
                    if ("WASD".Contains(abilityDirection))
                    {
                        character.UseAbility(this, abilityDirection);
                    }
                    else
                    {
                        GameUI.DisplayGameStatus("Dirección inválida. La habilidad no se usó.");
                        ProcessPlayerTurn(character);
                    }
                }
                else
                {
                    GameUI.DisplayGameStatus($"La habilidad está en enfriamiento. Debes esperar {character.CurrentCooldown} turno(s) antes de usarla de nuevo.");
                    ProcessPlayerTurn(character);
                }
                return;
            }

            int newX = character.X;
            int newY = character.Y;

            switch (action)
            {
                case 'W': newX--; break;
                case 'A': newY--; break;
                case 'S': newX++; break;
                case 'D': newY++; break;
            }

            MoveCharacter(character, newX, newY);

        }

        public void MoveCharacter(Character character, int newX, int newY)
        {
            if (IsValid(newX, newY) && (grid[newX, newY] == " " || grid[newX, newY] == "T" || (newX == grialX && newY == grialY)))
            {
                grid[character.X, character.Y] = " ";
                character.SetPosition(newX, newY);
                grid[newX, newY] = character.Symbol;

                if (traps.ContainsKey((newX, newY)))
                {
                    traps[(newX, newY)].Activate(character, this);
                    traps.Remove((newX, newY));
                }

                AnsiConsole.Clear();
                DisplayBoard();
            }
            else
            {
                Console.WriteLine("Movimiento inválido. La posición de destino no es válida.");
            }
        }


        public (int, int) GetRandomStartPosition()
        {
            (int, int)[] startPositions = new (int, int)[]
            {
                (1, 1), // Esquina superior izquierda
                (size - 2, size - 2), // Esquina inferior derecha
                (1, size - 2), // Esquina superior derecha
                (size - 2, 1) // Esquina inferior izquierda
            };

            return startPositions[random.Next(4)];
        }

        public void Reconfigure()
        {
            Console.Clear(); // Limpiamos la consola antes de reconfigurar
            Console.WriteLine("Reconfigurando el laberinto...");

            // Guardamos las posiciones actuales de los personajes y el Grial
            var player1Pos = (player1Character.X, player1Character.Y);
            var player2Pos = (player2Character.X, player2Character.Y);
            (int grialPosX, int grialPosY) = (grialX, grialY);

            // Reinicializamos el tablero manteniendo solo los bordes
            InitializeBoard();
            AddBorders();

            // Generamos nuevo tablero asegurando accesibilidad
            bool accessible;
            do
            {
                InitializeBoard();
                AddBorders();
                AddObstacles();
                accessible = IsAccessible();
            } while (!accessible);

            // Restauramos las posiciones de los personajes
            grid[player1Pos.Item1, player1Pos.Item2] = player1Character.Symbol;
            grid[player2Pos.Item1, player2Pos.Item2] = player2Character.Symbol;
            player1Character.SetPosition(player1Pos.Item1, player1Pos.Item2);
            player2Character.SetPosition(player2Pos.Item1, player2Pos.Item2);

            // Restauramos el Grial
            grid[grialPosX, grialPosY] = "G";
            grialX = grialPosX;
            grialY = grialPosY;

            // Recolocamos las trampas
            PlaceTraps();

            DisplayBoard(); // Mostramos el tablero actualizado
        }


        public void RemoveTrap(int x, int y)
        {
            if (traps.ContainsKey((x, y)))
            {
                traps.Remove((x, y));
            }
        }

        public void DecreaseIllusoryTrapCount()
        {
            illusoryTrapCount--;
        }

        public bool CheckVictory(Character character)
        {
            if (character.X == grialX && character.Y == grialY)
            {
                GameUI.ShowVictoryMessage(character);
                AskToPlayAgain();
                return true;
            }
            return false;
        }

        private void AskToPlayAgain()
        {
            GameUI.AskToPlayAgain();
        }
    }
}