using System;

namespace FateMazeGame
{
    public enum CharacterType
    {
        Saber,
        Archer,
        Rider,
        Lancer,
        Berserker
    }

    public class Character
    {
        public string Name { get; set; }
        public CharacterType Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Symbol { get; set; }
        public IAbility Ability { get; set; }
        public int Cooldown { get; set; }
        public int Speed { get; set; }
        public int CurrentCooldown { get; set; }
        public string AbilityDescription { get; set; }
        public int ImmobilizedTurns { get; set; }

        public Character(string name, CharacterType type, string symbol, IAbility ability, int speed, int cooldown, string abilityDescription)
        {
            Name = name;
            Type = type;
            Symbol = symbol;
            Ability = ability;
            Speed = speed;
            Cooldown = cooldown;
            AbilityDescription = abilityDescription;
            ImmobilizedTurns = 0;
        }

        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool CanUseAbility()
        {
            return CurrentCooldown == 0;
        }

        public void UseAbility(Board board, char direction)
        {
            Ability.Execute(this, board, direction);
            CurrentCooldown = Cooldown;
        }

        public void ReduceCooldown()
        {
            if (CurrentCooldown > 0)
            {
                CurrentCooldown--;
            }
        }

        public void Immobilize(int turns)
        {
            ImmobilizedTurns = turns;
        }

        public bool IsImmobilized()
        {
            return ImmobilizedTurns > 0;
        }

        public void ReduceImmobilization()
        {
            if (ImmobilizedTurns > 0)
            {
                ImmobilizedTurns--;
            }
        }
    }

}
