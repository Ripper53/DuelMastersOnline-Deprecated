
namespace DuelMasters.Cards {
    public sealed class SuperExplosiveVolcanodon : Creature {
        public override string OriginalName => "Super Explosive Volcanodon";

        public SuperExplosiveVolcanodon(Duelist owner) : base(owner) {
            ManaCost = 4;
            Power = 2000;
            Add(Civilization.FIRE);
            Add(Race.DRAGONOID);

            Effect.PowerAttacker(this, 4000);
        }

    }
}
