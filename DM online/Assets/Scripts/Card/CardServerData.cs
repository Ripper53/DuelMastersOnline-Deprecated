using DuelMasters;

public struct CardServerData {
    public enum Zone { DECK, HAND, SHIELDZONE, MANAZONE, BATTLEZONE, GRAVEYARD };
    public Zone CurrentZone;
    public int ID;
    public string OriginalName, Name;
    public int ManaCost;
    public int ManaNumber;
    public Civilization[] Civilizations;

    public static CardServerData GetCardData(Card card, Zone currentZone) {
        return new CardServerData() {
            ID = card.ID,
            OriginalName = card.OriginalName,
            Name = card.Name,
            ManaCost = card.ManaCost,
            ManaNumber = card.ManaNumber,
            Civilizations = card.GetCivilizations(),
            CurrentZone = currentZone
        };
    }

}
