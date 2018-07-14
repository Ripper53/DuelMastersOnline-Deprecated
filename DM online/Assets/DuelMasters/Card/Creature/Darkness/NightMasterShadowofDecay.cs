
namespace DuelMasters.Cards {
    public sealed class NightMasterShadowofDecay : Creature {
        public override string OriginalName => "Night Master, Shadow of Decay";

        public NightMasterShadowofDecay(Duelist owner) : base(owner) {
            ManaCost = 6;
            Power = 3000;
            Add(Civilization.DARKNESS);
            Add(Race.GHOST);

            Blocker = true;
        }

    }
}
