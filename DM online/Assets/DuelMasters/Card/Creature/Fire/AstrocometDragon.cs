
namespace DuelMasters.Cards {
    public sealed class AstrocometDragon : Creature {
        public override string OriginalName => "Astrocomet Dragon";

        public AstrocometDragon(Duelist owner) : base(owner) {
            ManaCost = 7;
            ShieldBreaker = 2;
            Power = 6000;
            Add(Civilization.FIRE);
            Add(Race.ARMORED_DRAGON);

            Effect.PowerAttacker(this, 4000);
        }

    }
}
