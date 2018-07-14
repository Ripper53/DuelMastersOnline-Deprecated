
namespace DuelMasters.Cards {
    public sealed class SteelSmasher : Creature {
        public override string OriginalName => "Steel Smasher";

        public SteelSmasher(Duelist owner) : base(owner) {
            ManaCost = 2;
            Power = 3000;
            Add(Civilization.NATURE);
            Add(Race.BEAST_FOLK);

            CanAttackPlayer = (player) => false;
        }

    }
}
