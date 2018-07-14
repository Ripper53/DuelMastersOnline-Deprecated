
namespace DuelMasters.Cards {
    public sealed class BrawlerZyler : Creature {
        public override string OriginalName => "Brawler Zyler";

        public BrawlerZyler(Duelist owner) : base(owner) {
            ManaCost = 2;
            Power = 1000;
            Add(Civilization.FIRE);
            Add(Race.HUMAN);

            Effect.PowerAttacker(this, 2000);
        }

    }
}
