
namespace DuelMasters.Cards {
    public sealed class UltimateForce : Spell {
        public override string OriginalName => "Ultimate Force";

        public UltimateForce(Duelist owner) : base(owner) {
            ManaCost = 5;
            Add(Civilization.NATURE);
        }

        protected override void CastSpell() {
            Owner.TaskList.AddTask(Effect.TopCardOfDeckIntoManaZoneTask(this, 2, false));
        }

    }
}
