
namespace DuelMasters.Cards {
    public sealed class SonicWing : Spell {
        public override string OriginalName => "Sonic Wing";

        public SonicWing(Duelist owner) : base(owner) {
            ManaCost = 3;
            Add(Civilization.LIGHT);
        }

        protected override void CastSpell() {
            if (Owner.BattleZone.GetAll<Creature>().Length > 0)
                Owner.TaskList.AddTask(Effect.CannotBeBlockedTask(this, 1, false));
        }

    }
}
