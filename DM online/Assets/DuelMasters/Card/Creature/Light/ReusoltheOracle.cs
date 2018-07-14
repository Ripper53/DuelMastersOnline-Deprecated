
namespace DuelMasters.Cards {
    public sealed class ReusoltheOracle : Creature {
        public override string OriginalName => "Reusol, the Oracle";

        public ReusoltheOracle(Duelist owner) : base(owner) {
            ManaCost = 2;
            Power = 2000;
            Add(Civilization.LIGHT);
            Add(Race.LIGHT_BRINGER);
        }

    }
}
