using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DuelMasters {
    public class Game {
        private int currentIdNumber;
        public int GetID() {
            currentIdNumber += 1;
            return currentIdNumber;
        }

        public enum State { IN_PROGRESS, COMPLETED }
        public State GameState { get; private set; }
        public enum Step { START, CHARGE, MAIN, ATTACK, END }
        private Step _step;
        public Step GameStep {
            get {
                return _step;
            }
            set {
                _step = value;
                OnStepChanged(GameStep);
            }
        }

        public bool Busy { get; private set; }
        public bool Waiting() {
            return Busy || Blocked || DuelistsDoingTask();
        }
        /// <summary>
        /// Used to battle Blocker against attacking Creature and cancel previous attack.
        /// </summary>
        public bool Blocked { get; private set; }

        private List<Duelist> allDuelists;
        public IEnumerable<Duelist> Duelists => allDuelists;
        public int TurnIndex { get; private set; }
        public Duelist CurrentDuelistTurn => allDuelists[TurnIndex];
        public int NumberOfDuelists => allDuelists.Count;

        public Duelist this[int index] => allDuelists[index];

        public bool DuelistsDoingTask() {
            foreach (Duelist duelist in allDuelists) {
                if (duelist.TaskList.DoingTask) {
                    return true;
                }
            }
            return false;
        }
        public bool OtherDuelistsDoingTask(Duelist duelistToExclude) {
            foreach (Duelist duelist in allDuelists) {
                if (duelist != duelistToExclude && duelist.TaskList.DoingTask) {
                    return true;
                }
            }
            return false;
        }
        public bool CurrentDuelistDoingTask() {
            return CurrentDuelistTurn.TaskList.DoingTask;
        }

        public Game(Type[] firstDeck, Type[] secondDeck) {
            GameState = State.IN_PROGRESS;
            currentIdNumber = -1;
            Duelist firstDuelist = new Duelist(this, firstDeck) {
                Name = "Duelist 0"
            };
            Duelist secondDuelist = new Duelist(this, secondDeck) {
                Name = "Duelist 1"
            };

            // Turn event handlers.
            firstDuelist.TurnStarted += OnDuelistTurnStarted;
            firstDuelist.AtTurnEnd += OnDuelistAtTurnEnd;
            firstDuelist.TurnEnded += OnDuelistTurnEnded;
            secondDuelist.TurnStarted += OnDuelistTurnStarted;
            secondDuelist.AtTurnEnd += OnDuelistAtTurnEnd;
            secondDuelist.TurnEnded += OnDuelistTurnEnded;

            // Battle Zone event handlers.
            firstDuelist.BattleZone.PutCard += OnBattleZonePut;
            firstDuelist.BattleZone.RemovedCard += OnBattleZoneRemoved;
            secondDuelist.BattleZone.PutCard += OnBattleZonePut;
            secondDuelist.BattleZone.RemovedCard += OnBattleZoneRemoved;

            allDuelists = new List<Duelist>(new Duelist[2] {
                firstDuelist,
                secondDuelist
            });

            TurnIndex = 0;
        }

        public void BeginDuel() {
            foreach (Duelist duelist in allDuelists) {
                duelist.SetUp();
            }
            ChargeStep();
        }

        #region Events
        public delegate void StepEventHandler(Game source, Step step);
        public event StepEventHandler StepChanged;
        protected virtual void OnStepChanged(Step step) {
            StepChanged?.Invoke(this, step);
        }

        public delegate void BattleZoneEventHandler(Game source, Zone battlezone, Card putCard);
        public event BattleZoneEventHandler BattleZonePut;
        protected virtual void OnBattleZonePut(Zone battleZone, Card putCard) {
            BattleZonePut?.Invoke(this, battleZone, putCard);
        }
        public event BattleZoneEventHandler BattleZoneRemoved;
        protected virtual void OnBattleZoneRemoved(Zone battleZone, Card removedCard) {
            BattleZoneRemoved?.Invoke(this, battleZone, removedCard);
        }

        public delegate void AttackEventHandler(Game source, Creature attackingCreature);
        public event AttackEventHandler Attacking;
        protected virtual void OnAttacking(Creature attackingCreature) {
            Attacking?.Invoke(this, attackingCreature);
        }
        public event AttackEventHandler Attacked;
        protected virtual void OnAttacked(Creature attackingCreature) {
            Attacked?.Invoke(this, attackingCreature);
        }

        public delegate void AttackCreatureEventHandler(Game source, Creature attackingCreature, Creature defendingCreature);
        public event AttackCreatureEventHandler AttackingCreature;
        protected virtual void OnAttackingCreature(Creature attackingCreature, Creature defendingCreature) {
            AttackingCreature?.Invoke(this, attackingCreature, defendingCreature);
        }
        public event AttackCreatureEventHandler BlockingCreature;
        protected virtual void OnBlockingCreature(Creature attackingCreature, Creature defendingCreature) {
            BlockingCreature?.Invoke(this, attackingCreature, defendingCreature);
        }
        public event AttackCreatureEventHandler BlockedCreature;
        protected virtual void OnBlockedCreature(Creature attackingCreature, Creature blocker) {
            BlockedCreature?.Invoke(this, attackingCreature, blocker);
        }
        public event AttackCreatureEventHandler AttackedCreature;
        protected virtual void OnAttackedCreature(Creature attackingCreature, Creature defendingCreature) {
            AttackedCreature?.Invoke(this, attackingCreature, defendingCreature);
        }

        public delegate void AttackPlayerEventHandler(Game source, Creature attackingCreature, Duelist defendingPlayer);
        public event AttackPlayerEventHandler AttackingPlayer;
        protected virtual void OnAttackingPlayer(Creature attackingCreature, Duelist defendingPlayer) {
            AttackingPlayer?.Invoke(this, attackingCreature, defendingPlayer);
        }
        public event AttackPlayerEventHandler BlockingPlayer;
        protected virtual void OnBlockingPlayer(Creature attackingCreature, Duelist defendingPlayer) {
            BlockingPlayer?.Invoke(this, attackingCreature, defendingPlayer);
        }
        public event AttackPlayerEventHandler AttackedPlayer;
        protected virtual void OnAttackedPlayer(Creature attackingCreature, Duelist defendingPlayer) {
            AttackedPlayer?.Invoke(this, attackingCreature, defendingPlayer);
        }

        public delegate void DuelistEventHandler(Game source, Duelist duelist);
        public event DuelistEventHandler DuelistTurnStarted;
        protected virtual void OnDuelistTurnStarted(Duelist duelist) {
            DuelistTurnStarted?.Invoke(this, duelist);
        }
        public event DuelistEventHandler DuelistAtTurnEnd;
        protected virtual void OnDuelistAtTurnEnd(Duelist duelist) {
            DuelistAtTurnEnd?.Invoke(this, duelist);
        }
        public event DuelistEventHandler DuelistTurnEnded;
        protected virtual void OnDuelistTurnEnded(Duelist duelist) {
            DuelistTurnEnded?.Invoke(this, duelist);
        }

        public event DuelistEventHandler DuelistEndAttackStep;
        protected virtual void OnDuelistEndAttackStep(Duelist duelist) {
            DuelistEndAttackStep?.Invoke(this, duelist);
        }
        #endregion

        #region Turn & Step Functions
        public void NextTurn() {
            TurnIndex += 1;
            if (TurnIndex >= NumberOfDuelists) TurnIndex = 0;
            StartStep();
        }

        public void NextStep() {
            if (Waiting()) return;
            switch (GameStep) {
                case Step.START:
                    ChargeStep();
                    break;
                case Step.CHARGE:
                    MainStep();
                    break;
                case Step.MAIN:
                    AttackStep();
                    break;
                case Step.ATTACK:
                    EndStep();
                    break;
                case Step.END:
                    NextTurn();
                    break;
            }
        }

        public void StartStep() {
            GameStep = Step.START;
            // Untap and effects.
            CurrentDuelistTurn.StartTurn();
            OnDuelistTurnStarted(CurrentDuelistTurn);
            CurrentDuelistTurn.Draw();
        }
        public void ChargeStep() {
            GameStep = Step.CHARGE;
        }
        public void MainStep() {
            GameStep = Step.MAIN;
        }
        public void AttackStep() {
            GameStep = Step.ATTACK;
        }

        public void EndStep() {
            Task.Run(EndAttackStepAsync);
        }
        private async Task EndAttackStepAsync() {
            OnDuelistEndAttackStep(CurrentDuelistTurn);
            await Task.Run(() => { while (Waiting()) { Thread.Sleep(100); } });
            CompleteEndStep();
        }
        public void CompleteEndStep() {
            GameStep = Step.END;
            // Effects.
            CurrentDuelistTurn.EndTurn();
        }
        #endregion

        #region Battle Functions
        public void Battle(Creature attackingCreature, Creature defendingCreature) {
            if (GameStep != Step.ATTACK || !attackingCreature.CreatureAction.EvalCanAttack(defendingCreature) || Waiting()) return;
            Busy = true;
            Task.Run(() => BattleAsync(attackingCreature, defendingCreature));
        }
        /// <summary>
        /// Used when the attacking Creature is Blocked.
        /// </summary>
        public void BattleWithoutWaiting(Creature attackingCreature, Creature defendingCreature) {
            Blocked = true;
            Busy = true;
            Task.Run(() => BattleWithoutTasksAsync(attackingCreature, defendingCreature));
        }
        private async Task BattleAsync(Creature attackingCreature, Creature defendingCreature) {
            OnAttacking(attackingCreature);
            OnAttackingCreature(attackingCreature, defendingCreature);
            // Wait for all On Attack effects to resolve.
            await Task.Run(() => { while (DuelistsDoingTask()) { Thread.Sleep(100); } });
            // If effect removed the attacking Creature from the Battle Zone, cancel the attack.
            if (!(attackingCreature.CurrentZone is BattleZone)) { Busy = false; return; }
            OnBlockingCreature(attackingCreature, defendingCreature);
            // Wait for Player to pick Blocker, or none to block.
            await Task.Run(() => { while (DuelistsDoingTask()) { Thread.Sleep(100); } });
            // If Blocked, cancel the attack.
            if (Blocked) { Blocked = false; return; }
            BattleCreatures(attackingCreature, defendingCreature);
            OnAttacked(attackingCreature);
            OnAttackedCreature(attackingCreature, defendingCreature);
            defendingCreature.OnDefended(attackingCreature);
            await Task.Run(() => { while (DuelistsDoingTask()) { Thread.Sleep(100); } });
            Busy = false;
        }
        /// <summary>
        /// Used when the attacking Creature is Blocked.
        /// </summary>
        private async Task BattleWithoutTasksAsync(Creature attackingCreature, Creature defendingCreature) {
            OnBlockedCreature(attackingCreature, defendingCreature);
            BattleCreatures(attackingCreature, defendingCreature);
            OnAttackedCreature(attackingCreature, defendingCreature);
            attackingCreature.OnBlocked(defendingCreature);
            defendingCreature.OnDefended(attackingCreature);
            await Task.Run(() => { while (DuelistsDoingTask()) { Thread.Sleep(100); } });
            Busy = false;
        }
        private void BattleCreatures(Creature attackingCreature, Creature defendingCreature) {
            attackingCreature.Tapped = true;
            defendingCreature.Tapped = true;
            // Check if Creatures are still in the Battle Zone.
            if (attackingCreature.CurrentZone is BattleZone && defendingCreature.CurrentZone is BattleZone) {
                int attackPower = attackingCreature.Power;
                int defendingPower = defendingCreature.Power;
                if (attackPower > defendingPower) {
                    defendingCreature.Destroy();
                } else if (attackPower < defendingPower) {
                    attackingCreature.Destroy();
                } else {
                    defendingCreature.Destroy();
                    attackingCreature.Destroy();
                }
            }
        }

        public void AttackPlayer(Creature attackingCreature, Duelist defendingDuelist) {
            if (GameStep != Step.ATTACK || !attackingCreature.CreatureAction.EvalCanAttackPlayer(defendingDuelist) || Waiting()) return;
            Busy = true;
            Task.Run(() => AttackPlayerAsync(attackingCreature, defendingDuelist));
        }
        private async Task AttackPlayerAsync(Creature attackingCreature, Duelist defendingPlayer) {
            OnAttacking(attackingCreature);
            OnAttackingPlayer(attackingCreature, defendingPlayer);
            // Wait for On Attack tasks to be completed.
            await Task.Run(() => { while (DuelistsDoingTask()) { Thread.Sleep(100); } });
            // If some effect caused the attacking Creature to be removed from the Battle Zone, cancel the attack.
            if (!(attackingCreature.CurrentZone is BattleZone)) { Busy = false; return; }
            OnBlockingPlayer(attackingCreature, defendingPlayer);
            // Wait for Player to pick a Blocker, or pick not to Block.
            await Task.Run(() => { while (DuelistsDoingTask()) { Thread.Sleep(100); } });
            // If Blocked, cancel the attack.
            if (Blocked) { Blocked = false; return; }
            attackingCreature.Tapped = true;
            // Check if attacking Creature is still in the Battle Zone.
            if (attackingCreature.CurrentZone is BattleZone) {
                if (defendingPlayer.ShieldZone.NumberOfCards > 0) {
                    for (int i = 0; i < attackingCreature.ShieldBreaker && defendingPlayer.ShieldZone.NumberOfCards > 0; i++)
                        defendingPlayer.ShieldZone.BreakShield(0);

                    /*string description = "Break ";
                    description += attackingCreature.ShieldBreaker > 1 ? $"{attackingCreature.ShieldBreaker} Shields." : " a Shield";
                    attackingCreature.Owner.TaskList.AddTask(new DuelTask(
                        description,
                        (args) => {
                            for (int i = 0; i < attackingCreature.ShieldBreaker && defendingPlayer.ShieldZone.NumberOfCards > 0; i++)
                                if (i >= args.Length) {
                                    defendingPlayer.ShieldZone.BreakShield(0);
                                } else {
                                    defendingPlayer.ShieldZone.BreakShield((Card)args[i]);
                                }
                        },
                        false,
                        () => defendingPlayer.ShieldZone.GetAll<Card>()
                    ));
                    await Task.Run(() => { while (DuelistsDoingTask()) { Thread.Sleep(100); } });*/
                } else {
                    // Duelist Lose
                    Lose(defendingPlayer);
                }
                OnAttacked(attackingCreature);
                OnAttackedPlayer(attackingCreature, defendingPlayer);
            }
            Busy = false;
        }
        #endregion

        public void Lose(Duelist loser) {
            if (allDuelists.Remove(loser)) {
                if (NumberOfDuelists <= 1) GameState = State.COMPLETED;
                OnLost(loser);
            }
        }

        public delegate void LostEventHandler(Game source, Duelist loser);
        public event LostEventHandler Lost;
        protected virtual void OnLost(Duelist loser) {
            Lost?.Invoke(this, loser);
        }

        #region Get Functions
        public Zone[] GetAllBattleZones() {
            Zone[] battleZones = new Zone[allDuelists.Count];
            for (int i = 0; i < battleZones.Length; i++) {
                battleZones[i] = allDuelists[i].BattleZone;
            }
            return battleZones;
        }

        public T[] GetAllInBattleZone<T>() where T : Card {
            List<T> allCards = new List<T>();
            foreach (Zone bz in GetAllBattleZones()) {
                foreach (Card card in bz.Cards) {
                    if (card is T) {
                        T cardType = (T)card;
                        allCards.Add(cardType);
                    }
                }
            }
            return allCards.ToArray();
        }
        public T[] GetAllInBattleZone<T>(Func<T, bool> predicate) where T : Card {
            List<T> allCards = new List<T>();
            foreach (Zone bz in GetAllBattleZones()) {
                foreach (Card card in bz.Cards) {
                    if (card is T) {
                        T cardType = (T)card;
                        if (predicate(cardType))
                            allCards.Add(cardType);
                    }
                }
            }
            return allCards.ToArray();
        }

        public T[] GetAllInBattleZone<T>(Zone zoneToExclude) where T : Card {
            List<T> allCards = new List<T>();
            foreach (Zone bz in GetAllBattleZones()) {
                if (bz != zoneToExclude) {
                    foreach (Card card in bz.Cards) {
                        if (card is T) allCards.Add((T)card);
                    }
                }
            }
            return allCards.ToArray();
        }
        public T[] GetAllInBattleZone<T>(Zone zoneToExclude, Func<T, bool> predicate) where T : Card {
            List<T> allCards = new List<T>();
            foreach (Zone bz in GetAllBattleZones()) {
                if (bz != zoneToExclude) {
                    foreach (Card card in bz.Cards) {
                        if (card is T) {
                            T cardType = (T)card;
                            if (predicate(cardType))
                                allCards.Add(cardType);
                        }
                    }
                }
            }
            return allCards.ToArray();
        }

        public Card SearchID(int cardId) {
            foreach (Duelist duelist in allDuelists) {
                foreach (Card card in duelist.AllCards) {
                    if (cardId == card.ID)
                        return card;
                }
            }
            return null;
        }
        #endregion

        public string PrintGame() {
            string gameStr = "";
            foreach (Duelist duelist in allDuelists) {
                if (duelist == CurrentDuelistTurn)
                    gameStr += $"TURN - {duelist.Name} -- Charge: {duelist.Charge}";
                else
                    gameStr += duelist.Name;
                gameStr += "\r\n" + duelist.PrintZones() + "\r\n";
            }
            return $"Current Step: {GameStep}\r\n\r\n{gameStr}";
        }

    }
}
