using System.Collections.Generic;

namespace DuelMasters {
    public class DuelAction {
        public Duelist Owner { get; private set; }
        public List<Card> usableCards;
        public void AddUsable(Card card) {
            usableCards.Add(card);
        }
        public void RemoveUsable(Card card) {
            usableCards.Remove(card);
        }
        public bool ContainsUsable(Card card) {
            return usableCards.Contains(card);
        }

        public DuelAction(Duelist owner) {
            Owner = owner;
            usableCards = new List<Card>();
        }
        public bool ActionCheck(Card card, Game.Step step) {
            Game game = Owner.Game;
            return game.CurrentDuelistTurn == Owner && game.GameStep == step && card.CurrentZone is Hand;
        }

        public bool ChargeCard(Card card) {
            if (ActionCheck(card, Game.Step.CHARGE) && Owner.Charge > 0) {
                Owner.Charge -= 1;
                Owner.ManaZone.Put(card);
                return true;
            }
            return false;
        }

        public void UseCard(Card card, Card[] mana) {
            if (ActionCheck(card, Game.Step.MAIN) && ContainsUsable(card) && ExactMana(card, mana)) {
                Card.TapAll(mana);
                card.Use();
            }
        }

        public bool ExactMana(Card cardToUse, Card[] manaToUse) {
            int manaCount = 0;
            foreach (Card mana in manaToUse) {
                if (mana.Tapped) return false;
                manaCount += mana.ManaNumber;
            }
            if (manaCount == cardToUse.ManaCost) {
                foreach (Card mana in manaToUse) {
                    if (cardToUse.Contains(mana.Civilizations)) {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
