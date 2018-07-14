using System;
using System.Threading;
using System.Threading.Tasks;

namespace DuelMasters {
    public class Duelist {
        public Game Game { get; private set; }
        public DuelAction DuelAction { get; private set; }

        public Card[] AllCards { get; private set; }

        public string Name { get; set; }

        public DuelTaskList TaskList { get; private set; }

        public int Charge { get; set; }

        public Zone Deck { get; private set; }
        public Hand Hand { get; private set; }
        public ShieldZone ShieldZone { get; private set; }
        public Zone ManaZone { get; private set; }
        public BattleZone BattleZone { get; private set; }
        public Zone Graveyard { get; private set; }

        public Duelist(Game game, params Type[] cards) {
            Game = game;
            DuelAction = new DuelAction(this);
            TaskList = new DuelTaskList(this);
            Deck = new Zone(this);
            Hand = new Hand(this);
            ShieldZone = new ShieldZone(this);
            ManaZone = new Zone(this);
            BattleZone = new BattleZone(this);
            Graveyard = new Zone(this);

            AllCards = new Card[cards.Length];
            int i = 0;
            foreach (Type card in cards) {
                Card createdCard = (Card)Activator.CreateInstance(card, this);
                AllCards[i] = createdCard;
                Deck.Put(createdCard);
                i += 1;
            }
        }

        public void SetUp() {
            Deck.Shuffle();
            Charge = 1;
            for (int i = 0; i < 5; i++) {
                Card drawn = DrawCard();
                if (drawn != null) {
                    ShieldZone.Put(drawn);
                } else {
                    break;
                }
            }
            for (int i = 0; i < 5; i++) {
                Draw();
            }
        }

        public void StartTurn() {
            Charge = 1;
            // Get rid of all Summoning Sickness.
            foreach (Creature creature in BattleZone.GetAll<Creature>())
                creature.SummoningSickness = false;
            ManaZone.UntapAll();
            BattleZone.UntapAll();
            OnTurnStarted();
        }

        public void EndTurn() {
            Task.Run(EndTurnAsync);
        }
        private async Task EndTurnAsync() {
            OnAtTurnEnd();
            await Task.Run(() => { while (Game.Waiting()) { Thread.Sleep(100); } });
            OnTurnEnded();
        }

        public delegate void DuelistEventHandler(Duelist source);
        public event DuelistEventHandler TurnStarted;
        protected virtual void OnTurnStarted() {
            TurnStarted?.Invoke(this);
        }
        public event DuelistEventHandler AtTurnEnd;
        protected virtual void OnAtTurnEnd() {
            AtTurnEnd?.Invoke(this);
        }
        public event DuelistEventHandler TurnEnded;
        protected virtual void OnTurnEnded() {
            TurnEnded?.Invoke(this);
        }

        /// <summary>
        /// Draw the top card of the Deck to Hand.
        /// </summary>
        public void Draw() {
            Card drawn = DrawCard();
            if (drawn != null) {
                Hand.Put(drawn);
            }
        }

        /// <summary>
        /// Returns the top card of the Deck, does NOT remove it from the Deck,
        /// (last index).
        /// </summary>
        public Card DrawCard() {
            if (Deck.NumberOfCards > 1) {
                return Deck[Deck.NumberOfCards - 1];
            } else {
                Game.Lose(this);
                return null;
            }
        }

        public string PrintZones() {
            return $"Deck: {Deck.Print()}\r\nHand: {Hand.Print()}\r\nShield Zone: {ShieldZone.Print()}\r\nMana Zone: {ManaZone.Print()}\r\nBattle Zone: {BattleZone.Print()}\r\nGraveyard: {Graveyard.Print()}\r\n";
        }

    }
}
