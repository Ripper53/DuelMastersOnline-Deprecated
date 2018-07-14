using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using DuelMasters;
using System.Collections.Generic;
using System;
using System.IO;

public class Player : NetworkBehaviour {
    public static Player LocalPlayer { get; private set; }
    public int ID { get; set; }
    public Duelist Duelist { get; set; }
    [SerializeField]
    private CardNetworkData cardPrefab;
    [SerializeField]
    private Transform cardBackPrefab;
    [SerializeField]
    private GameObject optionalTaskPrefab;
    [SerializeField]
    private GameObject mandatoryTaskPrefab;
    [SerializeField]
    private SelectableID selectableIDPrefab;

    private GameObject gameOverPanel;

    private static List<CardNetworkData> allCards;

    [SyncVar(hook = "UpdateReadyUI")]
    private bool ready;
    private void UpdateReadyUI(bool ready) {
        if (readyText != null) {
            
            if (ready) {
                readyText.text = "Ready!";
                deckDropdown.interactable = false;
            } else {
                readyText.text = "Not Ready";
                deckDropdown.interactable = true;
            }
        }
    }
    public DeckHolder DeckHolder;

    #region Card UI
    public class CardUI {
        public int ConnID, InstanceID;
        public Action<NetworkConnection, NetworkInstanceId> Target;
    }
    public Queue<CardUI> UI { get; private set; }
    [ServerCallback]
    private void Awake() {
        UI = new Queue<CardUI>();
    }
    [ServerCallback]
    private void Update() {
        if (UI.Count > 0) {
            CardUI cardUI = UI.Dequeue();
            cardUI.Target(FindPlayer(cardUI.ConnID).connectionToClient, FindPlayer(cardUI.InstanceID).netId);
        }
    }
    public static Player FindPlayer(int playerId) {
        DMNetworkManager dm = (DMNetworkManager)NetworkManager.singleton;
        foreach (Player player in dm.AllPlayers) {
            if (player.ID == playerId)
                return player;
        }
        return null;
    }
    #endregion

    public Transform Deck { get; private set; }
    public Transform Hand { get; private set; }
    public Transform ShieldZone { get; private set; }
    public Transform ManaZone { get; private set; }
    public Transform BattleZone { get; private set; }
    public Transform Graveyard { get; private set; }

    // Game
    private Button stepButton;
    private Text stepText;
    private Transform taskPanel;
    public CardView View { get; private set; }
    public class CardView {
        public Image Artwork { get; private set; }
        public Text Name { get; private set; }
        public Text Description { get; private set; }

        public CardView(Image artwork, Text name, Text description) {
            Artwork = artwork;
            Name = name;
            Description = description;
        }

        public void SetActive(bool value) {
            Artwork.gameObject.SetActive(value);
        }
    }
    private Button finishedButton;

    // Lobby
    private Dropdown deckDropdown;
    private Text readyText;

    public void Init() {
        if (NetworkManager.networkSceneName == "Game") {
            selectableToDestroy = new List<SelectableID>();
            AvailableZone availableZone = FindObjectOfType<AvailableZone>();
            if (isLocalPlayer) {
                LocalPlayer = this;
                allCards = new List<CardNetworkData>();
                SelectedCards = new List<CardNetworkData>();
                stepButton = GameObject.FindGameObjectWithTag("NextStep").GetComponent<Button>();
                stepButton.onClick.AddListener(NextStep);
                stepText = stepButton.GetComponentInChildren<Text>();
                taskPanel = GameObject.FindGameObjectWithTag("TaskPanel").transform;
                GameObject cardView = GameObject.FindGameObjectWithTag("CardView");
                Text[] cardTexts = cardView.GetComponentsInChildren<Text>();
                View = new CardView(cardView.GetComponent<Image>(), cardTexts[0], cardTexts[1]);
                cardTexts[1].transform.parent.gameObject.SetActive(false); // TO REMOVE!
                finishedButton = GameObject.FindGameObjectWithTag("FinishedButton").GetComponent<Button>();
                finishedButton.onClick.AddListener(FinishedSelection);
                gameOverPanel = GameObject.FindGameObjectWithTag("GameOver");

                View.SetActive(false);
                finishedButton.gameObject.SetActive(false);
                taskPanel.parent.gameObject.SetActive(false);
                gameOverPanel.SetActive(false);

                AvailableZone.Zones zones = availableZone.LocalZones;
                Deck = zones.Deck;
                Hand = zones.Hand;
                ShieldZone = zones.ShieldZone;
                ManaZone = zones.ManaZone;
                BattleZone = zones.BattleZone;
                Graveyard = zones.Graveyard;
                CmdBeginDuel();
            } else {
                AvailableZone.Zones zones = availableZone.GetZones();
                Deck = zones.Deck;
                Hand = zones.Hand;
                ShieldZone = zones.ShieldZone;
                ManaZone = zones.ManaZone;
                BattleZone = zones.BattleZone;
                Graveyard = zones.Graveyard;

                ShieldZone.parent.gameObject.AddComponent<AttackPlayerDroppable>().Owner = this;
            }
        } else if (NetworkManager.networkSceneName == "Lobby") {
            if (isLocalPlayer) {
                LocalPlayer = this;
                readyText = GameObject.FindGameObjectWithTag("ReadyButton").GetComponentInChildren<Text>();
                readyText.GetComponentInParent<Button>().onClick.AddListener(() => CmdSetReady(!ready, DeckHolder.LoadDeck(deckDropdown.options[deckDropdown.value].text).Cards));

                deckDropdown = GameObject.FindGameObjectWithTag("DeckDropdown").GetComponent<Dropdown>();
                deckDropdown.ClearOptions();
                Directory.CreateDirectory(DeckHolder.DeckPath);
                string[] deckPaths = Directory.GetFiles(DeckHolder.DeckPath);
                string[] deckNames = new string[deckPaths.Length];
                for (int i = 0; i < deckNames.Length; i++) {
                    deckNames[i] = Path.GetFileNameWithoutExtension(deckPaths[i]);
                }
                List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>(deckNames.Length);
                foreach (string deckName in deckNames) {
                    optionDatas.Add(new Dropdown.OptionData(deckName));
                }
                deckDropdown.AddOptions(optionDatas);
            }
        }
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
        ready = false;
        taskObjs = new List<GameObject>();
        UI = new Queue<CardUI>();
        Init();
    }
    [Command]
    private void CmdSetReady(bool value, string[] cards) {
        DeckHolder = new DeckHolder(cards);
        ready = value;
        if (ready) {
            DMNetworkManager dm = (DMNetworkManager)NetworkManager.singleton;
            if (dm.numPlayers > 1) {
                foreach (Player player in dm.AllPlayers) {
                    if (!player.ready)
                        return;
                }
                dm.ServerChangeScene("Game");
            }
        }
    }
    [Command]
    private void CmdBeginDuel() {
        DuelMastersGame game = FindObjectOfType<DuelMastersGame>();
        if (game != null && game.BeginDuel()) {
            foreach (Duelist duelist in Duelist.Game.Duelists) {
                foreach (Card card in duelist.AllCards) {
                    card.Tap += (source, tap) => UI.Enqueue(new CardUI() {
                        ConnID = ID,
                        InstanceID = ID,
                        Target = (conn, netId) => RpcTap(source.ID, tap)
                    });
                }
            }
        }
    }

    public void NextStep() {
        CmdNextStep();
    }
    [Command]
    public void CmdNextStep() {
        if (Duelist.Game.CurrentDuelistTurn == Duelist) {
            Duelist.Game.NextStep();
        }
    }

    [Command]
    public void CmdForceUseCard(int cardId) {
        Card card = Duelist.Hand.Search<Card>(cardId);
        if (card != null) card.Use();
    }

    [Command]
    public void CmdUseCard(int cardId) {
        Card card = Duelist.Hand.Search<Card>(cardId);
        if (card != null && Duelist.DuelAction.ActionCheck(card, Game.Step.MAIN)) {
            Duelist.TaskList.AddTask(new DuelTask(
                $"Select mana for {card.Name}.",
                (args) => Duelist.DuelAction.UseCard(card, (Card[])args),
                true,
                () => Duelist.ManaZone.GetAll<Card>((c) => !c.Tapped)
            ));
        }
    }

    [Command]
    public void CmdChargeCard(int cardId) {
        Card card = Duelist.Hand.Search<Card>(cardId);
        if (card != null) Duelist.DuelAction.ChargeCard(card);
    }

    [Command]
    public void CmdBattle(int atkingCreatureId, int defingCreatureId, GameObject defingPlayer) {
        if (defingPlayer == null) return;
        Player defingP = defingPlayer.GetComponent<Player>();
        if (defingP == null) return;
        Creature atkingCreature = Duelist.BattleZone.Search<Creature>(atkingCreatureId);
        Creature defingCreature = defingP.Duelist.BattleZone.Search<Creature>(defingCreatureId);
        if (atkingCreature != null && defingCreature != null) {
            Duelist.Game.Battle(atkingCreature, defingCreature);
        }
    }

    [Command]
    public void CmdAttackPlayer(GameObject defPlayer, int atkingId) {
        if (defPlayer != null) {
            Player p = defPlayer.GetComponent<Player>();
            if (p != null) {
                Creature atkingCreature = Duelist.BattleZone.Search<Creature>(atkingId);
                if (atkingCreature != null) {
                    p.Duelist.Game.AttackPlayer(atkingCreature, p.Duelist);
                }
            }
        }
    }

    [TargetRpc]
    public void TargetCurrentStep(NetworkConnection target, Game.Step step, bool isTurn) {
        stepButton.interactable = isTurn;
        stepText.text = step.ToString();
    }

    private List<GameObject> taskObjs;

    // TASK
    [TargetRpc]
    public void TargetAddTask(NetworkConnection target, string description, bool optional) {
        taskPanel.parent.gameObject.SetActive(true);
        if (optional) {
            GameObject taskObj = Instantiate(optionalTaskPrefab, taskPanel);
            taskObjs.Add(taskObj);
            taskObj.GetComponentInChildren<Text>().text = description;
            Button[] buttons = taskObj.GetComponentsInChildren<Button>();
            buttons[0].onClick.AddListener(() => CompleteTask(taskObj));
            buttons[1].onClick.AddListener(() => RemoveTask(taskObj));
        } else {
            GameObject taskObj = Instantiate(mandatoryTaskPrefab, taskPanel);
            taskObjs.Add(taskObj);
            taskObj.GetComponentInChildren<Text>().text = description;
            taskObj.GetComponentInChildren<Button>().onClick.AddListener(() => CompleteTask(taskObj));
        }
    }
    [TargetRpc]
    public void TargetAddCardTask(NetworkConnection target, string description, bool optional) {
        taskPanel.parent.gameObject.SetActive(true);
        if (optional) {
            GameObject taskObj = Instantiate(optionalTaskPrefab, taskPanel);
            taskObjs.Add(taskObj);
            taskObj.GetComponentInChildren<Text>().text = description;
            Button[] buttons = taskObj.GetComponentsInChildren<Button>();
            buttons[0].onClick.AddListener(() => ChooseCard(taskObj));
            buttons[1].onClick.AddListener(() => RemoveTask(taskObj));
        } else {
            GameObject taskObj = Instantiate(mandatoryTaskPrefab, taskPanel);
            taskObjs.Add(taskObj);
            taskObj.GetComponentInChildren<Text>().text = description;
            taskObj.GetComponentInChildren<Button>().onClick.AddListener(() => ChooseCard(taskObj));
        }
    }
    [TargetRpc]
    public void TargetRemoveTask(NetworkConnection target, int index) {
        GameObject taskObj = taskObjs[index];
        taskObjs.RemoveAt(index);
        Destroy(taskObj);
        if (taskObjs.Count <= 0) {
            taskPanel.parent.gameObject.SetActive(false);
            ClearTask();
        }
    }
    [ClientRpc]
    public void RpcTap(int cardId, bool tapped) {
        foreach (CardNetworkData card in allCards) {
            if (cardId == card.Data.ID) {
                if (tapped) {
                    card.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
                } else {
                    if (card.Data.CurrentZone != CardServerData.Zone.MANAZONE)
                        card.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    else
                        card.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
                }
            }
        }
    }
    private void ClearTask() {
        foreach (Transform tran in taskPanel) {
            Destroy(tran.gameObject);
        }
    }

    private GameObject selectedTaskObj;
    public List<CardNetworkData> SelectedCards { get; private set; }

    public void ChooseCard(GameObject taskObj) {
        selectedTaskObj = taskObj;
        CmdChooseCard(gameObject, taskObjs.FindIndex((obj) => obj == taskObj));
    }
    [Command]
    private void CmdChooseCard(GameObject player, int index) {
        if (player != null) {
            Player p = player.GetComponent<Player>();
            if (p != null) {
                Game game = p.Duelist.Game;
                if (p.Duelist != game.CurrentDuelistTurn && game.CurrentDuelistDoingTask()) {
                    return;
                } else {
                    DuelTask task = Duelist.TaskList[index];
                    if (task.SelectableArgs != null) {
                        Card[] args = (Card[])task.SelectableArgs();
                        CardServerData[] argData = new CardServerData[args.Length];
                        for (int i = 0; i < args.Length; i++) {
                            argData[i] = CardServerData.GetCardData(args[i], CardServerData.Zone.GRAVEYARD);
                        }
                        TargetChooseCard(p.connectionToClient, index, argData);
                    }
                }
            }
        }
    }
    private List<SelectableID> selectableToDestroy;
    [TargetRpc]
    private void TargetChooseCard(NetworkConnection target, int index, CardServerData[] cardData) {
        SelectedCards.Clear();
        foreach (GameObject obj in taskObjs)
            obj.SetActive(false);
        finishedButton.gameObject.SetActive(true);
        foreach (CardServerData data in cardData) {
            SelectableID selectable = Instantiate(selectableIDPrefab, taskPanel);
            selectable.CardData.Data = data;
            Sprite artwork = Resources.Load<Sprite>("Cards/Artwork/" + data.OriginalName);
            selectable.CardData.Artwork.sprite = artwork;
            selectable.GetComponent<Canvas>().sortingOrder = 5;
            selectableToDestroy.Add(selectable);
        }
    }

    public void FinishedSelection() {
        finishedButton.gameObject.SetActive(false);
        foreach (SelectableID selectable in selectableToDestroy) {
            Destroy(selectable.gameObject);
        }
        selectableToDestroy.Clear();
        foreach (GameObject obj in taskObjs)
            obj.SetActive(true);
        int index = taskObjs.FindIndex((obj) => obj == selectedTaskObj);
        int[] cardIds = new int[SelectedCards.Count];
        for (int i = 0; i < cardIds.Length; i++)
            cardIds[i] = SelectedCards[i].Data.ID;
        CmdCompleteCardTask(index, cardIds);
    }

    public void CompleteTask(GameObject taskObj) {
        int index = taskObjs.FindIndex((obj) => obj == taskObj);
        CmdCompleteTask(index);
    }

    public void RemoveTask(GameObject taskObj) {
        int index = taskObjs.FindIndex((obj) => obj == taskObj);
        CmdRemoveTask(index);
    }

    [Command]
    public void CmdCompleteTask(int index) {
        if (index >= 0 && index < Duelist.TaskList.NumberOfTasks) {
            Duelist.TaskList.CompleteTask(index, new object[] { });
        }
    }
    [Command]
    public void CmdCompleteCardTask(int index, int[] cardIds) {
        if (index >= 0 && index < Duelist.TaskList.NumberOfTasks) {
            Card[] args = new Card[cardIds.Length];
            for (int i = 0; i < args.Length; i++)
                args[i] = Duelist.Game.SearchID(cardIds[i]);
            Duelist.TaskList.CompleteTask(index, args);
        }
    }

    [Command]
    public void CmdRemoveTask(int index) {
        if (index >= 0 && index < Duelist.TaskList.NumberOfTasks) {
            Duelist.TaskList.RemoveTask(index);
        }
    }

    // DECK
    // Face Down
    [TargetRpc]
    public void TargetPutCardBackInDeck(NetworkConnection target, NetworkInstanceId playerId) {
        Player player = ClientScene.FindLocalObject(playerId).GetComponent<Player>();
        Instantiate(cardBackPrefab, player.Deck);
    }
    [TargetRpc]
    public void TargetRemoveCardBackInDeck(NetworkConnection target, NetworkInstanceId playerId) {
        Player player = ClientScene.FindLocalObject(playerId).GetComponent<Player>();
        foreach (Transform tran in player.Deck) {
            if (tran.GetComponent<CardNetworkData>() == null) {
                Destroy(tran.gameObject);
                return;
            }
        }
    }

    // HAND
    // Face Up
    [TargetRpc]
    public void TargetPutCardInHand(NetworkConnection target, NetworkInstanceId playerId, CardServerData cardData) {
        Player player = ClientScene.FindLocalObject(playerId).GetComponent<Player>();
        CardNetworkData card = Instantiate(cardPrefab, player.Hand);
        allCards.Add(card);
        Sprite artwork = Resources.Load<Sprite>("Cards/Artwork/" + cardData.OriginalName);
        card.Artwork.sprite = artwork;
        card.Data = cardData;
        card.Owner = player;
        card.gameObject.AddComponent<Draggable>();
    }
    [TargetRpc]
    public void TargetRemoveCardInHand(NetworkConnection target, NetworkInstanceId playerId, int cardId) {
        Player player = ClientScene.FindLocalObject(playerId).GetComponent<Player>();
        foreach (Transform tran in player.Hand) {
            CardNetworkData cardData = tran.GetComponent<CardNetworkData>();
            if (cardData != null && cardData.Data.ID == cardId) {
                allCards.Remove(cardData);
                Destroy(tran.gameObject);
                return;
            }
        }
    }
    // Face Down
    [TargetRpc]
    public void TargetPutCardBackInHand(NetworkConnection target, NetworkInstanceId playerId) {
        Player player = ClientScene.FindLocalObject(playerId).GetComponent<Player>();
        Instantiate(cardBackPrefab, player.Hand);
    }
    [TargetRpc]
    public void TargetRemoveCardBackInHand(NetworkConnection target, NetworkInstanceId playerId) {
        Player player = ClientScene.FindLocalObject(playerId).GetComponent<Player>();
        foreach (Transform tran in player.Hand) {
            if (tran.GetComponent<CardNetworkData>() == null) {
                Destroy(tran.gameObject);
                return;
            }
        }
    }

    // SHIELD ZONE
    // Face Up
    [TargetRpc]
    public void TargetPutCardInShieldZone(NetworkConnection target, NetworkInstanceId playerId, CardServerData cardData) {
        Player player = ClientScene.FindLocalObject(playerId).GetComponent<Player>();
        CardNetworkData card = Instantiate(cardPrefab, player.ShieldZone);
        allCards.Add(card);
        Sprite artwork = Resources.Load<Sprite>("Cards/Artwork/" + cardData.OriginalName);
        card.Artwork.sprite = artwork;
        card.Data = cardData;
        card.Owner = player;
    }
    [TargetRpc]
    public void TargetRemoveCardInShieldZone(NetworkConnection target, NetworkInstanceId playerId, int cardId) {
        Player player = ClientScene.FindLocalObject(playerId).GetComponent<Player>();
        foreach (Transform tran in player.ShieldZone) {
            CardNetworkData cardData = tran.GetComponent<CardNetworkData>();
            if (cardData != null && cardData.Data.ID == cardId) {
                allCards.Remove(cardData);
                Destroy(tran.gameObject);
                return;
            }
        }
    }
    // Face Down
    [TargetRpc]
    public void TargetPutCardBackInShieldZone(NetworkConnection target, NetworkInstanceId playerId) {
        Player player = ClientScene.FindLocalObject(playerId).GetComponent<Player>();
        Instantiate(cardBackPrefab, player.ShieldZone);
    }
    [TargetRpc]
    public void TargetRemoveCardBackInShieldZone(NetworkConnection target, NetworkInstanceId playerId) {
        Player player = ClientScene.FindLocalObject(playerId).GetComponent<Player>();
        foreach (Transform tran in player.ShieldZone) {
            if (tran.GetComponent<CardNetworkData>() == null) {
                Destroy(tran.gameObject);
                return;
            }
        }
    }

    // MANA ZONE
    [TargetRpc]
    public void TargetPutCardInManaZone(NetworkConnection target, NetworkInstanceId playerId, CardServerData cardData) {
        Player player = ClientScene.FindLocalObject(playerId).GetComponent<Player>();
        CardNetworkData card = Instantiate(cardPrefab, player.ManaZone);
        allCards.Add(card);
        Sprite artwork = Resources.Load<Sprite>("Cards/Artwork/" + cardData.OriginalName);
        card.Artwork.sprite = artwork;
        card.Data = cardData;
        card.Owner = player;
        card.Artwork.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
    }
    [TargetRpc]
    public void TargetRemoveCardInManaZone(NetworkConnection target, NetworkInstanceId playerId, int cardId) {
        Player player = ClientScene.FindLocalObject(playerId).GetComponent<Player>();
        foreach (Transform tran in player.ManaZone) {
            CardNetworkData cardData = tran.GetComponent<CardNetworkData>();
            if (cardData != null && cardData.Data.ID == cardId) {
                allCards.Remove(cardData);
                Destroy(tran.gameObject);
                return;
            }
        }
    }

    // BATTLE ZONE
    [TargetRpc]
    public void TargetPutCardInBattleZone(NetworkConnection target, NetworkInstanceId playerId, CardServerData cardData) {
        Player player = ClientScene.FindLocalObject(playerId).GetComponent<Player>();
        CardNetworkData card = Instantiate(cardPrefab, player.BattleZone);
        allCards.Add(card);
        Sprite artwork = Resources.Load<Sprite>("Cards/Artwork/" + cardData.OriginalName);
        card.Artwork.sprite = artwork;
        card.Data = cardData;
        card.Owner = player;
        card.gameObject.AddComponent<AttackCreatureDroppable>();
        if (player.isLocalPlayer)
            card.gameObject.AddComponent<AttackDraggable>();
    }
    [TargetRpc]
    public void TargetRemoveCardInBattleZone(NetworkConnection target, NetworkInstanceId playerId, int cardId) {
        Player player = ClientScene.FindLocalObject(playerId).GetComponent<Player>();
        foreach (Transform tran in player.BattleZone) {
            CardNetworkData cardData = tran.GetComponent<CardNetworkData>();
            if (cardData != null && cardData.Data.ID == cardId) {
                allCards.Remove(cardData);
                Destroy(tran.gameObject);
                return;
            }
        }
    }

    // GRAVEYARD
    [TargetRpc]
    public void TargetPutCardInGraveyard(NetworkConnection target, NetworkInstanceId playerId, CardServerData cardData) {
        Player player = ClientScene.FindLocalObject(playerId).GetComponent<Player>();
        CardNetworkData card = Instantiate(cardPrefab, player.Graveyard);
        allCards.Add(card);
        Sprite artwork = Resources.Load<Sprite>("Cards/Artwork/" + cardData.OriginalName);
        card.Artwork.sprite = artwork;
        card.Data = cardData;
        card.Owner = player;
    }
    [TargetRpc]
    public void TargetRemoveCardInGraveyard(NetworkConnection target, NetworkInstanceId playerId, int cardId) {
        Player player = ClientScene.FindLocalObject(playerId).GetComponent<Player>();
        foreach (Transform tran in player.Graveyard) {
            CardNetworkData cardData = tran.GetComponent<CardNetworkData>();
            if (cardData != null && cardData.Data.ID == cardId) {
                allCards.Remove(cardData);
                Destroy(tran.gameObject);
                return;
            }
        }
    }

    [TargetRpc]
    public void TargetWinner(NetworkConnection target) {
        gameOverPanel.GetComponentInChildren<Text>().text = "You Won!";
        gameOverPanel.SetActive(true);
    }

    [TargetRpc]
    public void TargetLoser(NetworkConnection target) {
        gameOverPanel.GetComponentInChildren<Text>().text = "You Lost...";
        gameOverPanel.SetActive(true);
    }

}
