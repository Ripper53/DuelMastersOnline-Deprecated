using System;
using System.Collections.Generic;

namespace DuelMasters {
    /// <summary>
    /// <para>
    /// Default:
    /// Mana Number = 1,
    /// Shield Breaker = 1.
    /// </para>
    /// <para>
    /// Set:
    /// Name,
    /// Mana Cost,
    /// Power,
    /// Civilization,
    /// Race.
    /// </para>
    /// </summary>
    public abstract class Creature : Card {
        private CreatureData CreatureData { get; set; }
        protected sealed override CardData CardData => CreatureData;
        public int ShieldBreaker {
            get {
                return CreatureData.ShieldBreaker;
            }
            set {
                CreatureData.ShieldBreaker = value;
            }
        }
        public int Power {
            get {
                return CreatureData.Power;
            }
            set {
                CreatureData.Power = value;
            }
        }
        public IEnumerable<Race> Races => CreatureData.Races;

        public CreatureAction CreatureAction { get; private set; }
        public bool SummoningSickness { get; set; }
        private bool _blocker;
        public bool Blocker {
            get {
                return _blocker;
            }
            set {
                if (_blocker != value) {
                    _blocker = value;
                    if (Blocker) {
                        Owner.Game.BlockingCreature += BlockableCreature;
                        Owner.Game.BlockingPlayer += BlockablePlayer;
                    } else {
                        Owner.Game.BlockingCreature -= BlockableCreature;
                        Owner.Game.BlockingPlayer -= BlockablePlayer;
                    }
                }
            }
        }
        #region Blocker Delegates
        private void BlockableCreature(Game game, Creature atking, Creature defing) {
            if (CurrentZone is BattleZone && !Tapped && Owner == defing.Owner && this != defing) {
                if (!atking.Blockable(this)) return;
                Owner.TaskList.AddTask(new DuelTask(
                    $"{Name}: Block creature {atking.Name}.",
                    (args) => {
                        game.BattleWithoutWaiting(atking, this);
                        Owner.TaskList.ClearOptionalTasks();
                    }
                ));
            }
        }
        private void BlockablePlayer(Game game, Creature atking, Duelist defing) {
            if (CurrentZone is BattleZone && !Tapped && Owner == defing) {
                if (!atking.Blockable(this)) return;
                defing.TaskList.AddTask(new DuelTask(
                    $"{Name}: Block player {atking.Name}.",
                    (args) => {
                        game.BattleWithoutWaiting(atking, this);
                        Owner.TaskList.ClearOptionalTasks();
                    }
                ));
            }
        }
        #endregion

        public Creature(Duelist owner) : base(owner) {
            CreatureData = new CreatureData() {
                Name = OriginalName,
                ManaNumber = 1,
                ShieldBreaker = 1
            };
            CreatureAction = new CreatureAction(this);
            SummoningSickness = false;
            Blocker = false;
            Destroy = () => Owner.Graveyard.Put(this);
            CanAttack = (creature) => true;
            CanAttackPlayer = (player) => true;
            CanAttackUntapped = (creature) => false;
            Blockable = (blocker) => true;
        }

        public void Add(Race race) {
            if (!Contains(race)) {
                CreatureData.Races.Add(race);
            }
        }
        public void Remove(Race race) {
            CreatureData.Races.Remove(race);
        }
        public bool Contains(Race race) {
            return CreatureData.Races.Contains(race);
        }
        public bool Contains(IEnumerable<Race> races) {
            foreach (Race race in races) {
                if (Contains(race)) {
                    return true;
                }
            }
            return false;
        }

        public sealed override void Use() {
            Summon();
        }

        public void Summon() {
            Owner.BattleZone.Put(this);
            OnSummoned();
        }

        public Action Destroy { get; set; }

        /// <summary>
        /// Return true if THIS Creature CAN attack Creature, false if THIS Creature CANNOT attack Creature.
        /// Passed in Creature will be null when doing check while attacking a Player.
        /// </summary>
        public Func<Creature, bool> CanAttack;

        /// <summary>
        /// Return true if THIS Creature CAN attack the Player, false if THIS Creature CANNOT attack the Player.
        /// </summary>
        public Func<Duelist, bool> CanAttackPlayer;

        public Func<Creature, bool> CanAttackUntapped;

        /// <summary>
        /// Return true if Creature CAN block THIS Creature, false if Creature CANNOT block THIS Creature.
        /// </summary>
        public Func<Creature, bool> Blockable { get; set; }

        public delegate void CreatureEventHandler(Creature source);
        public event CreatureEventHandler Summoned;
        protected virtual void OnSummoned() {
            Summoned?.Invoke(this);
        }

        public delegate void BattleEventHandler(Creature source, Creature otherCreature);
        public event BattleEventHandler Blocked;
        /// <summary>
        /// Called in Game object.
        /// </summary>
        public virtual void OnBlocked(Creature blocker) {
            Blocked?.Invoke(this, blocker);
        }

        public event BattleEventHandler Defended;
        /// <summary>
        /// Called in Game object.
        /// </summary>
        public virtual void OnDefended(Creature atkingCreature) {
            Defended?.Invoke(this, atkingCreature);
        }

    }
}
