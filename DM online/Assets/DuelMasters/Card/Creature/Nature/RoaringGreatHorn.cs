
namespace DuelMasters.Cards {
    public sealed class RoaringGreatHorn : Creature {
        public override string OriginalName => "Roaring Great-Horn";

        public RoaringGreatHorn(Duelist owner) : base(owner) {
            ManaCost = 7;
            ShieldBreaker = 2;
            Power = 8000;
            Add(Civilization.NATURE);
            Add(Race.HORNED_BEAST);

            Effect.PowerAttacker(this, 2000);
        }

    }
}
