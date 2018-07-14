
namespace DuelMasters.Cards {
    public sealed class LokVizierofHunting : Creature {
        public override string OriginalName => "Lok, Vizier of Hunting";

        public LokVizierofHunting(Duelist owner) : base(owner) {
            ManaCost = 4;
            Power = 4000;
            Add(Civilization.LIGHT);
            Add(Race.INITIATE);
        }

    }
}
