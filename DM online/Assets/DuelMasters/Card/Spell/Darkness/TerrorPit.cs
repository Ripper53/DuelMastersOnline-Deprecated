
namespace DuelMasters.Cards {
    public sealed class TerrorPit : Spell {
        public override string OriginalName => "Terror Pit";

        public TerrorPit(Duelist owner) : base(owner) {
            ManaCost = 6;
            Add(Civilization.DARKNESS);

            ShieldTrigger = true;
        }

        protected override void CastSpell() {
            Owner.TaskList.AddTask(Effect.DestroyOppTask(this, 1, false));
        }

    }
}
