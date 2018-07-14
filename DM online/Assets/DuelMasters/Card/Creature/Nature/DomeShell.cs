
namespace DuelMasters.Cards {
    public sealed class DomeShell : Creature {
        public override string OriginalName => "Dome Shell";

        public DomeShell(Duelist owner) : base(owner) {
            ManaCost = 4;
            Power = 3000;
            Add(Civilization.NATURE);
            Add(Race.COLONY_BEETLE);

            Effect.PowerAttacker(this, 2000);
        }

    }
}
