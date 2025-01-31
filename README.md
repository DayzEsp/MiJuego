Un juego hecho con mucho amor sksks , con tematica Fate , disfruten de lo poco que hay . 

El proyecto está organizado en varios componentes principales:
* Board.cs: La clase principal que maneja el tablero de juego
* Character.cs: Define los personajes (Servants) y sus características
* Program.cs: El punto de entrada del juego
* Clases de trampas: CurseOfCommandSealTrap.cs, EnkiduChainTrap.cs, IllusorySummonTrap.cs
* Interface IAbility.cs: Define el contrato para las habilidades de los personajes
1. Flujo del Juego:

Inicio -> Menú Principal -> Selección de Personajes -> Inicialización del Tablero -> Bucle de Juego -> Victoria/Derrota -> Opción de Jugar de Nuevo

1. Componentes Principales:
a) Clase Board:
* Maneja una cuadrícula de 21x21 (definido en Program.cs)
* Contiene la lógica para:
   * Generación del tablero con obstáculos aleatorios
   * Colocación de personajes
   * Colocación del Grial
   * Sistema de trampas
   * Validación de movimientos
   * Verificación de victoria
b) Sistema de Personajes:
* Implementa 5 tipos de Servants:
   * Saber (Arthur)
   * Archer (Gilgamesh)
   * Rider (Medusa)
   * Lancer (Cu Chulainn)
   * Berserker (Hercules)
* Cada personaje tiene:
   * Nombre
   * Tipo
   * Símbolo en el tablero
   * Habilidad especial
   * Sistema de cooldown
   * Velocidad
   * Estado de inmovilización
1. Algoritmos Importantes:
a) Generación del Tablero Accesible:

private bool IsAccessible() { // Usa BFS (Breadth-First Search) para verificar que todos los espacios // vacíos sean alcanzables bool[,] visited = new bool[size, size]; Queue<(int x, int y)> queue = new Queue<(int x, int y)>(); // ... }
b) Sistema de Trampas:

* Tres tipos de trampas:
   1. IllusorySummonTrap: Pide responder π, reconfigura el laberinto si falla
   2. CurseOfCommandSealTrap: Bloquea habilidades por 5 turnos
   3. EnkiduChainTrap: Inmoviliza al personaje por 2 turnos

1. Características de Diseño Notables:
a) Patrones de Diseño:
* Strategy Pattern: Para las habilidades de los personajes (IAbility)
* Template Method: En la implementación de trampas (TrapBase)
* Factory Method: Para la creación de personajes
b) Manejo de Estado:
* Sistema de cooldown para habilidades
* Estado de inmovilización para personajes
* Gestión de posiciones en el tablero
1. Sistema de Movimiento:

public void MoveCharacter(Character character, int newX, int newY) { // Validación de movimiento // Actualización de posición // Activación de trampas si las hay // Actualización visual }

1. Sistemas de Validación:
* Verificación de movimientos válidos
* Comprobación de victoria
* Validación de uso de habilidades
* Verificación de accesibilidad del tablero
1. Aspectos Técnicos Destacables:

a) Manejo de Memoria:
* Uso de estructuras de datos eficientes (Dictionary para trampas)
* Reutilización de objetos en lugar de crear nuevos
b) Organización del Código:
* Separación clara de responsabilidades
* Encapsulación adecuada
* Uso de propiedades auto-implementadas
* Manejo de estados mediante enumeraciones


Sistema de Habilidades (Abilities): Todas las habilidades implementan la interfaz IAbility con diferentes estrategias:
a) DivergingShotAbility (Flecha de Emiya):

Rango de 3 casillas
Puede activar trampas a distancia
Limpia el camino eliminando la trampa
Verifica línea recta en la dirección elegida
csharp

b) SprintAbility (Velocidad de Rider):

Permite moverse 2 casillas de una vez
Verifica que el camino esté despejado
No puede atravesar obstáculos
Requiere espacio libre en la casilla intermedia y final
c) StrategicLeapAbility (Salto de Shirou Emiya):

Permite saltar sobre obstáculos
Aterriza 2 casillas más allá
Requiere un obstáculo en medio
La casilla de aterrizaje debe estar vacía
d) AgileSpinAbility (Giro de Lancer):

Movimiento a casilla adyacente
Desactiva automáticamente trampas
Combina movimiento y desactivación
No funciona con obstáculos
e) ChargeAbility (Arremetida de Berserker):

Destruye obstáculos adyacentes
Se mueve a la posición del obstáculo
Solo funciona con obstáculos (símbolo "■")
No funciona con trampas o espacios vacíos
Interfaz Gráfica (GameUI): La clase GameUI implementa una interfaz rica usando Spectre.Console con:
a) Estilos Predefinidos:

b) Componentes Visuales:

Menú Principal con ASCII Art
Tablero colorido usando Canvas
Paneles informativos con bordes
Mensajes de estado estilizados
Sistema de selección interactivo
c) Características de la UI:

Soporte para colores ANSI
Menú de navegación con flechas
Prompts interactivos para acciones
Mensajes de estado formatados
Visualización de cooldowns
Mensajes de victoria con efectos
d) Métodos de Interacción:

GetPlayerAction(): Selección de movimiento
GetAbilityDirection(): Dirección de habilidad
SelectCharacter(): Elección de personaje
AskToPlayAgain(): Prompt de nueva partida
e) Sistema de Coloración:

csharp

Copy
private static Color GetCellColor(string cell) {
    return cell switch {
        "■" => Color.White,    // Obstáculos
        "T" => Color.Red,      // Trampas
        "G" => Color.Gold1,    // Grial
        // ... otros casos
    };
}
Integración de Sistemas:
a) Flujo de Habilidades:

Usuario selecciona usar habilidad (H)
Selecciona dirección (WASD)
Se valida la habilidad y cooldown
Se ejecuta la habilidad específica
Se actualiza el estado del tablero
b) Manejo de Estados:

Actualización de posiciones
Control de cooldowns
Estados de inmovilización
Verificación de victoria
Gestión de trampas activas
