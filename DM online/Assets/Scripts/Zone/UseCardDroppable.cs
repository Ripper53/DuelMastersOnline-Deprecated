using UnityEngine.EventSystems;

public class UseCardDroppable : Droppable {

    public override void OnDrop(PointerEventData eventData) {
        CardNetworkData card = GetCardNetworkData(eventData);
        if (card != null && card.Data.CurrentZone == CardServerData.Zone.HAND) {
            Player.LocalPlayer.CmdUseCard(card.Data.ID);
        }
    }

}
