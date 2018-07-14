using System;
using System.Collections.Generic;

namespace DuelMasters {
    public class Zone {
        private static Random rng = new Random();
        public Duelist Owner { get; private set; }
        private List<Card> cards;
        public IEnumerable<Card> Cards => cards;
        public int NumberOfCards => cards.Count;

        public Card this[int index] => cards[index];

        public Zone(Duelist owner) {
            Owner = owner;
            cards = new List<Card>();
        }

        public void Put(Card card) {
            if (card.CurrentZone != null) {
                // If already in this Zone, return.
                if (card.CurrentZone == this) return;
                // If in another Zone, remove Card from that Zone.
                card.CurrentZone.Remove(card);
            }
            // Add Card to this Zone.
            cards.Add(card);
            card.CurrentZone = this;
            OnPutCard(card);
        }

        public void Remove(Card card) {
            cards.Remove(card);
            card.CurrentZone = null;
            card.Tapped = false;
            OnRemovedCard(card);
        }

        public T Search<T>(int id) where T : Card {
            foreach (T card in GetAll<T>()) {
                if (card.ID == id)
                    return card;
            }
            return null;
        }

        public void UntapAll() {
            foreach (Card card in Cards) {
                card.Tapped = false;
            }
        }

        public delegate void ZoneEventHandler(Zone source, Card card);
        public event ZoneEventHandler PutCard;
        protected virtual void OnPutCard(Card putCard) {
            PutCard?.Invoke(this, putCard);
        }
        public event ZoneEventHandler RemovedCard;
        protected virtual void OnRemovedCard(Card removedCard) {
            RemovedCard?.Invoke(this, removedCard);
        }

        public T[] GetAll<T>() where T : Card {
            List<T> allTypes = new List<T>();
            foreach (Card card in cards) {
                if (card is T) {
                    T cardType = (T)card;
                    allTypes.Add(cardType);
                }
            }
            return allTypes.ToArray();
        }

        public T[] GetAll<T>(Func<T, bool> predicate) where T : Card {
            List<T> allTypes = new List<T>();
            foreach (Card card in cards) {
                if (card is T) {
                    T cardType = (T)card;
                    if (predicate(cardType))
                        allTypes.Add(cardType);
                }
            }
            return allTypes.ToArray();
        }

        public bool Contains<T>(Civilization civ) where T : Card {
            foreach (T card in GetAll<T>()) {
                if (card.Contains(civ)) return true;
            }
            return false;
        }
        public int Count<T>(Civilization civ) where T : Card {
            int count = 0;
            foreach (T card in GetAll<T>()) {
                if (card.Contains(civ)) count += 1;
            }
            return count;
        }

        public bool Contains(Race race) {
            foreach (Creature creature in GetAll<Creature>()) {
                if (creature.Contains(race)) return true;
            }
            return false;
        }
        public int Count(Race race) {
            int count = 0;
            foreach (Creature creature in GetAll<Creature>()) {
                if (creature.Contains(race)) count += 1;
            }
            return count;
        }

        public void Shuffle() {
            for (var i = 0; i < cards.Count - 1; i++)
                Swap(i, rng.Next(i, cards.Count));
        }

        public void Swap(int i, int j) {
            Card temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
        }

        public string Print() {
            string zoneStr = "";
            foreach (Card card in cards) {
                zoneStr += card.Name + " -- ";
            }
            zoneStr = zoneStr.TrimEnd('-', ' ') + '.';
            return zoneStr;
        }

    }
}
