using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuelMasters.Cards {
    public sealed class OnslaughterTriceps : Creature {
        public override string OriginalName => "Onslaughter Triceps";

        public OnslaughterTriceps(Duelist owner) : base(owner) {
            ManaCost = 3;
            Power = 5000;
            Add(Civilization.FIRE);
            Add(Race.DRAGONOID);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this)
                    Owner.TaskList.AddTask(Effect.ManaZoneToGraveyardTask<Card>(this, 1, false));
            };
        }

    }
}
