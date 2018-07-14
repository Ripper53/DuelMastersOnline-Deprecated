using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using DuelMasters;

/// <summary>
/// ONLY exists on the SERVER.
/// </summary>
public class DuelMastersGame : NetworkBehaviour {
    private int currentPlayerId;
    public int GetPlayerID() {
        currentPlayerId -= 1;
        return currentPlayerId;
    }

    private Game game;
    private List<Duelist> availableDuelists;

    private void Awake() {
        currentPlayerId = 0;
        duelReady = 0;
    }

    public Duelist GetAvailableDuelist() {
        if (availableDuelists.Count > 0) {
            Duelist duelist = availableDuelists[0];
            availableDuelists.RemoveAt(0);
            return duelist;
        }
        return null;
    }

    private int duelReady;
    public bool BeginDuel() {
        duelReady += 1;
        if (duelReady == 2) {
            DMNetworkManager dm = (DMNetworkManager)NetworkManager.singleton;
            Type[][] decks = new Type[dm.numPlayers][];
            for (int i = 0; i < decks.Length; i++) {
                string[] cards = dm[i].DeckHolder.Cards;
                decks[i] = new Type[cards.Length];
                for (int j = 0; j < decks[i].Length; j++) {
                    decks[i][j] = Type.GetType("DuelMasters.Cards." + cards[j]);
                }
            }
            game = new Game(decks[0], decks[1]);
            availableDuelists = new List<Duelist>(game.Duelists);

            game.Lost += (source, loser) => {
                foreach (Player player in dm.AllPlayers) {
                    if (player.Duelist == loser) {
                        player.UI.Enqueue(new Player.CardUI() {
                            ConnID = player.ID,
                            InstanceID = player.ID,
                            Target = (conn, netId) => player.TargetLoser(conn)
                        });
                    }
                }
                if (source.GameState == Game.State.COMPLETED && source.NumberOfDuelists > 0) {
                    foreach (Player player in dm.AllPlayers) {
                        if (player.Duelist == source[0]) {
                            player.UI.Enqueue(new Player.CardUI() {
                                ConnID = player.ID,
                                InstanceID = player.ID,
                                Target = (conn, netId) => player.TargetWinner(conn)
                            });
                        }
                    }
                }
            };

            foreach (Player player in dm.AllPlayers) {
                player.ID = GetPlayerID();
                player.Duelist = GetAvailableDuelist();

                player.Duelist.Game.StepChanged += (game, step) => player.UI.Enqueue(new Player.CardUI() {
                    ConnID = player.ID,
                    InstanceID = player.ID,
                    Target = (conn, netId) => player.TargetCurrentStep(conn, step, player.Duelist == game.CurrentDuelistTurn)
            });

                // Put Card
                player.Duelist.Deck.PutCard += (source, putCard) => player.UI.Enqueue(new Player.CardUI() {
                    ConnID = player.ID,
                    InstanceID = player.ID,
                    Target = (conn, netId) => player.TargetPutCardBackInDeck(conn, netId)
                });
                player.Duelist.Hand.PutCard += (source, putCard) => player.UI.Enqueue(new Player.CardUI() {
                    ConnID = player.ID,
                    InstanceID = player.ID,
                    Target = (conn, netId) => player.TargetPutCardInHand(conn, netId, CardServerData.GetCardData(putCard, CardServerData.Zone.HAND))
                });
                player.Duelist.ShieldZone.PutCard += (source, putCard) => player.UI.Enqueue(new Player.CardUI() {
                    ConnID = player.ID,
                    InstanceID = player.ID,
                    Target = (conn, netId) => player.TargetPutCardBackInShieldZone(conn, netId)
                });
                player.Duelist.ManaZone.PutCard += (source, putCard) => player.UI.Enqueue(new Player.CardUI() {
                    ConnID = player.ID,
                    InstanceID = player.ID,
                    Target = (conn, netId) => player.TargetPutCardInManaZone(conn, netId, CardServerData.GetCardData(putCard, CardServerData.Zone.MANAZONE))
                });
                player.Duelist.BattleZone.PutCard += (source, putCard) => player.UI.Enqueue(new Player.CardUI() {
                    ConnID = player.ID,
                    InstanceID = player.ID,
                    Target = (conn, netId) => player.TargetPutCardInBattleZone(conn, netId, CardServerData.GetCardData(putCard, CardServerData.Zone.BATTLEZONE))
                });
                player.Duelist.Graveyard.PutCard += (source, putCard) => player.UI.Enqueue(new Player.CardUI() {
                    ConnID = player.ID,
                    InstanceID = player.ID,
                    Target = (conn, netId) => player.TargetPutCardInGraveyard(conn, netId, CardServerData.GetCardData(putCard, CardServerData.Zone.GRAVEYARD))
                });

                // Remove Card
                player.Duelist.Deck.RemovedCard += (source, removedCard) => player.UI.Enqueue(new Player.CardUI() {
                    ConnID = player.ID,
                    InstanceID = player.ID,
                    Target = (conn, netId) => player.TargetRemoveCardBackInDeck(conn, netId)
                });
                player.Duelist.Hand.RemovedCard += (source, removedCard) => player.UI.Enqueue(new Player.CardUI() {
                    ConnID = player.ID,
                    InstanceID = player.ID,
                    Target = (conn, netId) => player.TargetRemoveCardInHand(conn, netId, removedCard.ID)
                });
                player.Duelist.ShieldZone.RemovedCard += (source, removedCard) => player.UI.Enqueue(new Player.CardUI() {
                    ConnID = player.ID,
                    InstanceID = player.ID,
                    Target = (conn, netId) => player.TargetRemoveCardBackInShieldZone(conn, netId)
                });
                player.Duelist.ManaZone.RemovedCard += (source, removedCard) => player.UI.Enqueue(new Player.CardUI() {
                    ConnID = player.ID,
                    InstanceID = player.ID,
                    Target = (conn, netId) => player.TargetRemoveCardInManaZone(conn, netId, removedCard.ID)
                });
                player.Duelist.BattleZone.RemovedCard += (source, removedCard) => player.UI.Enqueue(new Player.CardUI() {
                    ConnID = player.ID,
                    InstanceID = player.ID,
                    Target = (conn, netId) => player.TargetRemoveCardInBattleZone(conn, netId, removedCard.ID)
                });
                player.Duelist.Graveyard.RemovedCard += (source, removedCard) => player.UI.Enqueue(new Player.CardUI() {
                    ConnID = player.ID,
                    InstanceID = player.ID,
                    Target = (conn, netId) => player.TargetRemoveCardInGraveyard(conn, netId, removedCard.ID)
                });

                // Tasks
                player.Duelist.TaskList.AddedTask += (source, addedTask) => {
                    if (addedTask.SelectableArgs != null) {
                        // If task requires targets.
                        object[] args = addedTask.SelectableArgs();
                        if (args.Length > 0) {
                            if (args[0] is Card) {
                                player.UI.Enqueue(new Player.CardUI() {
                                    ConnID = player.ID,
                                    InstanceID = player.ID,
                                    Target = (conn, netId) => player.TargetAddCardTask(conn, addedTask.Description, addedTask.Optional)
                                });
                            } else if (args[0] is Type) {
                                // TO DO!
                            }
                        } else {
                            player.UI.Enqueue(new Player.CardUI() {
                                ConnID = player.ID,
                                InstanceID = player.ID,
                                Target = (conn, netId) => player.TargetAddCardTask(conn, addedTask.Description, addedTask.Optional)
                            });
                        }
                    } else {
                        // If task does not require targets.
                        player.UI.Enqueue(new Player.CardUI() {
                            ConnID = player.ID,
                            InstanceID = player.ID,
                            Target = (conn, netId) => player.TargetAddTask(conn, addedTask.Description, addedTask.Optional)
                        });
                    }
                };
                player.Duelist.TaskList.RemovedTask += (source, removedTask, index) => player.UI.Enqueue(new Player.CardUI() {
                    ConnID = player.ID,
                    InstanceID = player.ID,
                    Target = (conn, netId) => player.TargetRemoveTask(conn, index)
                });
            }
            foreach (Player player in dm.AllPlayers) {
                Duelist duelist = player.Duelist;
                foreach (Player otherPlayer in dm.AllPlayers) {
                    Duelist otherDuelist = otherPlayer.Duelist;

                    if (duelist != otherDuelist) {
                        otherDuelist.Deck.PutCard += (d, putCard) => player.UI.Enqueue(new Player.CardUI() {
                            ConnID = player.ID,
                            InstanceID = otherPlayer.ID,
                            Target = (conn, netId) => player.TargetPutCardBackInDeck(conn, netId)
                        });
                        otherDuelist.Hand.PutCard += (hand, putCard) => player.UI.Enqueue(new Player.CardUI() {
                            ConnID = player.ID,
                            InstanceID = otherPlayer.ID,
                            Target = (conn, netId) => player.TargetPutCardBackInHand(conn, netId)
                        });
                        otherDuelist.ShieldZone.PutCard += (sz, putCard) => player.UI.Enqueue(new Player.CardUI() {
                            ConnID = player.ID,
                            InstanceID = otherPlayer.ID,
                            Target = (conn, netId) => player.TargetPutCardBackInShieldZone(conn, netId)
                        });
                        otherDuelist.ManaZone.PutCard += (mz, putCard) => player.UI.Enqueue(new Player.CardUI() {
                            ConnID = player.ID,
                            InstanceID = otherPlayer.ID,
                            Target = (conn, netId) => player.TargetPutCardInManaZone(conn, netId, CardServerData.GetCardData(putCard, CardServerData.Zone.MANAZONE))
                        });
                        otherDuelist.BattleZone.PutCard += (bz, putCard) => player.UI.Enqueue(new Player.CardUI() {
                            ConnID = player.ID,
                            InstanceID = otherPlayer.ID,
                            Target = (conn, netId) => player.TargetPutCardInBattleZone(conn, netId, CardServerData.GetCardData(putCard, CardServerData.Zone.BATTLEZONE))
                        });
                        otherDuelist.Graveyard.PutCard += (g, putCard) => player.UI.Enqueue(new Player.CardUI() {
                            ConnID = player.ID,
                            InstanceID = otherPlayer.ID,
                            Target = (conn, netId) => player.TargetPutCardInGraveyard(conn, netId, CardServerData.GetCardData(putCard, CardServerData.Zone.GRAVEYARD))
                        });

                        otherDuelist.Deck.RemovedCard += (hand, removedCard) => player.UI.Enqueue(new Player.CardUI() {
                            ConnID = player.ID,
                            InstanceID = otherPlayer.ID,
                            Target = (conn, netId) => player.TargetRemoveCardBackInDeck(conn, netId)
                        });
                        otherDuelist.Hand.RemovedCard += (hand, removedCard) => player.UI.Enqueue(new Player.CardUI() {
                            ConnID = player.ID,
                            InstanceID = otherPlayer.ID,
                            Target = (conn, netId) => player.TargetRemoveCardBackInHand(conn, netId)
                        });
                        otherDuelist.ShieldZone.RemovedCard += (sz, removedCard) => player.UI.Enqueue(new Player.CardUI() {
                            ConnID = player.ID,
                            InstanceID = otherPlayer.ID,
                            Target = (conn, netId) => player.TargetRemoveCardBackInShieldZone(conn, netId)
                        });
                        otherDuelist.ManaZone.RemovedCard += (mz, removedCard) => player.UI.Enqueue(new Player.CardUI() {
                            ConnID = player.ID,
                            InstanceID = otherPlayer.ID,
                            Target = (conn, netId) => player.TargetRemoveCardInManaZone(conn, netId, removedCard.ID)
                        });
                        otherDuelist.BattleZone.RemovedCard += (bz, removedCard) => player.UI.Enqueue(new Player.CardUI() {
                            ConnID = player.ID,
                            InstanceID = otherPlayer.ID,
                            Target = (conn, netId) => player.TargetRemoveCardInBattleZone(conn, netId, removedCard.ID)
                        });
                        otherDuelist.Graveyard.RemovedCard += (g, removedCard) => player.UI.Enqueue(new Player.CardUI() {
                            ConnID = player.ID,
                            InstanceID = otherPlayer.ID,
                            Target = (conn, netId) => player.TargetRemoveCardInGraveyard(conn, netId, removedCard.ID)
                        });
                    }

                }
            }
            game.BeginDuel();
            return true;
        }
        return false;
    }

    /*public static Player FindPlayerWith(Duelist duelist) {
        foreach (Player player in ((DMNetworkManager)NetworkManager.singleton).AllPlayers) {
            if (player.duelist == duelist) {
                return player;
            }
        }
        return null;
    }*/

}
