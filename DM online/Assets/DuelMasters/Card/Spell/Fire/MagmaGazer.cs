
namespace DuelMasters.Cards {
    public sealed class MagmaGazer : Spell {
        public override string OriginalName => "Magma Gazer";

        public MagmaGazer(Duelist owner) : base(owner) {
            ManaCost = 3;
            Add(Civilization.FIRE);
        }

        protected override void CastSpell() {
            Owner.TaskList.AddTask(Effect.GroupEffect(
                this,
                false,
                () => Owner.BattleZone.GetAll<Creature>(),
                () => Effect.PowerAttackUntilEndTask(this, 4000, 1, false),
                () => Effect.ShieldBreakerUntilEndTask(this, 2, 1, false)
            ));
        }

    }
}
