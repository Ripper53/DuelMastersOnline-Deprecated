using System.Collections.Generic;

namespace DuelMasters {
    public class CreatureData : CardData {
        public int ShieldBreaker { get; set; }
        public int Power { get; set; }
        // List of Races because a Creature can have more than one.
        public List<Race> Races { get; private set; }

        public CreatureData() {
            Races = new List<Race>();
        }

    }

    // All Race types.
    public enum Race {
        ANGEL_COMMAND,
        ARMORED_DRAGON,
        ARMORED_WYVERN,
        ARMORLOID,
        BALLOON_MUSHROOM,
        BEAST_FOLK,
        BERSERKER,
        BRAIN_JACKER,
        CHIMERA,
        COLONY_BEETLE,
        CYBER_LORD,
        CYBER_VIRUS,
        DARK_LORD,
        DEMON_COMMAND,
        DRAGONOID,
        FISH,
        GEL_FISH,
        GHOST,
        GIANT_INSECT,
        GUARDIAN,
        HORNED_BEAST,
        HUMAN,
        INITIATE,
        LEVIATHAN,
        LIGHT_BRINGER,
        LIQUID_PEOPLE,
        LIVING_DEAD,
        MACHINE_EATER,
        PARASITE_WORM,
        ROCK_BEAST,
        STARLIGHT_TREE,
        TREE_FOLK
    }
}
