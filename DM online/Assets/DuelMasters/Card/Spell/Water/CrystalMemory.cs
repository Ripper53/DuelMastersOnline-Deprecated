
namespace DuelMasters.Cards {
    public sealed class CrystalMemory : Spell {
        public override string OriginalName => "Crystal Memory";

        public CrystalMemory(Duelist owner) : base(owner) {
            ManaCost = 4;
            Add(Civilization.WATER);
        }

        protected override void CastSpell() {
            if (Owner.Deck.NumberOfCards > 0)
                Owner.TaskList.AddTask(Effect.SearchDeckForTask<Card>(this, 1, false));
        }

    }
}
