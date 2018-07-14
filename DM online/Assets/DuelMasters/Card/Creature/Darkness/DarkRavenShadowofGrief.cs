
namespace DuelMasters.Cards {
    public sealed class DarkRavenShadowofGrief : Creature {
        public override string OriginalName => "Dark Raven, Shadow of Grief";

        public DarkRavenShadowofGrief(Duelist owner) : base(owner) {
            ManaCost = 4;
            Power = 1000;
            Add(Civilization.DARKNESS);
            Add(Race.GHOST);

            Blocker = true;
        }

    }
}
