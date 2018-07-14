
namespace DuelMasters.Cards {
    public sealed class DarkClown : Creature {
        public override string OriginalName => "Dark Clown";

        public DarkClown(Duelist owner) : base(owner) {
            ManaCost = 4;
            Power = 6000;
            Add(Civilization.DARKNESS);
            Add(Race.BRAIN_JACKER);

            Blocker = true;
            CanAttack = (creature) => false;
            Effect.DestroyAfterWinningBattle(this);
        }

    }
}
