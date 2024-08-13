using System.Net.NetworkInformation;

namespace StarlightHarbor.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] { ArtifactPool.EventOnly }, unremovable = true)]
    internal class ByteDrive : Artifact
    {
        private const int combatLimit = 3;
        private int combatCount = 0;
        public override int? GetDisplayNumber(State s) => new int?(combatCount);
        public override void OnCombatEnd(State state)
        {
            combatCount++;
            state.rewardsQueue.QueueImmediate(new ACardSelect()
            {
                browseAction = new ChooseACardToMakePermanent(),
                browseSource = CardBrowse.Source.Deck,
                filterTemporary = true,
                allowCloseOverride = true
            });            
            if (combatCount >= combatLimit) {
                state.rewardsQueue.QueueImmediate(new AAddCard(){
                    card = new CorruptedCore(), 
                });
               combatCount =0;
            }
            
        }
        
    }
}