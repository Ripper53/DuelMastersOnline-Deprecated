
namespace DuelMasters {
    /// <summary>
    /// <para>
    /// Default:
    /// Mana Number = 1.
    /// </para>
    /// <para>
    /// Set:
    /// Name,
    /// Mana Cost,
    /// Civilization.
    /// </para>
    /// </summary>
    public abstract class Spell : Card {
        private SpellData SpellData { get; set; }
        protected override CardData CardData => SpellData;

        public Spell(Duelist owner) : base(owner) {
            SpellData = new SpellData() {
                Name = OriginalName,
                ManaNumber = 1
            };
        }

        public void Cast() {
            CurrentZone.Remove(this);
            CastSpell();
            Owner.Graveyard.Put(this);
            OnCast();
        }

        /// <summary>
        /// Spell effects go here!
        /// </summary>
        protected abstract void CastSpell();

        public sealed override void Use() {
            Cast();
        }

        public delegate void SpellEventHandler(Spell source);
        public event SpellEventHandler Casted;
        protected virtual void OnCast() {
            Casted?.Invoke(this);
        }

    }
}
