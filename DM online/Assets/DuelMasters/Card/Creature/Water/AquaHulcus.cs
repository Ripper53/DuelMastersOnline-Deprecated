
namespace DuelMasters.Cards {
    public sealed class AquaHulcus : Creature {
        public override string OriginalName => "Aqua Hulcus";

        public AquaHulcus(Duelist owner) : base(owner) {
            // Mana Number is automatically set to 1 in the base constructor.
            ManaCost = 3;
            Power = 2000;
            Add(Civilization.WATER);
            Add(Race.LIQUID_PEOPLE);

            // Event triggers when ANY Card is put into the Battle Zone on ANY side (friendly or enemy).
            // params: (Game the card is in, Battle Zone it was put into (there are multiple Battle Zones, representing different sides for each Duelist), Card that was put into the Battle Zone).
            owner.Game.BattleZonePut += (game, bz, putCard) => {
                // Checks if the Card put into the Battle Zone is THIS Card.
                if (putCard == this) {
                    // If it IS, then give the Duelist an option to draw 1 Card.
                    owner.TaskList.AddTask(Effect.DrawTask(this, 1));
                }
            };

        }

    }
}
