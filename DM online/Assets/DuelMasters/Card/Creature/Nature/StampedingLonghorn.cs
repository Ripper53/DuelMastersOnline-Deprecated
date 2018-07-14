
namespace DuelMasters.Cards {
    public sealed class StampedingLonghorn : Creature {
        public override string OriginalName => "Stampeding Longhorn";

        public StampedingLonghorn(Duelist owner) : base(owner) {
            ManaCost = 5;
            Power = 4000;
            Add(Civilization.NATURE);
            Add(Race.HORNED_BEAST);

            // Cannot be blocked by any Creature that has power 3000 or less.
            Blockable = (blocker) => blocker.Power > 3000;
        }

    }
}
