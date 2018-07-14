
namespace DuelMasters.Cards {
    public sealed class GatlingSkyterror : Creature {
        public override string OriginalName => "Gatling Skyterror";

        public GatlingSkyterror(Duelist owner) : base(owner) {
            ManaCost = 7;
            ShieldBreaker = 2;
            Power = 7000;
            Add(Civilization.FIRE);
            Add(Race.ARMORED_WYVERN);

            CanAttackUntapped = (creature) => true;
        }

    }
}
