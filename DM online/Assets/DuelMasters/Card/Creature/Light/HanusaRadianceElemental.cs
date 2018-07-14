
namespace DuelMasters.Cards {
    public sealed class HanusaRadianceElemental : Creature {
        public override string OriginalName => "Hanusa, Radiance Elemental";

        public HanusaRadianceElemental(Duelist owner) : base(owner) {
            ManaCost = 7;
            ShieldBreaker = 2;
            Power = 9500;
            Add(Civilization.LIGHT);
            Add(Race.ANGEL_COMMAND);
        }

    }
}
