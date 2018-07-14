
namespace DuelMasters.Cards {
    public sealed class SenatineJadeTree : Creature {
        public override string OriginalName => "Senatine Jade Tree";

        public SenatineJadeTree(Duelist owner) : base(owner) {
            ManaCost = 3;
            Power = 4000;
            Add(Civilization.LIGHT);
            Add(Race.STARLIGHT_TREE);

            Blocker = true;
            CanAttackPlayer = (player) => false;
        }

    }
}
