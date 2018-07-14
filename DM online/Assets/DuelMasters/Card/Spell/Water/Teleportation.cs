
namespace DuelMasters.Cards {
    public sealed class Teleportation : Spell {
        public override string OriginalName => "Teleportation";

        public Teleportation(Duelist owner) : base(owner) {
            ManaCost = 5;
            Add(Civilization.WATER);
        }

        protected override void CastSpell() {
            Owner.TaskList.AddTask(Effect.BounceTask<Creature>(this, 2, false));
        }

    }
}
