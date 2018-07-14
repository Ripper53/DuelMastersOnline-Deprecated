
namespace DuelMasters.Cards {
    public sealed class MightyShouter : Creature {
        public override string OriginalName => "Mighty Shouter";

        public MightyShouter(Duelist owner) : base(owner) {
            ManaCost = 3;
            Power = 2000;
            Add(Civilization.NATURE);
            Add(Race.BEAST_FOLK);

            Destroy = () => Owner.ManaZone.Put(this);
        }

    }
}
