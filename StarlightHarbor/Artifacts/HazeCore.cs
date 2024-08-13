namespace StarlightHarbor.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] { ArtifactPool.EventOnly }, unremovable = true)]
    internal class HazeCore : Artifact
    {
        public int fumecount = 0;
        //public bool generateFume = false;
        public override string Description() => "";
        public override int? GetDisplayNumber(State s) => new int?(fumecount);
        public override void OnPlayerSpawnSomething(State state, Combat combat, StuffBase thing)
        {
            fumecount++;
            if( fumecount >= 4) { 
                combat.Queue((CardAction)new AAddCard()
                {
                    card = (Card)new TrashFumes(),
                    destination = CardDestination.Discard,
                    //destination = CardDestination.Deck,
                    amount = 2
                

                });
                fumecount -= 4;
            }
        }

        public override void OnCombatEnd(State state) => this.fumecount = 0;

    }
}