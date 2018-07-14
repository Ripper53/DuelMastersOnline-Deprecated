
namespace DuelMasters.Cards {
    public sealed class BurningPower : Spell {
        public override string OriginalName => "Burning Power";

        public BurningPower(Duelist owner) : base(owner) {
            ManaCost = 1;
            Add(Civilization.FIRE);
        }

        protected override void CastSpell() {
            Owner.TaskList.AddTask(Effect.PowerAttackUntilEndTask(this, 2000, 1, false));
        }

    }
}
