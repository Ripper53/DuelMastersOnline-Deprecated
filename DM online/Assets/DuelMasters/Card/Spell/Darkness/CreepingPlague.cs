
using System.Collections.Generic;

namespace DuelMasters.Cards {
    public sealed class CreepingPlague : Spell {
        public override string OriginalName => "Creeping Plague";

        public CreepingPlague(Duelist owner) : base(owner) {
            ManaCost = 1;
            Add(Civilization.DARKNESS);

            creaturesWithSlayer = new List<Creature>();
        }

        private List<Creature> creaturesWithSlayer;

        /// <summary>
        /// Friendly Creatures that are blocked this turn will gain Slayer for this turn.
        /// </summary>
        protected override void CastSpell() {
            Owner.Game.BlockedCreature += Gain_Slayer;
            Owner.TurnEnded += (duelist) => {
                Owner.Game.BlockedCreature -= Gain_Slayer;
                foreach (Creature creature in creaturesWithSlayer)
                    Effect.RemoveSlayer(creature);
                creaturesWithSlayer.Clear();
            };
        }
        private void Gain_Slayer(Game source, Creature atking, Creature blocker) {
            if (atking.Owner == Owner && !creaturesWithSlayer.Contains(atking)) {
                Effect.Slayer(atking);
                creaturesWithSlayer.Add(atking);
            }
        }

    }
}
