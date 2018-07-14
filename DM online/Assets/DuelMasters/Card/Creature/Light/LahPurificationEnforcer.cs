
namespace DuelMasters.Cards {
    public sealed class LahPurificationEnforcer : Creature {
        public override string OriginalName => "Lah, Purification Enforcer";

        public LahPurificationEnforcer(Duelist owner) : base(owner) {
            ManaCost = 5;
            Power = 5500;
            Add(Civilization.LIGHT);
            Add(Race.BERSERKER);
        }

    }
}
