
namespace DuelMasters.Cards {
    public sealed class AuraBlast : Spell {
        public override string OriginalName => "Aura Blast";

        public AuraBlast(Duelist owner) : base(owner) {
            ManaCost = 4;
            Add(Civilization.NATURE);
        }

        protected override void CastSpell() {
            Effect.PowerAttackerUntilEnd(this, 2000, () => Owner.BattleZone.GetAll<Creature>());
        }

    }
}
