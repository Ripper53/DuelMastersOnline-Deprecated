
namespace DuelMasters.Cards {
    public sealed class BolshackDragon : Creature {
        public override string OriginalName => "Bolshack Dragon";

        public BolshackDragon(Duelist owner) : base(owner) {
            ManaCost = 6;
            ShieldBreaker = 2;
            Power = 6000;
            Add(Civilization.FIRE);
            Add(Race.ARMORED_DRAGON);

            Effect.PowerAttackerForEach(this, 1000, () => Owner.Graveyard.Count<Card>(Civilization.FIRE));
        }

    }
}
