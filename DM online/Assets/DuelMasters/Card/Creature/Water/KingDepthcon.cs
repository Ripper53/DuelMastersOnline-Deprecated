
namespace DuelMasters.Cards {
    public sealed class KingDepthcon : Creature {
        public override string OriginalName => "King Depthcon";

        public KingDepthcon(Duelist owner) : base(owner) {
            ManaCost = 7;
            ShieldBreaker = 2;
            Power = 6000;
            Add(Civilization.WATER);
            Add(Race.LEVIATHAN);

            // Cannot be Blocked by any Blocker.
            Blockable = (blocker) => false;
        }

    }
}
