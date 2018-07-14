
namespace DuelMasters.Cards {
    public sealed class MieleVizierofLightning : Creature {
        public override string OriginalName => "Miele, Vizier of Lightning";

        public MieleVizierofLightning(Duelist owner) : base(owner) {
            ManaCost = 3;
            Power = 1000;
            Add(Civilization.LIGHT);
            Add(Race.INITIATE);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this)
                    owner.TaskList.AddTask(Effect.TapOppTask(this));
            };
        }

    }
}
