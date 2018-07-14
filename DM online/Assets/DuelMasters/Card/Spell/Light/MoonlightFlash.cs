
namespace DuelMasters.Cards {
    public sealed class MoonlightFlash : Spell {
        public override string OriginalName => "Moonlight Flash";

        public MoonlightFlash(Duelist owner) : base(owner) {
            ManaCost = 4;
            Add(Civilization.LIGHT);
        }

        protected override void CastSpell() {
            if (Owner.Game.GetAllInBattleZone<Creature>(Owner.BattleZone).Length > 0)
                Owner.TaskList.AddTask(Effect.TapOppTask(this, 2, false));
        }

    }
}
