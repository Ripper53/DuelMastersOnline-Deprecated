
namespace DuelMasters.Cards {
    public sealed class HunterFish : Creature {
        public override string OriginalName => "Hunter Fish";

        public HunterFish(Duelist owner) : base(owner) {
            ManaCost = 2;
            Power = 3000;
            Add(Civilization.WATER);
            Add(Race.FISH);

            Blocker = true;
            CanAttack = (creature) => false;
        }

    }
}
