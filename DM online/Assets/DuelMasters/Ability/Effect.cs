using System;
using System.Collections.Generic;
using System.Linq;

namespace DuelMasters {
    public static class Effect {
        private static Random rng = new Random();

        #region Power Attacker
        public static void PowerAttacker(Creature source, int powerBuff) {
            Game.AttackEventHandler attacking = (game, atking) => {
                if (source == atking) atking.Power += powerBuff;
            };
            Game.AttackEventHandler attacked = (game, atking) => {
                if (source == atking) atking.Power -= powerBuff;
            };
            source.Owner.Game.Attacking += attacking;
            source.Owner.Game.Attacked += attacked;
        }

        /// <summary>
        /// Gains power buff IF predicate returns true.
        /// </summary>
        public static void PowerAttacker(Creature source, int powerBuff, Func<bool> predicate) {
            bool appliedBuff = false;
            source.Owner.Game.Attacking += (game, atking) => {
                if (source == atking && predicate()) {
                    atking.Power += powerBuff;
                    appliedBuff = true;
                }
            };
            source.Owner.Game.Attacked += (game, atking) => {
                if (appliedBuff) {
                    atking.Power -= powerBuff;
                    appliedBuff = false;
                }
            };
        }

        public static void PowerAttackerForEach(Creature source, int powerBuff, Func<int> forEach) {
            int appliedBuff = 0;
            source.Owner.Game.Attacking += (game, atking) => {
                if (source == atking) {
                    appliedBuff = forEach();
                    atking.Power += (powerBuff * appliedBuff);
                }
            };
            source.Owner.Game.Attacked += (game, atking) => {
                if (source == atking) {
                    atking.Power -= (powerBuff * appliedBuff);
                    appliedBuff = 0;
                }
            };
        }

        public static void PowerAttackerUntilEnd(Card source, int powerBuff, Func<Creature[]> args) {
            foreach (Creature creature in args()) {
                Game.AttackEventHandler attacking = (game, atking) => {
                    if (creature == atking) atking.Power += powerBuff;
                };
                Game.AttackEventHandler attacked = (game, atking) => {
                    if (creature == atking) atking.Power -= powerBuff;
                };
                creature.Owner.Game.Attacking += attacking;
                creature.Owner.Game.Attacked += attacked;
                creature.Owner.Game.DuelistTurnEnded += (game, duelist) => {
                    creature.Owner.Game.Attacking -= attacking;
                    creature.Owner.Game.Attacked -= attacked;
                };
            }
        }

        public static DuelTask PowerAttackUntilEndTask(Card source, int powerBuff, int numberOfArgs = 1, bool optional = true) {
            string description = $"{source.Name}: Give ";
            description += numberOfArgs > 1 ? $"{numberOfArgs} creatures" : "a creature";
            description += $" power attacker +{powerBuff} until the end of the turn.";
            Func<Creature[]> allArgs = () => source.Owner.BattleZone.GetAll<Creature>();
            return new DuelTask(
                description,
                (args) => {
                    Creature[] allCreatures = allArgs();
                    for (int i = 0; i < numberOfArgs && allCreatures.Length > 0; i++) {
                        if (i >= args.Length) {
                            if (optional)
                                break;
                            else
                                PowerAttackerUntilEnd(source, powerBuff, () => new Creature[] { allCreatures[0] });
                        } else {
                            PowerAttackerUntilEnd(source, powerBuff, () => new Creature[] { (Creature)args[i] });
                        }
                        allCreatures = allArgs();
                    }
                },
                optional,
                allArgs
            );
        }
        public static void ShieldBreakerUntilEnd(Card source, int shieldBreaker, Func<Creature[]> args) {
            Creature[] allCreatures = args();
            int[] shieldMemory = new int[allCreatures.Length];
            for (int i = 0; i < allCreatures.Length; i++) {
                shieldMemory[i] = allCreatures[i].ShieldBreaker;
                if (shieldBreaker > allCreatures[i].ShieldBreaker) {
                    allCreatures[i].ShieldBreaker = shieldBreaker;
                }
            }
            Game.DuelistEventHandler a = null;
            a = (game, duelist) => {
                for (int i = 0; i < allCreatures.Length; i++) {
                    if (shieldMemory[i] > allCreatures[i].ShieldBreaker)
                        allCreatures[i].ShieldBreaker = shieldMemory[i];
                }
                source.Owner.Game.DuelistTurnEnded -= a;
            };
            source.Owner.Game.DuelistTurnEnded += a;
        }
        public static DuelTask ShieldBreakerUntilEndTask(Card source, int shieldBreaker, int numberOfTargets = 1, bool optional = true) {
            string description = $"{source.Name}: Give ";
            description += numberOfTargets > 1 ? $"{numberOfTargets} creatures" : "a creature";
            description += " shield breaker";
            description += shieldBreaker > 1 ? $" {shieldBreaker}." : ".";
            Func<Creature[]> allArgs = () => source.Owner.BattleZone.GetAll<Creature>();
            return new DuelTask(
                description,
                (args) => {
                    List<Creature> creaturesToBuff = new List<Creature>();
                    Creature[] allCreatures = allArgs();
                    for (int i = 0; i < numberOfTargets && allCreatures.Length > 0; i++) {
                        if (i >= args.Length) {
                            if (optional) {
                                break;
                            } else {
                                foreach (Creature creature in allCreatures) {
                                    if (!creaturesToBuff.Contains(creature)) {
                                        creaturesToBuff.Add(creature);
                                        break;
                                    }
                                }
                            }
                        } else {
                            creaturesToBuff.Add((Creature)args[i]);
                        }
                        allCreatures = allArgs();
                    }
                    ShieldBreakerUntilEnd(source, shieldBreaker, () => creaturesToBuff.ToArray());
                },
                optional,
                allArgs
            );
        }

        public static DuelTask GroupEffect(Card source, bool optionalEffects, Func<object[]> selectableArgs, params Func<DuelTask>[] effects) {
            string description = "";
            foreach (Func<DuelTask> effect in effects) {
                description += effect().Description + "\r\n";
            }
            return new DuelTask(
                description,
                (args) => {
                    foreach (Func<DuelTask> effect in effects) {
                        effect().CompleteTask(args);
                    }
                },
                optionalEffects,
                selectableArgs
            );
        }
        #endregion

        #region Slayer
        public static void Slayer(Creature source) {
            source.Owner.Game.AttackedCreature += Slayer_AttackedCreature;
            source.Defended += Slayer_Defended;
        }
        public static void RemoveSlayer(Creature source) {
            source.Owner.Game.AttackedCreature -= Slayer_AttackedCreature;
            source.Defended -= Slayer_Defended;
        }
        private static void Slayer_AttackedCreature(Game game, Creature atking, Creature defing) {
            if (defing.CurrentZone is BattleZone)
                defing.Destroy();
        }
        private static void Slayer_Defended(Creature defing, Creature atking) {
            if (atking.CurrentZone is BattleZone)
                atking.Destroy();
        }
        #endregion

        /// <typeparam name="T">Card type that can be put into the Graveyard.</typeparam>
        public static DuelTask ManaZoneToGraveyardTask<T>(Card source, int numberOfCardsToTransfer = 1, bool optionalTransfer = true) where T : Card {
            string description = "Put ";
            description += numberOfCardsToTransfer > 1 ? $"{numberOfCardsToTransfer} {GetTypeString(typeof(T))}s" : $"a {GetTypeString(typeof(T))}";
            description += " from your mana zone into your graveyard.";
            Func<T[]> allArgs = () => source.Owner.ManaZone.GetAll<T>();
            return new DuelTask(
                description,
                (args) => {
                    T[] allCards = allArgs();
                    Zone graveyard = source.Owner.Graveyard;
                    for (int i = 0; i < numberOfCardsToTransfer && allCards.Length > 0; i++) {
                        if (i >= args.Length) {
                            if (optionalTransfer)
                                break;
                            else
                                graveyard.Put(allCards[0]);
                        } else {
                            graveyard.Put((T)args[i]);
                        }
                        allCards = allArgs();
                    }
                },
                optionalTransfer,
                allArgs
            );
        }

        public static void DestroyAfterWinningBattle(Creature source) {
            source.Owner.Game.AttackedCreature += (game, atking, defing) => {
                if (source.CurrentZone is BattleZone)
                    source.Destroy();
            };
            source.Defended += (defing, atking) => {
                if (source.CurrentZone is BattleZone)
                    source.Destroy();
            };
        }

        public static void Draw(Card source, int numberOfCardsToDraw = 1) {
            for (int i = 0; i < numberOfCardsToDraw; i++) source.Owner.Draw();
        }

        public static DuelTask DrawTask(Card source, int numberOfCardsToDraw = 1, bool optionalDraw = true) {
            string description = numberOfCardsToDraw > 1 ? $"Draw {numberOfCardsToDraw} cards." : "Draw a card.";
            return new DuelTask(
                $"{source.Name}: {description}",
                (args) => Draw(source, numberOfCardsToDraw),
                optionalDraw
            );
        }

        public static DuelTask UntapSelfTask(Creature source, bool optionalUntap = true) {
            return new DuelTask(
                $"{source.Name}: Untap.",
                (args) => source.Tapped = false,
                optionalUntap
            );
        }

        public static DuelTask BounceTask<T>(Card source, int numberToBounce = 1, bool optionalBounce = true) where T : Card {
            string description = $"{source.Name}: Return ";
            description += numberToBounce > 1 ? $"{numberToBounce} {GetTypeString(typeof(T))}s" : $"a {GetTypeString(typeof(T))}";
            description += " in the battle zone to their owners' hands.";
            return new DuelTask(
                description,
                (args) => {
                    for (int i = 0; i < numberToBounce && i < args.Length; i++) {
                        T card = (T)args[i];
                        card.Owner.Hand.Put(card);
                    }
                },
                optionalBounce,
                () => source.Owner.Game.GetAllInBattleZone<T>()
            );
        }

        public static void DestroyAllBlockers(Card source) {
            foreach (Creature creature in source.Owner.Game.GetAllInBattleZone<Creature>()) {
                if (creature.Blocker) creature.Destroy();
            }
        }

        public static DuelTask DestroyAllBlockersTask(Card source, bool optionaDestroy = true) {
            return new DuelTask(
                $"{source.Name}: Destroy all blockers.",
                (args) => DestroyAllBlockers(source),
                optionaDestroy
            );
        }

        public static void DestroyAllWithPowerLessOrEqualTo(Card source, int power) {
            foreach (Creature creature in source.Owner.Game.GetAllInBattleZone<Creature>()) {
                if (creature.Power <= power) creature.Destroy();
            }
        }

        public static DuelTask DestroyAllWithPowerLessOrEqualToTask(Card source, int power, bool optionaDestroy = true) {
            return new DuelTask(
                $"{source.Name}: Destroy all creatures that have {power} power or less.",
                (args) => DestroyAllWithPowerLessOrEqualTo(source, power),
                optionaDestroy
            );
        }

        public static void TapAllOpp(Card source) {
            foreach (Zone bz in source.Owner.Game.GetAllBattleZones()) {
                if (bz != source.Owner.BattleZone) {
                    foreach (Creature creature in bz.GetAll<Creature>()) {
                        creature.Tapped = true;
                    }
                }
            }
        }

        public static DuelTask TapOppTask(Card source, int numberOfCreaturesToTap = 1, bool optionalTap = true) {
            string description = $"{source.Name}: Tap ";
            description += numberOfCreaturesToTap > 1 ? $"{numberOfCreaturesToTap} creatures" : "a creature";
            description += " your opponent controls.";
            return new DuelTask(
                description,
                (args) => {
                    for (int i = 0; i < numberOfCreaturesToTap && i < args.Length; i++)
                        ((Creature)args[i]).Tapped = true;
                },
                optionalTap,
                () => source.Owner.Game.GetAllInBattleZone<Creature>(source.Owner.BattleZone)
            );
        }

        /// <summary>
        /// Static effect that affects the Battle Zone.
        /// </summary>
        public static void StaticEffectAll<T>(Creature source, Action<T> effect, Action<T> removeEffect) where T : Card {
            Game game = source.Owner.Game;
            game.BattleZonePut += (g, bz, putCard) => {
                if (putCard == source) {
                    // If Creature put into the Battle Zone is this Creature then apply effect to all.
                    foreach (T card in game.GetAllInBattleZone<T>())
                        effect(card);
                } else if (source.CurrentZone is BattleZone && putCard is T) {
                    // If Creature is already in Battle Zone and the put Card is a Creature, apply effect to put Creature.
                    T putCardType = (T)putCard;
                    effect(putCardType);
                }
            };
            game.BattleZoneRemoved += (g, bz, removedCard) => {
                if (removedCard == source) {
                    // If Creature removed from the Battle Zone is this Creature then remove effect from all.
                    foreach (T card in game.GetAllInBattleZone<T>())
                        removeEffect(card);
                } else if (source.CurrentZone is BattleZone && removedCard is T) {
                    // If Creature is in Battle Zone and the removed Card is a Creature, remove effect from removed Creature.
                    T removedCardType = (T)removedCard;
                    removeEffect(removedCardType);
                }
            };
        }
        /// <summary>
        /// Static effect that affects friendly side of Battle Zone only.
        /// </summary>
        public static void StaticEffectAllFriendly<T>(Creature source, Action<T> effect, Action<T> removeEffect) where T : Card {
            Game game = source.Owner.Game;
            game.BattleZonePut += (g, bz, putCard) => {
                if (putCard == source) {
                    // If Creature put into the Battle Zone is this Creature then apply effect to all.
                    foreach (T card in source.Owner.BattleZone.GetAll<T>())
                        effect(card);
                } else if (source.CurrentZone is BattleZone && putCard.Owner == source.Owner && putCard is T) {
                    // If Creature is already in Battle Zone and the put Card is a Creature, apply effect to put Creature.
                    T putCardType = (T)putCard;
                    effect(putCardType);
                }
            };
            game.BattleZoneRemoved += (g, bz, removedCard) => {
                if (removedCard == source) {
                    // If Creature removed from the Battle Zone is this Creature then remove effect from all.
                    foreach (T card in source.Owner.BattleZone.GetAll<T>())
                        removeEffect(card);
                } else if (source.CurrentZone is BattleZone && removedCard.Owner == source.Owner && removedCard is T) {
                    // If Creature is in Battle Zone and the removed Card is a Creature, remove effect from removed Creature.
                    T removedCardType = (T)removedCard;
                    removeEffect(removedCardType);
                }
            };
        }

        public static void StaticPowerBuffSelf(Creature source, int powerBuff, Race conditionRace) {
            bool havePowerBuff = false;
            StaticEffectAllFriendly<Creature>(
                source,
                (creature) => {
                    if (!havePowerBuff && creature.Contains(conditionRace)) {
                        source.Power += powerBuff;
                        havePowerBuff = true;
                    }
                },
                (creature) => {
                    if (havePowerBuff && creature.Contains(conditionRace)) {
                        source.Power -= powerBuff;
                        havePowerBuff = false;
                    }
                }
            );
        }

        public static void StaticSpellEffectUntilEnd<T>(Spell source, Action<T> effect, Action<T> removeEffect) where T : Card {
            StaticSpellEffectUntilEndMaker(source, effect, removeEffect, () => source.Owner.Game.GetAllInBattleZone<T>());
        }
        public static void StaticSpellEffectUntilEndFriendly<T>(Spell source, Action<T> effect, Action<T> removeEffect) where T : Card {
            StaticSpellEffectUntilEndMaker(source, effect, removeEffect, () => source.Owner.BattleZone.GetAll<T>());
        }
        public static void StaticSpellEffectUntilEndOpp<T>(Spell source, Action<T> effect, Action<T> removeEffect) where T : Card {
            StaticSpellEffectUntilEndMaker(source, effect, removeEffect, () => source.Owner.Game.GetAllInBattleZone<T>(source.Owner.BattleZone));
        }
        private static void StaticSpellEffectUntilEndMaker<T>(Spell source, Action<T> effect, Action<T> removeEffect, Func<T[]> toApply) where T : Card {
            Game game = source.Owner.Game;
            foreach (T card in toApply())
                effect(card);

            Game.BattleZoneEventHandler applyEffectOnPut = (g, bz, putCard) => {
                if (putCard is T)
                    effect((T)putCard);
            };
            game.BattleZonePut += applyEffectOnPut;

            Game.BattleZoneEventHandler removeEffectOnRemove = (g, bz, removedCard) => {
                if (removedCard is T)
                    removeEffect((T)removedCard);
            };
            game.BattleZoneRemoved += removeEffectOnRemove;

            Game.DuelistEventHandler turnEnd = null;
            turnEnd = (g, duelist) => {
                foreach (T card in toApply())
                    removeEffect(card);

                game.BattleZonePut -= applyEffectOnPut;
                game.BattleZoneRemoved -= removeEffectOnRemove;
                game.DuelistAtTurnEnd -= turnEnd;
            };
            game.DuelistTurnEnded += turnEnd;
        }

        public static DuelTask CannotBeBlockedTask(Card source, int numberOfCreaturesThatCannotBeBlocker = 1, bool optionalCannotBlock = true) {
            string description = $"{source.Name}: Choose ";
            description += numberOfCreaturesThatCannotBeBlocker > 1 ? $"{numberOfCreaturesThatCannotBeBlocker} creatures, they" : "a creature, it";
            description += " cannot be blocked this turn.";
            return new DuelTask(
                description,
                (args) => {
                    for (int i = 0; i < numberOfCreaturesThatCannotBeBlocker && i < args.Length; i++) {
                        Creature creature = (Creature)args[i];
                        Func<Creature, bool> oldBlockable = creature.Blockable;
                        creature.Owner.Game.DuelistTurnEnded += (g, d) => creature.Blockable = oldBlockable;
                        creature.Blockable = (blocker) => false;
                    }
                },
                optionalCannotBlock,
                () => source.Owner.BattleZone.GetAll<Creature>()
            );
        }

        public static DuelTask SearchDeckForTask<T>(Card source, int numberOfCardsToTake = 1, bool optionalSearch = true) where T : Card {
            string description = $"{source.Name}: Take ";
            description += numberOfCardsToTake > 1 ? $"{numberOfCardsToTake} {GetTypeString(typeof(T))}s" : $"a {GetTypeString(typeof(T))}";
            description += " from your deck and put it into your hand.";
            Func<T[]> allArgs = () => source.Owner.Deck.GetAll<T>();
            return new DuelTask(
                description,
                (args) => {
                    T[] allCards = allArgs();
                    Zone hand = source.Owner.Hand;
                    for (int i = 0; i < numberOfCardsToTake && allCards.Length > 0; i++) {
                        if (i >= args.Length) {
                            if (optionalSearch)
                                break;
                            else
                                hand.Put(allCards[0]);
                        } else {
                            hand.Put((T)args[i]);
                        }
                        allCards = allArgs();
                    }
                    source.Owner.Deck.Shuffle();
                },
                optionalSearch,
                allArgs
            );
        }

        public static DuelTask UntapAllFriendlyTask(Card source, bool optionalUntap = true) {
            return new DuelTask(
                $"{source.Name}: Untap all your creatures in the battle zone.",
                (args) => {
                    foreach (Creature creature in source.Owner.BattleZone.GetAll<Creature>())
                        creature.Tapped = false;
                },
                optionalUntap
            );
        }

        public static DuelTask DrawUpTo(Card source, int numberOfCardsThatCanBeDrawn = 1) {
            string description = $"{source.Name}: Draw ";
            description += numberOfCardsThatCanBeDrawn > 1 ? $"up to {numberOfCardsThatCanBeDrawn} cards." : "a card.";
            return new DuelTask(
                description,
                (args) => {
                    if (args.Length > 0) {
                        int upTo = (int)args[0];
                        for (int i = 0; i < upTo; i++) {
                            source.Owner.Draw();
                        }
                    }
                },
                true,
                () => {
                    return new object[1] { typeof(Int32) };
                }
            );
        }

        public static DuelTask BounceAllTask<T>(Card source, bool optionalBounce = true) where T : Card {
            string description = $"{source.Name}: Return all {GetTypeString(typeof(T))}s in the battle zone to their owners' hands.";
            return BounceAllTask<T>(
                source,
                description,
                (card) => true,
                optionalBounce
            );
        }

        /// <summary>
        /// Returns all Creatures to their Owner's Hands if Creature has Power equal to or less then power.
        /// </summary>
        /// <param name="power">Creatures with power equal to, or less than, will be returned to their Owners' hands.s</param>
        public static DuelTask BounceAllWithLessOrEqualPowerTask(Card source, int power, bool optionalBounce = true) {
            string description = $"{source.Name}: Return all creatures in the battle zone that have power {power} or less to their owners' hands.";
            return BounceAllTask<Creature>(
                source,
                description,
                (creature) => creature.Power <= power,
                optionalBounce
            );
        }

        public static DuelTask BounceAllTask<T>(Card source, string description, Func<T, bool> predicate, bool optionalBounce = true) where T : Card {
            return new DuelTask(
                $"{source.Name}: {description}",
                (args) => {
                    foreach (T card in source.Owner.Game.GetAllInBattleZone<T>()) {
                        if (predicate(card))
                            card.Owner.Hand.Put(card);
                    }
                },
                optionalBounce
            );
        }

        public static DuelTask DestroyFriendlyTask(Card source, int numberOfCreaturesToDestroy = 1, bool optionalDestroy = true) {
            string description = $"{source.Name}: Destroy ";
            description += numberOfCreaturesToDestroy > 1 ? $"{numberOfCreaturesToDestroy} of your creatures." : "1 of your creature.";
            Func<Creature[]> allArgs = () => source.Owner.BattleZone.GetAll<Creature>();
            return new DuelTask(
                description,
                (args) => {
                    Creature[] allCreatures = allArgs();
                    for (int i = 0; i < numberOfCreaturesToDestroy && allCreatures.Length > 0; i++) {
                        if (i >= args.Length) {
                            if (optionalDestroy)
                                break;
                            else
                                allCreatures[0].Destroy();
                        } else {
                            ((Creature)args[i]).Destroy();
                        }
                        allCreatures = allArgs();
                    }
                },
                optionalDestroy,
                allArgs
            );
        }

        public static void OppDestroyTask(Card source, int numberOfCreaturesForOppToDestroy = 1) {
            string description = $"{source.Name}: Destroy ";
            description += numberOfCreaturesForOppToDestroy > 1 ? $"{numberOfCreaturesForOppToDestroy} of your creatures." : "1 of your creatures.";
            foreach (Duelist duelist in source.Owner.Game.Duelists) {
                if (duelist != source.Owner) {
                    duelist.TaskList.AddTask(
                        new DuelTask(
                            description,
                            (args) => {
                                Creature[] allCreatures = duelist.BattleZone.GetAll<Creature>();
                                for (int i = 0; i < numberOfCreaturesForOppToDestroy && allCreatures.Length > 0; i++) {
                                    if (i >= args.Length) {
                                        allCreatures[0].Destroy();
                                    } else {
                                        ((Creature)args[0]).Destroy();
                                    }
                                    allCreatures = duelist.BattleZone.GetAll<Creature>();
                                }
                            }, 
                            false,
                            () => duelist.BattleZone.GetAll<Creature>()
                        )
                    );
                }
            }
            
        }

        public static DuelTask ReturnFromGraveyardTask<T>(Card source, int numberOfCardsToReturn = 1, bool optionalReturn = true) where T : Card {
            string description = "Return ";
            description += numberOfCardsToReturn > 1 ? $"{numberOfCardsToReturn} {GetTypeString(typeof(T))}s" : $"a {GetTypeString(typeof(T))}";
            description += " to your hand from your graveyard.";
            return new DuelTask(
                description,
                (args) => {
                    for (int i = 0; i < numberOfCardsToReturn && i < args.Length; i++)
                        source.Owner.Hand.Put((T)args[i]);
                },
                optionalReturn,
                () => source.Owner.Graveyard.GetAll<T>()
            );
        }

        public static DuelTask DestroySelfOrOtherTask(Creature source, int numberOfOthersToDestroy = 1) {
            string description = $"{source.Name}: Destroy ";
            description += numberOfOthersToDestroy > 1 ? $"{numberOfOthersToDestroy} other creatures" : "an other creature";
            description += " or destroy this creature.";
            return new DuelTask(
                description,
                (args) => {
                    if (args.Length < numberOfOthersToDestroy || args.Contains(source)) {
                        source.Destroy();
                    } else {
                        for (int i = 0; i < numberOfOthersToDestroy; i++)
                            ((Creature)args[i]).Destroy();
                    }
                },
                false,
                () => source.Owner.BattleZone.GetAll<Creature>()
            );
        }

        public static DuelTask DestroyOppTask(Card source, int numberOfCreaturesToDestroy = 1, bool optionalDestroy = true) {
            string description = $"{source.Name}: Destroy ";
            description += numberOfCreaturesToDestroy > 1 ? $"{numberOfCreaturesToDestroy} creatures." : "a creature.";
            Func<Creature[]> allArgs = () => source.Owner.Game.GetAllInBattleZone<Creature>(source.Owner.BattleZone);
            return new DuelTask(
                description,
                (args) => {
                    Creature[] allCreatures = allArgs();
                    for (int i = 0; i < numberOfCreaturesToDestroy && allCreatures.Length > 0; i++) {
                        if (i >= args.Length) {
                            if (optionalDestroy)
                                break;
                            else
                                allCreatures[0].Destroy();
                        } else {
                            ((Creature)args[i]).Destroy();
                        }
                        allCreatures = allArgs();
                    }
                },
                optionalDestroy,
                allArgs
            );
        }
        public static DuelTask DestroyOppTask(Card source, string description, Func<Creature, bool> predicate, int numberOfCreaturesToDestroy = 1, bool optionalDestroy = true) {
            Func<Creature[]> allArgs = () => source.Owner.Game.GetAllInBattleZone(source.Owner.BattleZone, predicate);
            return new DuelTask(
                $"{source.Name}: {description}",
                (args) => {
                    Creature[] allCreatures = allArgs();
                    for (int i = 0; i < numberOfCreaturesToDestroy && allCreatures.Length > 0; i++) {
                        if (i >= args.Length) {
                            if (optionalDestroy)
                                break;
                            else
                                allCreatures[0].Destroy();
                        } else {
                            ((Creature)args[i]).Destroy();
                        }
                        allCreatures = allArgs();
                    }
                },
                optionalDestroy,
                allArgs
            );
        }
        public static DuelTask DestroyPowerLessOrEqualOppTask(Card source, int power, int numberOfCreaturesToDestroy = 1, bool optionalDestroy = true) {
            string description = $"{source.Name}: Destroy ";
            description += numberOfCreaturesToDestroy > 1 ? $"{numberOfCreaturesToDestroy} creatures" : "a creature";
            description += $"that have {power} or less.";
            return DestroyOppTask(source, description, (creature) => { if (creature.Power <= power) return true; else return false; }, numberOfCreaturesToDestroy, optionalDestroy);
        }

        public static DuelTask DestroyOppTappedCreatureTask(Card source, bool tapped = true, int numberOfCreaturesToDestroy = 1, bool optionalDestroy = true) {
            string description = $"{source.Name}: Destroy ";
            description += numberOfCreaturesToDestroy > 1 ? $"{numberOfCreaturesToDestroy} creatures that are" : "a creature that is";
            description += tapped ? " tapped." : " untapped.";
            Func<Creature[]> allArgs = () => source.Owner.Game.GetAllInBattleZone<Creature>(source.Owner.BattleZone, (creature) => creature.Tapped == tapped);
            return new DuelTask(
                description,
                (args) => {
                    Creature[] allCreatures = allArgs();
                    for (int i = 0; i < numberOfCreaturesToDestroy && allCreatures.Length > 0; i++) {
                        if (i >= args.Length) {
                            if (optionalDestroy)
                                break;
                            else
                                allCreatures[0].Destroy();
                        } else {
                            ((Creature)args[i]).Destroy();
                        }
                        allCreatures = allArgs();
                    }
                },
                optionalDestroy,
                allArgs
            );
        }
        /// <typeparam name="T">Card type to move.</typeparam>
        public static DuelTask BattleZoneToManaZoneFriendlyTask<T>(Card source, int numberOfTargets = 1, bool optional = true) where T : Card {
            string description = $"{source.Name}: Choose ";
            description += numberOfTargets > 1 ? $"{numberOfTargets} {GetTypeString(typeof(T))}s in your battle zone and put them" : $"a {GetTypeString(typeof(T))} in your battle zone and put it";
            description += " into the mana zone.";
            return ArgsToManaZoneMaker(description, () => source.Owner.BattleZone.GetAll<T>(), numberOfTargets, optional);
        }
        /// <typeparam name="T">Card type to move.</typeparam>
        public static DuelTask BattleZoneToManaZoneOppTask<T>(Card source, int numberOfTargets = 1, bool optional = true) where T : Card {
            string description = $"{source.Name}: Choose ";
            description += numberOfTargets > 1 ? $"{numberOfTargets} of your opponent's {GetTypeString(typeof(T))}s in the battle zone and put them" : $"a opponent's {GetTypeString(typeof(T))} in the battle zone and put it";
            description += " into the mana zone.";
            return ArgsToManaZoneMaker(description, () => source.Owner.Game.GetAllInBattleZone<T>(source.Owner.BattleZone), numberOfTargets, optional);
        }
        public static DuelTask HandToManaZoneTask<T>(Card source, int numberOfTargets = 1, bool optional = true) where T : Card {
            string description = $"{source.Name}: Choose ";
            description += numberOfTargets > 1 ? $"{numberOfTargets} {GetTypeString(typeof(T))}s in your hand and put them" : $"a {GetTypeString(typeof(T))} in your hand and put it";
            description += " into the mana zone.";
            return ArgsToManaZoneMaker(description, () => source.Owner.Hand.GetAll<T>(), numberOfTargets, optional);
        }
        public static void OppBattleZoneToManaZoneTask<T>(Card source, int numberOfTargets = 1, bool optional = false) where T : Card {
            Duelist owner = source.Owner;
            foreach (Duelist duelist in owner.Game.Duelists) {
                if (duelist != owner)
                    duelist.TaskList.AddTask(BattleZoneToManaZoneOppTask<T>(source, numberOfTargets, optional));
            }
        }
        public static DuelTask GraveyardToManaZoneFriendlyTask<T>(Card source, int numberOfTargets = 1, bool optional = true) where T : Card {
            string description = $"{source.Name}: Choose ";
            description += numberOfTargets > 1 ? $"{numberOfTargets} {GetTypeString(typeof(T))}s in your graveyard and put them" : $"a {GetTypeString(typeof(T))} in your graveyard and put it";
            description += " into your mana zone.";
            return ArgsToManaZoneMaker(description, () => source.Owner.Graveyard.GetAll<T>(), numberOfTargets, optional);
        }
        /// <typeparam name="T">Card type to move.</typeparam>
        private static DuelTask ArgsToManaZoneMaker<T>(string description, Func<T[]> allArgs, int numberOfTargets = 1, bool optional = true) where T : Card {
            return new DuelTask(
                description,
                (args) => {
                    T[] allCards = allArgs();
                    for (int i = 0; i < numberOfTargets && allCards.Length > 0; i++) {
                        if (i >= args.Length) {
                            if (optional)
                                break;
                            else
                                allCards[0].Owner.ManaZone.Put(allCards[0]);
                        } else {
                            T card = (T)args[i];
                            card.Owner.ManaZone.Put(card);
                        }
                        allCards = allArgs();
                    }
                },
                optional,
                allArgs
            );
        }

        public static void OppDiscardRandom(Card source, int numberOfCardsToDiscard = 1) {
            for (int i = 0; i < numberOfCardsToDiscard; i++) {
                foreach (Duelist duelist in source.Owner.Game.Duelists) {
                    if (duelist != source.Owner && duelist.Hand.NumberOfCards > 0) {
                        duelist.Hand.Discard(rng.Next(0, duelist.Hand.NumberOfCards));
                    }
                }
            }
        }

        public static DuelTask OppDiscardRandomTask(Card source, int numberOfCardsToDiscard = 1, bool optionalDiscard = true) {
            string description = $"{source.Name}: Your opponent discards ";
            description += numberOfCardsToDiscard > 1 ? $"{numberOfCardsToDiscard} cards" : "a card";
            description += " from his hand.";
            return new DuelTask(
                description,
                (args) => OppDiscardRandom(source, numberOfCardsToDiscard),
                optionalDiscard
            );
        }

        public static void CanAttackUntapped(Creature source) {

        }

        public static DuelTask AttackUntappedCreature(Spell source, int numberOfCreaturesToTarget = 1, bool optional = true) {
            string description = $"{source.Name}: Choose ";
            description += numberOfCreaturesToTarget > 1 ? $"{numberOfCreaturesToTarget} creatures, they" : "a creature, it";
            description += " can be attacked when untapped this turn.";
            Func<Creature[]> allArgs = () => source.Owner.Game.GetAllInBattleZone<Creature>(source.Owner.BattleZone);
            List<Creature> attackableWhenUntapped = new List<Creature>();
            return new DuelTask(
                description,
                (args) => {
                    Creature[] allCreatures = allArgs();
                    for (int i = 0; i < numberOfCreaturesToTarget && allCreatures.Length > 0; i++) {
                        if (i >= args.Length) {
                            if (optional) {
                                break;
                            } else {
                                foreach (Creature c in allCreatures) {
                                    if (!attackableWhenUntapped.Contains(c)) {
                                        attackableWhenUntapped.Add(c);
                                        break;
                                    }
                                }
                            }
                        } else {
                            attackableWhenUntapped.Add((Creature)args[i]);
                        }
                        allCreatures = allArgs();
                    }
                    StaticSpellEffectUntilEndFriendly<Creature>(
                        source,
                        (creature) => {
                            foreach (Creature c in attackableWhenUntapped)
                                creature.CreatureAction.CanAttackUntapped.Add(c);
                        },
                        (creature) => {
                            foreach (Creature c in attackableWhenUntapped)
                                creature.CreatureAction.CanAttackUntapped.Remove(c);
                        }
                    );
                },
                optional,
                allArgs
            );
        }

        public static void AttackEachTurnIfAble(Creature source) {
            Game game = source.Owner.Game;
            bool attacked = false;
            Duelist.DuelistEventHandler setHasAttackedToFalse = (duelist) => {
                attacked = false;
            };
            Game.AttackEventHandler setHasAttackedToTrue = (g, atking) => {
                if (atking == source)
                    attacked = true;
            };
            Game.DuelistEventHandler attack = (g, d) => {
                foreach (Duelist duelist in game.Duelists) {
                    if (attacked) return;
                    if (source.Owner != duelist)
                        game.AttackPlayer(source, duelist);
                }
                foreach (Creature creature in game.GetAllInBattleZone<Creature>(source.Owner.BattleZone)) {
                    if (attacked) return;
                    game.Battle(source, creature);
                }
            };
            game.BattleZonePut += (g, bz, putCard) => {
                if (putCard == source) {
                    source.Owner.TurnStarted += setHasAttackedToFalse;
                    game.Attacked += setHasAttackedToTrue;
                    game.DuelistEndAttackStep += attack;
                }
            };
            game.BattleZoneRemoved += (g, bz, removedCard) => {
                if (removedCard == source) {
                    source.Owner.TurnStarted -= setHasAttackedToFalse;
                    game.Attacked -= setHasAttackedToTrue;
                    game.DuelistEndAttackStep -= attack;
                }
            };
        }

        public static void TopCardOfDeckIntoManaZone(Card source, int numberOfCards = 1) {
            Duelist owner = source.Owner;
            for (int i = 0; i < numberOfCards; i++) {
                owner.ManaZone.Put(owner.DrawCard());
            }
        }

        public static DuelTask TopCardOfDeckIntoManaZoneTask(Card source, int numberOfCards = 1, bool optional = true) {
            string description = $"{source.Name}: Put the top ";
            description += numberOfCards > 1 ? $"{numberOfCards} cards" : "card";
            description += " of your deck into the mana zone.";
            return new DuelTask(
                description,
                (args) => TopCardOfDeckIntoManaZone(source, numberOfCards),
                optional
            );
        }

        private static string GetTypeString(Type type) {
            return type.ToString().Split('.')[1].ToLower();
        }

    }
}
