
namespace DuelMasters.Cards {
    public sealed class DeathbladeBeetle : Creature {
        public override string OriginalName => "Deathblade Beetle";

        public DeathbladeBeetle(Duelist owner) : base(owner) {
            ManaCost = 5;
            ShieldBreaker = 2;
            Power = 3000;
            Add(Civilization.NATURE);
            Add(Race.GIANT_INSECT);

            Effect.PowerAttacker(this, 4000);
        }

    }
}
