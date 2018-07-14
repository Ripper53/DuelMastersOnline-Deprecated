using UnityEngine.EventSystems;

public class ChargeCardDroppable : Droppable {

    public override void OnDrop(PointerEventData eventData) {
        CardNetworkData card = GetCardNetworkData(eventData);
        if (card != null) {
            card.Owner.CmdChargeCard(card.Data.ID);
        }
    }

}
