
namespace DuelMasters.Cards {
    public sealed class DiaNorkMoonlightGuardian : Creature {
        public override string OriginalName => "Dia Nork, Moonlight Guardian";

        public DiaNorkMoonlightGuardian(Duelist owner) : base(owner) {
            ManaCost = 4;
            Power = 5000;
            Add(Civilization.LIGHT);
            Add(Race.GUARDIAN);

            CanAttackPlayer = (defendingPlayer) => false;
            Blocker = true;
        }

    }
}
