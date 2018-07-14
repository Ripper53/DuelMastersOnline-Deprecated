
namespace DuelMasters.Cards {
    public sealed class NaturalSnare : Spell {
        public override string OriginalName => "Natural Snare";

        public NaturalSnare(Duelist owner) : base(owner) {
            ManaCost = 6;
            Add(Civilization.NATURE);

            ShieldTrigger = true;
        }

        protected override void CastSpell() {
            Owner.TaskList.AddTask(Effect.BattleZoneToManaZoneOppTask<Creature>(this, 1, false));
        }

    }
}
