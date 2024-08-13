using System.Collections.Generic;
using StarlightHarbor.Cards;

namespace StarlightHarbor.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] { ArtifactPool.EventOnly }, unremovable = true)]
    internal class TineMine : Artifact
    {


        public override string Description() => "";
        public override void OnTurnEnd(State state, Combat combat)
        {           
                ASpawn aspawn1 = new ASpawn();
                SpaceMine spaceMine1 = new SpaceMine();
                spaceMine1.yAnimation = 0.0;
                aspawn1.thing = (StuffBase)spaceMine1;
                aspawn1.artifactPulse = this.Key();
                combat.QueueImmediate((CardAction)aspawn1);
        }
        public override void OnCombatStart(State state, Combat combat) {
            Combat combat1 = combat;
            AStatus a = new AStatus();
            a.status = Status.droneShift;
            a.statusAmount = 1;
            a.targetPlayer = true;
            a.artifactPulse = this.Key();
            combat1.QueueImmediate((CardAction)a);
        }

    }
}
