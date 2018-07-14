
namespace DuelMasters.Cards {
    public sealed class FreiVizierofAir : Creature {
        public override string OriginalName => "Frei, Vizier of Air";

        public FreiVizierofAir(Duelist owner) : base(owner) {
            ManaCost = 4;
            Power = 3000;
            Add(Civilization.LIGHT);
            Add(Race.INITIATE);

            Owner.AtTurnEnd += (source) => {
                if (Tapped) owner.TaskList.AddTask(Effect.UntapSelfTask(this));
            };
        }

    }
}
