
namespace DuelMasters.Cards {
    public sealed class DarkReversal : Spell {
        public override string OriginalName => "Dark Reversal";

        public DarkReversal(Duelist owner) : base(owner) {
            ManaCost = 2;
            Add(Civilization.DARKNESS);

            ShieldTrigger = true;
        }

        protected override void CastSpell() {
            Owner.TaskList.AddTask(Effect.ReturnFromGraveyardTask<Creature>(this, 1, false));
        }

    }
}
