
namespace DuelMasters.Cards {
    public sealed class DimensionGate : Spell {
        public override string OriginalName => "Dimension Gate";

        public DimensionGate(Duelist owner) : base(owner) {
            ManaCost = 3;
            Add(Civilization.NATURE);

            ShieldTrigger = true;
        }

        protected override void CastSpell() {
            Owner.TaskList.AddTask(Effect.SearchDeckForTask<Creature>(this, 1, false));
        }

    }
}
