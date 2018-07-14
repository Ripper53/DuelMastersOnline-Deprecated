
namespace DuelMasters.Cards {
    public sealed class RevolverFish : Creature {
        public override string OriginalName => "Revolver Fish";

        public RevolverFish(Duelist owner) : base(owner) {
            ManaCost = 4;
            Power = 5000;
            Add(Civilization.WATER);
            Add(Race.GEL_FISH);

            Blocker = true;
            CanAttack = (creature) => false;
        }

    }
}
