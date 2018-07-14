using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DuelMasters;

public class DeckAddableCard : MonoBehaviour, IPointerClickHandler {
    public Card Card { get; set; }
    public CardCollection Collection { get; set; }

    public GameObject CardInDeckPrefab { get; set; }
    private GameObject cardInDeck;

    public void OnPointerClick(PointerEventData eventData) {
        string name = Card.GetType().Name;
        if (Collection.CardsInDeck.FindAll((str) => str == name).Count < 4) {
            Collection.CardsInDeck.Add(name);
            cardInDeck = Instantiate(CardInDeckPrefab, Collection.DeckHolderPanel);
            DeckRemoveableCard removeableCard = cardInDeck.gameObject.AddComponent<DeckRemoveableCard>();
            removeableCard.Collection = Collection;
            removeableCard.Name = name;
            cardInDeck.GetComponentInChildren<Text>().text = Card.OriginalName;
            cardInDeck.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>("Cards/Artwork/" + Card.OriginalName);
        }
    }

}
