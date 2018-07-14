
namespace DuelMasters.Cards {
    public sealed class GoldenWingStriker : Creature {
        public override string OriginalName => "Golden Wing Striker";

        public GoldenWingStriker(Duelist owner) : base(owner) {
            ManaCost = 3;
            Power = 2000;
            Add(Civilization.NATURE);
            Add(Race.BEAST_FOLK);

            Effect.PowerAttacker(this, 2000);
        }

    }
}
