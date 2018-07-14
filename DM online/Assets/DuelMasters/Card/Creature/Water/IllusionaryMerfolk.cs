
namespace DuelMasters.Cards {
    public sealed class IllusionaryMerfolk : Creature {
        public override string OriginalName => "Illusionary Merfolk";

        public IllusionaryMerfolk(Duelist owner) : base(owner) {
            ManaCost = 5;
            Power = 4000;
            Add(Civilization.WATER);
            Add(Race.GEL_FISH);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this && Owner.BattleZone.Contains(Race.CYBER_LORD))
                    Owner.TaskList.AddTask(Effect.DrawUpTo(this, 3));
            };
        }

    }
}
