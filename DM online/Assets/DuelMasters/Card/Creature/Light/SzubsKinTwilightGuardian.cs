
namespace DuelMasters.Cards {
    public sealed class SzubsKinTwilightGuardian : Creature {
        public override string OriginalName => "Szubs Kin, Twilight Guardian";

        public SzubsKinTwilightGuardian(Duelist owner) : base(owner) {
            ManaCost = 5;
            Power = 6000;
            Add(Civilization.LIGHT);
            Add(Race.GUARDIAN);

            Blocker = true;
            CanAttackPlayer = (player) => false;
        }

    }
}
