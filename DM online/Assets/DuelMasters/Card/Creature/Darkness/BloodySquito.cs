
namespace DuelMasters.Cards {
    public sealed class BloodySquito : Creature {
        public override string OriginalName => "Bloody Squito";

        public BloodySquito(Duelist owner) : base(owner) {
            ManaCost = 2;
            Power = 4000;
            Add(Civilization.DARKNESS);
            Add(Race.BRAIN_JACKER);

            Blocker = true;
            CanAttack = (creature) => false;

            Effect.DestroyAfterWinningBattle(this);
        }

    }
}
