
namespace DuelMasters.Cards {
    public sealed class SpiralGate : Spell {
        public override string OriginalName => "Spiral Gate";

        public SpiralGate(Duelist owner) : base(owner) {
            ManaCost = 2;
            Add(Civilization.WATER);

            ShieldTrigger = true;
        }

        protected override void CastSpell() {
            if (Owner.Game.GetAllInBattleZone<Creature>().Length > 0)
                Owner.TaskList.AddTask(Effect.BounceTask<Creature>(this, 1, false));
        }

    }
}
