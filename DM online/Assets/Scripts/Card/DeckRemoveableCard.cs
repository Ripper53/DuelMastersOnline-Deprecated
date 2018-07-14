using UnityEngine;
using UnityEngine.EventSystems;

public class DeckRemoveableCard : MonoBehaviour, IPointerClickHandler {
    public CardCollection Collection { get; set; }
    public string Name { get; set; }

    public void OnPointerClick(PointerEventData eventData) {
        Collection.CardsInDeck.Remove(Name);
        Destroy(gameObject);
    }

}
