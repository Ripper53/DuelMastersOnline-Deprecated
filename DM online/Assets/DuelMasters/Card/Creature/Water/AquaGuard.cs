
namespace DuelMasters.Cards {
    public sealed class AquaGuard : Creature {
        public override string OriginalName => "Aqua Guard";

        public AquaGuard(Duelist owner) : base(owner) {
            ManaCost = 1;
            Power = 2000;
            Add(Civilization.WATER);
            Add(Race.LIQUID_PEOPLE);

            // Sets THIS Creature as a Blocker.
            Blocker = true;
            // This Creature cannot attack at all.
            CanAttack = (creature) => false;
        }

    }
}
