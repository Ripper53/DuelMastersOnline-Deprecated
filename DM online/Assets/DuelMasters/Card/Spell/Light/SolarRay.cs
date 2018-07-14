
namespace DuelMasters.Cards {
    public sealed class SolarRay : Spell {
        public override string OriginalName => "Solar Ray";

        public SolarRay(Duelist owner) : base(owner) {
            ManaCost = 2;
            Add(Civilization.LIGHT);

            ShieldTrigger = true;
        }

        protected override void CastSpell() {
            if (Owner.Game.GetAllInBattleZone<Creature>(Owner.BattleZone).Length > 0)
                Owner.TaskList.AddTask(Effect.TapOppTask(this, 1, false));
        }

    }
}
