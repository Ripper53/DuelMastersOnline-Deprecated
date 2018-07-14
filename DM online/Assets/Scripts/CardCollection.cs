using System.Linq;
using System.Reflection;
using System;
using UnityEngine;
using DuelMasters;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class CardCollection : MonoBehaviour {
    private Type[] allCards;
    private DeckHolder deckHolder;
    public List<string> CardsInDeck { get; private set; }
    private int page, cardsPerPage;

    private Game fakeGame;

    [SerializeField]
    private CardNetworkData cardPrefab;
    [SerializeField]
    private Button deckHolderPrefab;
    public Transform DeckHolderPanel;
    [SerializeField]
    private InputField deckNameInputField;
    [SerializeField]
    private GameObject cardInDeckPrefab;

    [SerializeField]
    private Button newButton;
    [SerializeField]
    private Button saveButton;
    [SerializeField]
    private Button backButton;
    [SerializeField]
    private Button exitButton;

    private void Awake() {
        page = 0;
        cardsPerPage = 10;
        CardsInDeck = new List<string>();

        deckNameInputField = GameObject.FindGameObjectWithTag("DeckName").GetComponent<InputField>();

        string nspace = "DuelMasters.Cards";

        var cards = from t in Assembly.GetExecutingAssembly().GetTypes()
                where t.IsClass && t.Namespace == nspace
                select t;
        List<Type> cardList = cards.ToList();
        List<Type> toRemove = new List<Type>();
        foreach (Type type in cardList) {
            if (!type.IsSubclassOf(typeof(Card))) {
                toRemove.Add(type);
            }
        }
        foreach (Type type in toRemove)
            cardList.Remove(type);
        allCards = cardList.ToArray();

        fakeGame = new Game(new Type[] { }, new Type[] { });
    }

    private void Start() {
        LoadAllDecks();
        LoadPage();
    }

    public void NextPage() {
        if ((page * cardsPerPage) + cardsPerPage < allCards.Length) {
            page += 1;
            LoadPage();
        }
    }

    public void PreviousPage() {
        if (page > 0) {
            page -= 1;
            LoadPage();
        }
    }

    public void LoadPage() {
        ClearCardsPanel();
        for (int i = 0, pageIndex = i + (page * cardsPerPage); i < cardsPerPage && pageIndex < allCards.Length; i++, pageIndex++) {
            Type cardType = allCards[pageIndex];
            CardNetworkData cardData = Instantiate(cardPrefab, transform);
            Card card = (Card)Activator.CreateInstance(cardType, fakeGame.CurrentDuelistTurn);
            cardData.Artwork.sprite = Resources.Load<Sprite>("Cards/Artwork/" + card.OriginalName);
            cardData.Data = CardServerData.GetCardData(card, CardServerData.Zone.GRAVEYARD);
            if (deckHolder != null) {
                DeckAddableCard addableCard = cardData.gameObject.AddComponent<DeckAddableCard>();
                addableCard.Collection = this;
                addableCard.Card = card;
                addableCard.CardInDeckPrefab = cardInDeckPrefab;
            }
        }
    }

    private void LoadDeckHolder(string deckName) {
        deckHolder = null;
        Button deck = Instantiate(deckHolderPrefab, DeckHolderPanel);
        deck.GetComponentsInChildren<Button>()[1].onClick.AddListener(() => {
            DeleteDeck(deckName);
            Destroy(deck.gameObject);
        });
        deck.GetComponentInChildren<Text>().text = deckName;
        deck.onClick.AddListener(() => LoadDeck(deckName));
    }

    public void SaveDeck() {
        deckHolder.Cards = CardsInDeck.ToArray();
        DeckHolder.SaveDeck(deckHolder, deckNameInputField.text);
        LoadAllDecks();
        LoadPage();
    }

    public void NewDeck() {
        CardsInDeck.Clear();
        deckHolder = new DeckHolder(new string[] { });
        deckNameInputField.text = "Untitled";
        ClearDeckHolderPanel();
        LoadPage();
        SetViewDeck();
    }

    public void LoadDeck(string deckName) {
        ClearDeckHolderPanel();
        deckHolder = DeckHolder.LoadDeck(deckName);
        deckNameInputField.text = deckName;
        CardsInDeck.Clear();
        CardsInDeck.AddRange(deckHolder.Cards);
        for (int i = 0; i < deckHolder.Cards.Length; i++) {
            Card card = (Card)Activator.CreateInstance(Type.GetType("DuelMasters.Cards." + deckHolder.Cards[i]), fakeGame.CurrentDuelistTurn);
            GameObject cardInDeck = Instantiate(cardInDeckPrefab, DeckHolderPanel);
            cardInDeck.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>("Cards/Artwork/" + card.OriginalName);
            cardInDeck.GetComponentInChildren<Text>().text = card.OriginalName;
            DeckRemoveableCard removeableCard = cardInDeck.AddComponent<DeckRemoveableCard>();
            removeableCard.Collection = this;
            removeableCard.Name = card.GetType().Name;
        }
        LoadPage();
        SetViewDeck();
    }

    public void DeleteDeck(string deckName) {
        DeckHolder.DeleteDeck(deckName);
    }

    public void LoadAllDecks() {
        ClearDeckHolderPanel();
        Directory.CreateDirectory(DeckHolder.DeckPath);
        foreach (string deckName in Directory.GetFiles(DeckHolder.DeckPath)) {
            LoadDeckHolder(Path.GetFileNameWithoutExtension(deckName));
        }
        LoadPage();
        SetViewDeckHolders();
    }

    private void SetViewDeck() {
        newButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        saveButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
    }
    private void SetViewDeckHolders() {
        saveButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        newButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
    }

    public void ClearCardsPanel() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }

    public void ClearDeckHolderPanel() {
        foreach (Transform child in DeckHolderPanel) {
            Destroy(child.gameObject);
        }
    }

}
