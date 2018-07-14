
namespace DuelMasters.Cards {
    public sealed class UrthPurifyingElemental : Creature {
        public override string OriginalName => "Urth, Purifying Elemental";

        public UrthPurifyingElemental(Duelist owner) : base(owner) {
            ManaCost = 6;
            ShieldBreaker = 2;
            Power = 6000;
            Add(Civilization.LIGHT);
            Add(Race.ANGEL_COMMAND);

            // If Creature is tapped, Player can choose to untap Creature.
            owner.AtTurnEnd += (source) => {
                if (Tapped) source.TaskList.AddTask(Effect.UntapSelfTask(this));
            };
        }

    }
}
