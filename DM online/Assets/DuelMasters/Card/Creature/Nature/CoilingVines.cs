
namespace DuelMasters.Cards {
    public sealed class CoilingVines : Creature {
        public override string OriginalName => "Coiling Vines";

        public CoilingVines(Duelist owner) : base(owner) {
            ManaCost = 4;
            Power = 3000;
            Add(Civilization.NATURE);
            Add(Race.TREE_FOLK);

            Destroy = () => Owner.ManaZone.Put(this);
        }

    }
}
