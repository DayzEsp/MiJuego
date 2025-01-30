using FateMazeGame;

public class EnkiduChainTrap : TrapBase
{
    public override void Activate(Character character, Board board)
    {
        // Inmovilizar al personaje por 2 turnos
        character.Immobilize(2);
        GameUI.DisplayTrapActivation("Trampa de Cadenas Enkidu");
    }
}
