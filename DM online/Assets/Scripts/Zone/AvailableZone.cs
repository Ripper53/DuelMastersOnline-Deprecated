using System.Collections.Generic;
using UnityEngine;

public class AvailableZone : MonoBehaviour {
    [SerializeField]
    private Zones localZones;
    [SerializeField]
    private List<Zones> zones;

    public Zones LocalZones => localZones;

    public Zones GetZones() {
        if (zones.Count > 0) {
            Zones zone = zones[0];
            zones.RemoveAt(0);
            return zone;
        }
        return null;
    }

    [System.Serializable]
    public class Zones {
        public Transform Deck, Hand, ShieldZone, ManaZone, BattleZone, Graveyard;
    }
}
