
namespace DuelMasters.Cards {
    public sealed class TowerShell : Creature {
        public override string OriginalName => "Tower Shell";

        public TowerShell(Duelist owner) : base(owner) {
            ManaCost = 6;
            Power = 5000;
            Add(Civilization.NATURE);
            Add(Race.COLONY_BEETLE);

            Blockable = (blocker) => blocker.Power > 4000;
        }

    }
}
