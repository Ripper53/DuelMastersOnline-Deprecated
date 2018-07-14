
namespace DuelMasters.Cards {
    public sealed class ToelVizierofHope : Creature {
        public override string OriginalName => "Toel, Vizier of Hope";

        public ToelVizierofHope(Duelist owner) : base(owner) {
            ManaCost = 5;
            Power = 2000;
            Add(Civilization.LIGHT);
            Add(Race.INITIATE);

            owner.AtTurnEnd += (duelist) => Owner.TaskList.AddTask(Effect.UntapAllFriendlyTask(this));
        }

    }
}
