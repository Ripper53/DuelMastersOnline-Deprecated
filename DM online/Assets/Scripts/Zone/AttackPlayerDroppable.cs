using UnityEngine.EventSystems;

public class AttackPlayerDroppable : Droppable {
    public Player Owner { get; set; }

    public override void OnDrop(PointerEventData eventData) {
        CardNetworkData card = GetCardNetworkData(eventData);
        if (card != null) {
            card.Owner.CmdAttackPlayer(Owner.gameObject, card.Data.ID);
        }
    }

}
