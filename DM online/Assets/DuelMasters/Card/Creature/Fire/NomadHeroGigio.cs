
namespace DuelMasters.Cards {
    public sealed class NomadHeroGigio : Creature {
        public override string OriginalName => "Nomad Hero Gigio";

        public NomadHeroGigio(Duelist owner) : base(owner) {
            ManaCost = 5;
            Power = 3000;
            Add(Civilization.FIRE);
            Add(Race.MACHINE_EATER);

            CanAttackUntapped = (creature) => true;
        }

    }
}
