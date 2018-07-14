
namespace DuelMasters.Cards {
    public sealed class EmeraldGrass : Creature {
        public override string OriginalName => "Emerald Grass";

        public EmeraldGrass(Duelist owner) : base(owner) {
            ManaCost = 2;
            Power = 3000;
            Add(Civilization.LIGHT);
            Add(Race.STARLIGHT_TREE);

            CanAttackPlayer = (player) => false;
            Blocker = true;
        }

    }
}
