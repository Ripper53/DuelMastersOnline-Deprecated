
namespace DuelMasters.Cards {
    public sealed class PangaeasSong : Spell {
        public override string OriginalName => "Pangaea's Song";

        public PangaeasSong(Duelist owner) : base (owner) {
            ManaCost = 1;
            Add(Civilization.NATURE);
        }

        protected override void CastSpell() {
            Owner.TaskList.AddTask(Effect.BattleZoneToManaZoneFriendlyTask<Creature>(this, 1, false));
        }

    }
}
