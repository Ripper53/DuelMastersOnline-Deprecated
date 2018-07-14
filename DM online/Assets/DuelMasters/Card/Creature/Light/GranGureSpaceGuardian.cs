
namespace DuelMasters.Cards {
    public sealed class GranGureSpaceGuardian : Creature {
        public override string OriginalName => "Gran Gure, Space Guardian";

        public GranGureSpaceGuardian(Duelist owner) : base(owner) {
            ManaCost = 6;
            Power = 9000;
            Add(Civilization.LIGHT);
            Add(Race.GUARDIAN);

            Blocker = true;
            CanAttackPlayer = (player) => false;
        }

    }
}
