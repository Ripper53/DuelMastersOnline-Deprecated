
namespace DuelMasters.Cards {
    public sealed class DeathSmoke : Spell {
        public override string OriginalName => "Death Smoke";

        public DeathSmoke(Duelist owner) : base(owner) {
            ManaCost = 4;
            Add(Civilization.DARKNESS);
        }

        protected override void CastSpell() {
            Owner.TaskList.AddTask(Effect.DestroyOppTappedCreatureTask(this, false, 1, false));
        }

    }
}
