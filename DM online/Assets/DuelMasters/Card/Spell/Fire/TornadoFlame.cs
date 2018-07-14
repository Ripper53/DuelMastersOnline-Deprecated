
namespace DuelMasters.Cards {
    public sealed class TornadoFlame : Spell {
        public override string OriginalName => "Tornado Flame";

        public TornadoFlame(Duelist owner) : base(owner) {
            ManaCost = 5;
            Add(Civilization.FIRE);

            ShieldTrigger = true;
        }

        protected override void CastSpell() {
            Owner.TaskList.AddTask(Effect.DestroyPowerLessOrEqualOppTask(this, 4000, 1, false));
        }

    }
}
