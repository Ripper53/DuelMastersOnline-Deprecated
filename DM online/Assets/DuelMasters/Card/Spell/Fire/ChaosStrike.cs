
namespace DuelMasters.Cards {
    public sealed class ChaosStrike : Spell {
        public override string OriginalName => "Chaos Strike";

        public ChaosStrike(Duelist owner) : base(owner) {
            ManaCost = 2;
            Add(Civilization.FIRE);
        }

        protected override void CastSpell() {
            Owner.TaskList.AddTask(Effect.AttackUntappedCreature(this, 1, false));
        }

    }
}
