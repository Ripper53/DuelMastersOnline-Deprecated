
namespace DuelMasters.Cards {
    public sealed class IereVizierofBullets : Creature {
        public override string OriginalName => "Iere, Vizier of Bullets";

        public IereVizierofBullets(Duelist owner) : base(owner) {
            ManaCost = 3;
            Power = 3000;
            Add(Civilization.LIGHT);
            Add(Race.INITIATE);
        }

    }
}
