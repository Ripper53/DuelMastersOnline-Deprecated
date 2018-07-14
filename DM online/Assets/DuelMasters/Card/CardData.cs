using System.Collections.Generic;

namespace DuelMasters {
    /// <summary>
    /// Basic Card data such as Name, Mana Cost, etc...
    /// </summary>
    public abstract class CardData {
        public string Name { get; set; }
        public int ManaCost { get; set; }
        public int ManaNumber { get; set; }
        // List of Civilizations because a Card can have more than one.
        public List<Civilization> Civilizations { get; private set; }

        public CardData() {
            Civilizations = new List<Civilization>();
        }

    }

    // All Civilization types.
    public enum Civilization {
        NATURE,
        FIRE,
        WATER,
        LIGHT,
        DARKNESS
    }
}
