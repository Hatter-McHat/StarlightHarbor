
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
//using parchmentArmada.Artifacts;
using StarlightHarbor.Artifacts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using CobaltCoreModding.Definitions;


#nullable enable
namespace StarlightHarbor.Ships
{
    internal class Luna :
      ISpriteManifest,
      IManifest,
      IShipPartManifest,
      IShipManifest,
      IStartershipManifest,
      IArtifactManifest,
      ICardManifest,
      IDeckManifest
      //IGlossaryManifest
    {
        public static Dictionary<string, ExternalSprite> sprites = new Dictionary<string, ExternalSprite>();
        public static Dictionary<string, ExternalPart> parts = new Dictionary<string, ExternalPart>();
        public static Dictionary<string, ExternalArtifact> artifacts = new Dictionary<string, ExternalArtifact>();
        private ExternalShip? luna;
        public DirectoryInfo? ModRootFolder { get; set; }
        public static ExternalDeck? LunaDeck { get; private set; }
        public static ExternalCard? LunaCasingsCard { get; private set; }

        public string Name => "starlight.harbor.luna";
        public ILogger? Logger { get; set; }

        public IEnumerable<DependencyEntry> Dependencies => (IEnumerable<DependencyEntry>)new DependencyEntry[0];


        //public static ExternalGlossary? ErisStrifeEngineGlossary { get; private set; }

        public DirectoryInfo? GameRootFolder { get; set; }

        private void addSprite(string name, ISpriteRegistry artRegistry)
        {
            if (this.ModRootFolder == null)
                throw new Exception("Root Folder not set");
            string fileName = Path.Combine(this.ModRootFolder.FullName, "Sprites","Luna", Path.GetFileName(name + ".png"));
            Luna.sprites.Add(name, new ExternalSprite("starlight.harbor.luna." + name, new FileInfo(fileName)));
            artRegistry.RegisterArt(Luna.sprites[name]);
        }

        private void addPart(
          string name,
        string sprite,
          PType type,
          bool flip,
          IShipPartRegistry registry)
        {
            Dictionary<string, ExternalPart> parts = Luna.parts;
            string key = name;
            string globalName = "starlight.harbor.luna." + name;
            Part partObjectTemplate = new Part();
            partObjectTemplate.active = true;
            partObjectTemplate.damageModifier = (PDamMod)0;
            partObjectTemplate.type = type;
            partObjectTemplate.flip = flip;
            ExternalPart externalPart = new ExternalPart(globalName, (object)partObjectTemplate, Luna.sprites[sprite] ?? throw new Exception());
            parts.Add(key, externalPart);
            registry.RegisterPart(Luna.parts[name]);
        }

        private void addArtifact(
          string name,
          string art,
          string desc,
          Type artifact,
          IArtifactRegistry registry)
        {
            Luna.artifacts.Add(name, new ExternalArtifact( "starlight.harbor" + name, artifact, Luna.sprites[art], (IEnumerable<ExternalGlossary>)new ExternalGlossary[0], (ExternalDeck)null, (IEnumerable<int>)null, (IEnumerable<string>)new string[1]));
            Luna.artifacts[name].AddLocalisation(name, desc);
            registry.RegisterArtifact(Luna.artifacts[name]);
        }

        public void LoadManifest(ISpriteRegistry artRegistry)
        {
            this.addSprite("luna_cannon", artRegistry);
            this.addSprite("luna_launcher", artRegistry);
            this.addSprite("luna_cockpit", artRegistry);
            this.addSprite("luna_structure", artRegistry);
            this.addSprite("luna_chassis", artRegistry);
            this.addSprite("luna_ammofeed", artRegistry);
            this.addSprite("luna_primer", artRegistry);
            this.addSprite("luna_cardframe", artRegistry);
            this.addSprite("luna_casings", artRegistry);
        }

        public void LoadManifest(IShipPartRegistry registry)
        {
            //add parts here, flip boolean
            //oh boy, different naming conventions
            this.addPart("cockpit0", "luna_cockpit", (PType)0, false, registry);
            this.addPart("cannon0", "luna_cannon", (PType)1, false, registry);
            this.addPart("structure0", "luna_structure", (PType)4, false, registry);
            this.addPart("cannon1", "luna_cannon", (PType)1, true, registry);
            this.addPart("launcher0", "luna_launcher", (PType)2, false, registry);
        }

        public void LoadManifest(IArtifactRegistry registry)
        {
            this.addArtifact("LUNA AMMOFEED", "luna_ammofeed", "Gain 2 less <c=energy>ENERGY</c> every turn.</c> The first 2 times you play a card each turn, gain 1 <c=energy>ENERGY</c>.\n<c=downside>Whenever you play an <c=action>attacking</c> card, add 1 <c=card>Spent Casings</c> to your draw pile.</c>", typeof(LunaAmmo), registry);
            this.addArtifact("LUNA PRIMER", "luna_primer", "Every 5 <c=card>Spent Casings</c> played, gain 1 <c=status>POWERDRIVE</c>.", typeof(LunaPrimer), registry);
        }
        public void LoadManifest(IDeckRegistry registry)
        {
            ExternalSprite raw = Luna.sprites["luna_casings"] ?? throw new Exception();
            ExternalSprite borderSprite = Luna.sprites["luna_cardframe"] ?? throw new Exception();
            Luna.LunaDeck = new ExternalDeck("starlight.harbor.lunaDeck", System.Drawing.Color.FromArgb(86, 50, 88), System.Drawing.Color.Black, raw, borderSprite, (ExternalSprite)null);
            registry.RegisterDeck(Luna.LunaDeck);
        }
        public void LoadManifest(ICardRegistry registry)
        {
            Luna.LunaCasingsCard = new ExternalCard("starlight.harbor.LunaCasingsCard", typeof(StarlightHarbor.Cards.LunaCasingsCard), ExternalSprite.GetRaw(441), Luna.LunaDeck);
            Luna.LunaCasingsCard.AddLocalisation("Spent Casings");
            registry.RegisterCard(Luna.LunaCasingsCard);
        }

        public void LoadManifest(IShipRegistry shipRegistry)
        {
            Ship shipObjectTemplate = new Ship();
            shipObjectTemplate.baseDraw = 5;
            shipObjectTemplate.baseEnergy = 3;
            shipObjectTemplate.heatTrigger = 3;
            shipObjectTemplate.heatMin = 0;
            shipObjectTemplate.hull = 7;
            shipObjectTemplate.hullMax = 7;
            shipObjectTemplate.shieldMaxBase = 3;
            //left to right, ship made here
            ExternalPart[] parts = new ExternalPart[5]
            {
                Luna.parts["cockpit0"],
                Luna.parts["cannon0"],
                Luna.parts["structure0"],
                Luna.parts["cannon1"],
                Luna.parts["launcher0"],
            };
            this.luna = new ExternalShip("starlight.harbor.lunaShip", (object)shipObjectTemplate, (IEnumerable<ExternalPart>)parts, Luna.sprites["luna_chassis"] ?? throw new Exception());
            shipRegistry.RegisterShip(this.luna);
        }

        public void LoadManifest(IStartershipRegistry registry)
        {
            if (this.luna == null)
                return;
            ExternalStarterShip starterShip = new ExternalStarterShip("starlight.harbor.lunaStarter", this.luna.GlobalName, (IEnumerable<ExternalCard>)new ExternalCard[0], (IEnumerable<ExternalArtifact>)new ExternalArtifact[1]
            {
                //change from 0 to 1 when uncommenting
                Luna.artifacts["LUNA AMMOFEED"]
            }, (IEnumerable<Type>)new Type[1]
            {
                //upgrade to A, handled by artifact
                //typeof (DodgeColorless),
                //typeof (CannonColorless),
                typeof (BasicShieldColorless)
            }, (IEnumerable<Type>)new Type[1]
            {
                new ShieldPrep().GetType()
            }, (IEnumerable<ExternalArtifact>)new ExternalArtifact[1] 
            {
                Luna.artifacts["LUNA PRIMER"]
            }) ;

            starterShip.AddLocalisation(nameof(Luna), "A gunship, capable of devastating firepower. Starts with  <c=card>Basic Dodge</c=card> and  <c=card>Basic Shot </c=card> upgraded.");
            registry.RegisterStartership(starterShip);           
        }
    }
}