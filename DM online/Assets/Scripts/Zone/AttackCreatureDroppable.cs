using UnityEngine.EventSystems;

public class AttackCreatureDroppable : Droppable {
    public CardNetworkData DefingCreature { get; private set; }

    private void Awake() {
        DefingCreature = GetComponent<CardNetworkData>();
    }

    public override void OnDrop(PointerEventData eventData) {
        CardNetworkData atkingCreature = GetCardNetworkData(eventData);
        if (atkingCreature != null) {
            if (atkingCreature.Data.CurrentZone == CardServerData.Zone.BATTLEZONE) {
                Player.LocalPlayer.CmdBattle(atkingCreature.Data.ID, DefingCreature.Data.ID, DefingCreature.Owner.gameObject);
            } else if (atkingCreature.Data.CurrentZone == CardServerData.Zone.HAND) {
                Player.LocalPlayer.CmdUseCard(atkingCreature.Data.ID);
            }
        }
    }

}
