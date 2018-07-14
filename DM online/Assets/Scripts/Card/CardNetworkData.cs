using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DuelMasters;

public class CardNetworkData : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public CardServerData Data;
    public Player Owner { get; set; }
    public Image Artwork { get; private set; }

    private void Awake() {
        Artwork = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (Player.LocalPlayer != null) {
            string civs = "Civilization: ";
            foreach (Civilization civ in Data.Civilizations) {
                civs += civ + ", ";
            }
            civs = civs.TrimEnd(' ', ',') + '.';

            Player.LocalPlayer.View.Artwork.sprite = Artwork.sprite;
            Player.LocalPlayer.View.Name.text = Data.Name;
            Player.LocalPlayer.View.Description.text = civs;
            Player.LocalPlayer.View.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (Player.LocalPlayer != null) {
            Player.LocalPlayer.View.SetActive(false);
        }
    }

}
