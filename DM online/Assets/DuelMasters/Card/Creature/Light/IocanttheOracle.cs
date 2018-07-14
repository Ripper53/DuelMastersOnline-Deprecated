
namespace DuelMasters.Cards {
    public sealed class IocanttheOracle : Creature {
        public override string OriginalName => "Iocant, the Oracle";

        public IocanttheOracle(Duelist owner) : base(owner) {
            ManaCost = 2;
            Power = 2000;
            Add(Civilization.LIGHT);
            Add(Race.LIGHT_BRINGER);

            Blocker = true;
            CanAttackPlayer = (player) => false;
            Effect.StaticPowerBuffSelf(this, 2000, Race.ANGEL_COMMAND);
        }

    }
}
