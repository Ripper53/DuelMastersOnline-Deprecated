
namespace DuelMasters.Cards {
    public sealed class PoisonousDahlia : Creature {
        public override string OriginalName => "Poisonous Dahlia";

        public PoisonousDahlia(Duelist owner) : base(owner) {
            ManaCost = 4;
            Power = 5000;
            Add(Civilization.NATURE);
            Add(Race.TREE_FOLK);

            CanAttackPlayer = (player) => false;
        }

    }
}
