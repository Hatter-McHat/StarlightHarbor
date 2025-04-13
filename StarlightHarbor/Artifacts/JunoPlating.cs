using System.Collections.Generic;

namespace StarlightHarbor.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] { ArtifactPool.EventOnly }, unremovable = true)]
    internal class JunoPlating : Artifact
    {

        //limit max evade to two, add card offerings
        public const int maxEvadeLimit = 2;
        public const int healthBoost = 3;
        public override string Description() => "";

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
        public override List<Tooltip>? GetExtraTooltips()
        {
            return new List<Tooltip>()
            {
                {
                    (Tooltip)new TTGlossary("status.evade", new object[1]
                    {
                      (object) "1"
                    })
                },
                {
                    (Tooltip)new TTGlossary("status.shield", new object[1]
                    {
                      (object) "1"
                    })
                }
            };
        }

    }
}
