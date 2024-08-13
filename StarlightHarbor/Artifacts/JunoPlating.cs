using System.Collections.Generic;

namespace StarlightHarbor.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] { ArtifactPool.EventOnly }, unremovable = true)]
    internal class JunoPlating : Artifact
    {
       
        //limit max evade to two, add card offerings
        private const int maxEvadeLimit = 2;
        private const int healthBoost = 3;
        public override string Description() => "<c=downside>You can no longer have more than 2 <c=status>EVADE</c> at a time, shield reduces by 1 at start of turn</c> +3 max hp gained from killing bosses";

        public override void OnRemoveArtifact(State state)
        {
            //hmmm coding
            state.ship.hpGainFromBossKills-= healthBoost;
        }
        public override void OnReceiveArtifact(State state)
        {
            state.ship.hpGainFromBossKills += healthBoost;
            if (state.ship.evadeMax.HasValue)
            {
                int? evadeMax = state.ship.evadeMax;
                int num = maxEvadeLimit;
                if (!(evadeMax.GetValueOrDefault() > num & evadeMax.HasValue))
                    return;
            }
            state.ship.evadeMax = new int?(maxEvadeLimit);
        }
        public override void OnTurnStart(State state, Combat combat) 
        {
            if (state.ship.Get(Status.shield) >0)
            {
                AStatus a = new AStatus();
                a.status = Status.shield;
                a.statusAmount = -1;
                a.targetPlayer = true;
                a.artifactPulse = this.Key();
                combat.QueueImmediate((CardAction)a);
            }
        }

    }
}
