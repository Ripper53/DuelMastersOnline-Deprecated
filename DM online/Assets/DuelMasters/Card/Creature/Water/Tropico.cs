
namespace DuelMasters.Cards {
    public sealed class Tropico : Creature {
        public override string OriginalName => "Tropico";

        public Tropico(Duelist owner) : base(owner) {
            ManaCost = 5;
            Power = 3000;
            Add(Civilization.WATER);
            Add(Race.CYBER_LORD);

            Blockable = (blocker) => {
                // While 2 other Creatures are in the Battle Zone, this Creature cannot be blocked.
                if (Owner.BattleZone.GetAll<Creature>().Length > 2)
                    return false;
                else
                    return true;
            };
        }

    }
}
