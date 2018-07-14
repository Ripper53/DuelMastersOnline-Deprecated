
namespace DuelMasters.Cards {
    public sealed class MarineFlower : Creature {
        public override string OriginalName => "Marine Flower";

        public MarineFlower(Duelist owner) : base(owner) {
            ManaCost = 1;
            Power = 2000;
            Add(Civilization.WATER);
            Add(Race.CYBER_VIRUS);

            Blocker = true;
            CanAttack = (creature) => false;
        }

    }
}
