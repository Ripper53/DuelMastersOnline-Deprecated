
namespace DuelMasters.Cards {
    public sealed class VirtualTripwire : Spell {
        public override string OriginalName => "Virtual Tripwire";

        public VirtualTripwire(Duelist owner) : base(owner) {
            ManaCost = 3;
            Add(Civilization.WATER);
        }

        protected override void CastSpell() {
            Owner.TaskList.AddTask(Effect.TapOppTask(this, 1, false));
        }

    }
}
