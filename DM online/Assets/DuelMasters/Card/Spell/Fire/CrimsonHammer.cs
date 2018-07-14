
namespace DuelMasters.Cards {
    public sealed class CrimsonHammer : Spell {
        public override string OriginalName => "Crimson Hammer";

        public CrimsonHammer(Duelist owner) : base(owner) {
            ManaCost = 2;
            Add(Civilization.FIRE);
        }

        protected override void CastSpell() {
            Owner.TaskList.AddTask(Effect.DestroyPowerLessOrEqualOppTask(this, 2000, 1, false));
        }

    }
}
