
namespace DuelMasters.Cards {
    public sealed class PhantomFish : Creature {
        public override string OriginalName => "Phantom Fish";

        public PhantomFish(Duelist owner) : base(owner) {
            ManaCost = 3;
            Power = 4000;
            Add(Civilization.WATER);
            Add(Race.GEL_FISH);

            Blocker = true;
            CanAttack = (creature) => false;
        }

    }
}
