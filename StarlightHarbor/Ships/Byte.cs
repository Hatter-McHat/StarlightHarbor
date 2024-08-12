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
    internal class Byte :
      ISpriteManifest,
      IManifest,
      IShipPartManifest,
      IShipManifest,
      IStartershipManifest,
      IArtifactManifest
      //ICardManifest,
      //IDeckManifest
      //IGlossaryManifest
    {
        public static Dictionary<string, ExternalSprite> sprites = new Dictionary<string, ExternalSprite>();
        public static Dictionary<string, ExternalPart> parts = new Dictionary<string, ExternalPart>();
        public static Dictionary<string, ExternalArtifact> artifacts = new Dictionary<string, ExternalArtifact>();
        //spelling it byte throws an error
        private ExternalShip? bite;
        public DirectoryInfo? ModRootFolder { get; set; }
        public static ExternalDeck? ByteDeck { get; private set; }
        //public static ExternalCard? ByteCasingsCard { get; private set; }

        public string Name => "starlight.harbor.byte";
        public ILogger? Logger { get; set; }

        public IEnumerable<DependencyEntry> Dependencies => (IEnumerable<DependencyEntry>)new DependencyEntry[0];


        //public static ExternalGlossary? ErisStrifeEngineGlossary { get; private set; }

        public DirectoryInfo? GameRootFolder { get; set; }

        private void addSprite(string name, ISpriteRegistry artRegistry)
        {
            if (this.ModRootFolder == null)
                throw new Exception("Root Folder not set");
            string fileName = Path.Combine(this.ModRootFolder.FullName, "Sprites", "Byte", Path.GetFileName(name + ".png"));
            Byte.sprites.Add(name, new ExternalSprite("starlight.harbor.byte." + name, new FileInfo(fileName)));
            artRegistry.RegisterArt(Byte.sprites[name]);
        }

        private void addPart(
          string name,
        string sprite,
          PType type,
          bool flip,
          IShipPartRegistry registry)
        {
            Dictionary<string, ExternalPart> parts = Byte.parts;
            string key = name;
            string globalName = "starlight.harbor.byte." + name;
            Part partObjectTemplate = new Part();
            partObjectTemplate.active = true;
            partObjectTemplate.damageModifier = (PDamMod)0;
            partObjectTemplate.type = type;
            partObjectTemplate.flip = flip;
            ExternalPart externalPart = new ExternalPart(globalName, (object)partObjectTemplate, Byte.sprites[sprite] ?? throw new Exception());
            parts.Add(key, externalPart);
            registry.RegisterPart(Byte.parts[name]);
        }

        private void addArtifact(
          string name,
          string art,
          string desc,
          Type artifact,
          IArtifactRegistry registry)
        {
            Byte.artifacts.Add(name, new ExternalArtifact("starlight.harbor" + name, artifact, Byte.sprites[art], (IEnumerable<ExternalGlossary>)new ExternalGlossary[0], (ExternalDeck)null, (IEnumerable<int>)null, (IEnumerable<string>)new string[1]));
            Byte.artifacts[name].AddLocalisation(name, desc);
            registry.RegisterArtifact(Byte.artifacts[name]);
        }

        public void LoadManifest(ISpriteRegistry artRegistry)
        {
            this.addSprite("byte_wing_a", artRegistry);
            this.addSprite("byte_wing_b", artRegistry);
            this.addSprite("byte_cannon", artRegistry);
            this.addSprite("byte_launcher", artRegistry);
            this.addSprite("byte_cockpit", artRegistry);
            this.addSprite("byte_structure",artRegistry);
            this.addSprite("byte_chassis", artRegistry);
            this.addSprite("byte_drive", artRegistry);
        }

        public void LoadManifest(IShipPartRegistry registry)
        {
            //add parts here, flip boolean
            //oh boy, different naming conventions
            this.addPart("wing0", "byte_wing_a", (PType)3, false, registry);
            this.addPart("cannon0", "byte_cannon", (PType)1, false, registry);
            this.addPart("launcher0", "byte_launcher", (PType)2, false, registry);
            this.addPart("structure0", "byte_structure", (PType)4, false, registry);
            this.addPart("cockpit0", "byte_cockpit", (PType)0, false, registry);
            this.addPart("wing1", "byte_wing_b", (PType)3, false, registry);
        }

        public void LoadManifest(IArtifactRegistry registry)
        {
            this.addArtifact("BYTE DRIVE", "byte_drive", "After every battle, you may choose a <c=cardtrait>temporary</c> card in your deck to become permanent. <c=downside>After every three battles, add a <c=card>Corrupted Core</c=card> to your deck.</c=downside>", typeof(ByteDrive), registry);
        }
        /*
        public void LoadManifest(IDeckRegistry registry)
        {
            ExternalSprite raw = Byte.sprites["byte_casings"] ?? throw new Exception();
            ExternalSprite borderSprite = Byte.sprites["byte_cardframe"] ?? throw new Exception();
            Byte.ByteDeck = new ExternalDeck("starlight.harbor.byteDeck", System.Drawing.Color.FromArgb(86, 50, 88), System.Drawing.Color.Black, raw, borderSprite, (ExternalSprite)null);
            registry.RegisterDeck(Byte.ByteDeck);
        }
        */
        /*
        public void LoadManifest(ICardRegistry registry)
        {
        }
        */

        public void LoadManifest(IShipRegistry shipRegistry)
        {
            Ship shipObjectTemplate = new Ship();
            shipObjectTemplate.baseDraw = 5;
            shipObjectTemplate.baseEnergy = 3;
            shipObjectTemplate.heatTrigger = 3;
            shipObjectTemplate.heatMin = 0;
            shipObjectTemplate.hull = 8;
            shipObjectTemplate.hullMax = 8;
            shipObjectTemplate.shieldMaxBase = 3;
            //left to right, ship made here
            ExternalPart[] parts = new ExternalPart[6]
            {
                Byte.parts["wing0"],
                Byte.parts["launcher0"],
                Byte.parts["cannon0"],
                Byte.parts["structure0"],
                Byte.parts["cockpit0"],
                Byte.parts["wing1"],
            };
            this.bite = new ExternalShip("starlight.harbor.byteShip", (object)shipObjectTemplate, (IEnumerable<ExternalPart>)parts, Byte.sprites["byte_chassis"] ?? throw new Exception());
            shipRegistry.RegisterShip(this.bite);
        }

        public void LoadManifest(IStartershipRegistry registry)
        {
            if (this.bite == null)
                return;
            ExternalStarterShip starterShip = new ExternalStarterShip("starlight.harbor.byteStarter", this.bite.GlobalName, (IEnumerable<ExternalCard>)new ExternalCard[0], (IEnumerable<ExternalArtifact>)new ExternalArtifact[1]
            {
                //change from 0 to 1 when uncommenting
                Byte.artifacts["BYTE DRIVE"]
            }, (IEnumerable<Type>)new Type[4]
            {
                //upgrade to A, handled by artifact
                typeof (DodgeColorless),
                typeof (CannonColorless),
                typeof (BasicShieldColorless),
                typeof (ColorlessCATSummon)
            }, (IEnumerable<Type>)new Type[1]
            {
                new ShieldPrep().GetType()
            });

            starterShip.AddLocalisation(nameof(Byte), "Computer ship that channels the onboard A.I.");
            registry.RegisterStartership(starterShip);
        }
    }
}