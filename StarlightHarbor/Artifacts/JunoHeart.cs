using System.Collections.Generic;

namespace StarlightHarbor.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] { ArtifactPool.Boss }, unremovable = true)]
    internal class JunoHeart : Artifact
    {
        private const int hitLimit = 4;
        private int hitsTaken = 0;
        private int energyGain = 0;
        //this description isn't used
        public override int? GetDisplayNumber(State s) => new int?(hitsTaken);
        public override string Description() => "";

        public override void OnPlayerTakeNormalDamage(State state, Combat combat, int rawAmount, Part? part)
        {
            hitsTaken++;
        }
        public override void OnTurnStart(State state, Combat combat)
        {
            if (hitsTaken >= hitLimit)
            {
                energyGain =(hitsTaken - (hitsTaken % hitLimit)) / hitLimit;
                AEnergy a = new AEnergy();
                a.changeAmount =(int)energyGain;
                a.artifactPulse = this.Key();
                combat.QueueImmediate((CardAction)a);
                hitsTaken = hitsTaken % hitLimit;
            }
        }


    }
}
