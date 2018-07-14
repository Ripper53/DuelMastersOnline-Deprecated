
namespace DuelMasters.Cards {
    public sealed class BurningMane : Creature {
        public override string OriginalName => "Burning Mane";

        public BurningMane(Duelist owner) : base(owner) {
            ManaCost = 2;
            Power = 2000;
            Add(Civilization.NATURE);
            Add(Race.BEAST_FOLK);
        }

    }
}
