using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableID : MonoBehaviour, IPointerClickHandler {
    public CardNetworkData CardData { get; private set; }
    private bool selected;

    private void Awake() {
        selected = false;
        CardData = GetComponent<CardNetworkData>();
    }

    public void OnPointerClick(PointerEventData eventData) {
        selected = !selected;
        if (selected) {
            Player.LocalPlayer.SelectedCards.Add(CardData);
            CardData.Artwork.color = new Color(1f, 1f, 1f, 0.5f);
        } else {
            Player.LocalPlayer.SelectedCards.Remove(CardData);
            CardData.Artwork.color = new Color(1f, 1f, 1f, 1f);
        }
    }

}
