using System.Collections.Generic;

namespace DuelMasters {
    public abstract class Card {
        public abstract string OriginalName { get; }
        public int ID { get; private set; }

        protected abstract CardData CardData { get; }
        public string Name {
            get {
                return CardData.Name;
            }
            set {
                CardData.Name = value;
            }
        }
        public int ManaCost {
            get {
                return CardData.ManaCost;
            }
            set {
                CardData.ManaCost = value;
            }
        }
        public int ManaNumber {
            get {
                return CardData.ManaNumber;
            }
            set {
                CardData.ManaNumber = value;
            }
        }
        public IEnumerable<Civilization> Civilizations => CardData.Civilizations;

        public Civilization[] GetCivilizations() {
            return CardData.Civilizations.ToArray();
        }

        public Duelist Owner { get; private set; }
        /// <summary>
        /// ONLY set through Zone object!
        /// Do NOT set it manually.
        /// </summary>
        public Zone CurrentZone { get; set; }

        private bool _tapped;
        public bool Tapped {
            get {
                return _tapped;
            }
            set {
                _tapped = value;
                OnTap(Tapped);
            }
        }

        public bool ShieldTrigger { get; set; }

        public Card(Duelist owner) {
            Owner = owner;
            ID = Owner.Game.GetID();
            Owner.DuelAction.AddUsable(this);
            Tapped = false;
            ShieldTrigger = false;
        }

        // Use card (ex. summon creature, cast spell, etc...).
        public abstract void Use();

        public void Add(Civilization civ) {
            if (!Contains(civ)) {
                CardData.Civilizations.Add(civ);
            }
        }
        public void Remove(Civilization civ) {
            CardData.Civilizations.Remove(civ);
        }
        public bool Contains(Civilization civ) {
            return CardData.Civilizations.Contains(civ);
        }
        public bool Contains(IEnumerable<Civilization> civs) {
            foreach (Civilization civ in civs) {
                if (Contains(civ)) {
                    return true;
                }
            }
            return false;
        }

        public delegate void CardTapEventHandler(Card source, bool tapped);
        public event CardTapEventHandler Tap;
        protected virtual void OnTap(bool tapped) {
            Tap?.Invoke(this, tapped);
        }

        public static void TapAll(IEnumerable<Card> cards, bool tap = true) {
            foreach (Card card in cards) {
                card.Tapped = tap;
            }
        }

    }
}
