namespace FateMazeGame
{
    public interface IAbility
    {
        void Execute(Character character, Board board, char direction);
    }
}
