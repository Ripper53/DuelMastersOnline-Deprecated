
namespace DuelMasters.Cards {
    public sealed class HolyAwe : Spell {
        public override string OriginalName => "Holy Awe";

        public HolyAwe(Duelist owner) : base(owner) {
            ManaCost = 6;
            Add(Civilization.LIGHT);
        }

        protected override void CastSpell() {
            Effect.TapAllOpp(this);
        }

    }
}
