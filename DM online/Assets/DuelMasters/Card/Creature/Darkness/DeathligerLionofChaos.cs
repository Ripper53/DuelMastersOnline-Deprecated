
namespace DuelMasters.Cards {
    public sealed class DeathligerLionofChaos : Creature {
        public override string OriginalName => "Deathliger, Lion of Chaos";

        public DeathligerLionofChaos(Duelist owner) : base(owner) {
            ManaCost = 7;
            ShieldBreaker = 2;
            Power = 9000;
            Add(Civilization.DARKNESS);
            Add(Race.DEMON_COMMAND);
        }

    }
}
