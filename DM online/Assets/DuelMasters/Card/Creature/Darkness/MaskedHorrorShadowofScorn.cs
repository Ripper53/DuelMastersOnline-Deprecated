
namespace DuelMasters.Cards {
    public sealed class MaskedHorrorShadowofScorn : Creature {
        public override string OriginalName => "Masked Horror, Shadow of Scorn";

        public MaskedHorrorShadowofScorn(Duelist owner) : base(owner) {
            ManaCost = 5;
            Power = 1000;
            Add(Civilization.DARKNESS);
            Add(Race.GHOST);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this)
                    Owner.TaskList.AddTask(Effect.OppDiscardRandomTask(this, 1, false));
            };
        }

    }
}
