
namespace DuelMasters.Cards {
    public sealed class LaserWing : Spell {
        public override string OriginalName => "Laser Wing";

        public LaserWing(Duelist owner) : base(owner) {
            ManaCost = 5;
            Add(Civilization.LIGHT);
        }

        protected override void CastSpell() {
            if (Owner.BattleZone.GetAll<Creature>().Length > 0)
                Owner.TaskList.AddTask(Effect.CannotBeBlockedTask(this, 2, false));
        }

    }
}
