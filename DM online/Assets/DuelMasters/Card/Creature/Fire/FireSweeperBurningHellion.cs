
namespace DuelMasters.Cards {
    public sealed class FireSweeperBurningHellion : Creature {
        public override string OriginalName => "Fire Sweeper Burning Hellion";

        public FireSweeperBurningHellion(Duelist owner) : base(owner) {
            ManaCost = 4;
            Power = 3000;
            Add(Civilization.FIRE);
            Add(Race.DRAGONOID);

            Effect.PowerAttacker(this, 2000);
        }

    }
}
