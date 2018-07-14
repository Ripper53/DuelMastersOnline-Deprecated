
namespace DuelMasters.Cards {
    public sealed class GhostTouch : Spell {
        public override string OriginalName => "Ghost Touch";

        public GhostTouch(Duelist owner) : base(owner) {
            ManaCost = 2;
            Add(Civilization.DARKNESS);

            ShieldTrigger = true;
        }

        protected override void CastSpell() {
            Effect.OppDiscardRandom(this);
        }

    }
}
