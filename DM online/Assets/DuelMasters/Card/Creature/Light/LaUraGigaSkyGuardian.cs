
namespace DuelMasters.Cards {
    public sealed class LaUraGigaSkyGuardian : Creature {
        public override string OriginalName => "La Ura Giga, Sky Guardian";

        public LaUraGigaSkyGuardian(Duelist owner) : base(owner) {
            ManaCost = 1;
            Power = 2000;
            Add(Civilization.LIGHT);
            Add(Race.GUARDIAN);

            Blocker = true;
            CanAttackPlayer = (player) => false;
        }

    }
}
