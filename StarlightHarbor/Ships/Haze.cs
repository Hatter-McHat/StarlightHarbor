
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using StarlightHarbor.Artifacts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using CobaltCoreModding.Definitions;
using StarlightHarbor.Cards;


#nullable enable
namespace StarlightHarbor.Ships
{
    internal class Haze :
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
        private ExternalShip? haze;
        public DirectoryInfo? ModRootFolder { get; set; }

        public string Name => "starlight.harbor.haze";
        public ILogger? Logger { get; set; }

        public IEnumerable<DependencyEntry> Dependencies => (IEnumerable<DependencyEntry>)new DependencyEntry[0];

        public static ExternalCard? HazeDroneCard { get; private set; }
        public static ExternalDeck? HazeDeck { get; private set; }

        //public static ExternalGlossary? ErisStrifeEngineGlossary { get; private set; }

        public DirectoryInfo? GameRootFolder { get; set; }

        private void addSprite(string name, ISpriteRegistry artRegistry)
        {
            if (this.ModRootFolder == null)
                throw new Exception("Root Folder not set");
            string fileName = Path.Combine(this.ModRootFolder.FullName, "Sprites", "Haze", Path.GetFileName(name + ".png"));
            Haze.sprites.Add(name, new ExternalSprite("starlight.harbor.haze." + name, new FileInfo(fileName)));
            artRegistry.RegisterArt(Haze.sprites[name]);
        }

        private void addPart(
          string name,
        string sprite,
          PType type,
          bool flip,
          IShipPartRegistry registry)
        {
            Dictionary<string, ExternalPart> parts = Haze.parts;
            string key = name;
            string globalName = "starlight.harbor.haze." + name;
            Part partObjectTemplate = new Part();
            partObjectTemplate.active = true;
            partObjectTemplate.damageModifier = (PDamMod)0;
            partObjectTemplate.type = type;
            partObjectTemplate.flip = flip;
            ExternalPart externalPart = new ExternalPart(globalName, (object)partObjectTemplate, Haze.sprites[sprite] ?? throw new Exception());
            parts.Add(key, externalPart);
            registry.RegisterPart(Haze.parts[name]);
        }

        private void addArtifact(
          string name,
          string art,
          string desc,
          Type artifact,
          IArtifactRegistry registry)
        {
            //problem child
            Haze.artifacts.Add(name, new ExternalArtifact( "starlight.harbor" + name, artifact, Haze.sprites[art], (IEnumerable<ExternalGlossary>)new ExternalGlossary[0], (ExternalDeck)null, (IEnumerable<int>)null, (IEnumerable<string>)new string[1]));
            Haze.artifacts[name].AddLocalisation(name, desc);
            registry.RegisterArtifact(Haze.artifacts[name]);
        }

        public void LoadManifest(ISpriteRegistry artRegistry)
        {
            this.addSprite("haze_cannon", artRegistry);
            this.addSprite("haze_launcher", artRegistry);
            this.addSprite("haze_cockpit", artRegistry);
            this.addSprite("haze_structure_a", artRegistry);
            this.addSprite("haze_structure_b", artRegistry);
            this.addSprite("haze_chassis", artRegistry);
            this.addSprite("haze_core", artRegistry);
            this.addSprite("haze_drone_card", artRegistry);
        }

        public void LoadManifest(IShipPartRegistry registry)
        {
            //add parts here, flip boolean
            //oh boy, different naming conventions
            this.addPart("cockpit0", "haze_cockpit", (PType)0, false, registry);
            this.addPart("structure0", "haze_structure_a", (PType)4, false, registry);
            this.addPart("launcher0", "haze_launcher", (PType)2, false, registry);
            this.addPart("structure1", "haze_structure_b", (PType)4, false, registry);
            this.addPart("cannon0", "haze_cannon", (PType)1, false, registry);
            this.addPart("structure2", "haze_structure_b", (PType)4, true, registry);
            this.addPart("launcher1", "haze_launcher", (PType)2, true, registry);
            this.addPart("structure3", "haze_structure_a", (PType)4, true, registry);
            this.addPart("cockpit1", "haze_cockpit", (PType)0, true, registry);
        }

        public void LoadManifest(IArtifactRegistry registry)
        {   
            //problem child
            this.addArtifact("HAZE CORE", "haze_core", "<c=downside>For every 4 objects you <c=midrow>LAUNCH</c>, add 2 <c=card>Fumes</c> to your discard pile.</c>", typeof(HazeCore), registry);
        }
        public void LoadManifest(IShipRegistry shipRegistry)
        {
            Ship shipObjectTemplate = new Ship();
            shipObjectTemplate.baseDraw = 5;
            shipObjectTemplate.baseEnergy = 3;
            shipObjectTemplate.heatTrigger = 3;
            shipObjectTemplate.heatMin = 0;
            shipObjectTemplate.hull = 9;
            shipObjectTemplate.hullMax = 9;
            shipObjectTemplate.shieldMaxBase = 3;
            //left to right, ship made here
            ExternalPart[] parts = new ExternalPart[9]
            {
                Haze.parts["cockpit0"],
                Haze.parts["structure0"],
                Haze.parts["launcher0"],
                Haze.parts["structure1"],
                Haze.parts["cannon0"],
                Haze.parts["structure2"],
                Haze.parts["launcher1"],
                Haze.parts["structure3"],
                Haze.parts["cockpit1"]
            };
            this.haze = new ExternalShip("starlight.harbor.hazeShip", (object)shipObjectTemplate, (IEnumerable<ExternalPart>)parts, Haze.sprites["haze_chassis"] ?? throw new Exception());
            shipRegistry.RegisterShip(this.haze);
        }


        public void LoadManifest(IStartershipRegistry registry)
        {
            //problem child
            if (this.haze == null)
                return;
            ExternalStarterShip starterShip = new ExternalStarterShip("starlight.harbor.hazeStarter", this.haze.GlobalName, (IEnumerable<ExternalCard>)new ExternalCard[0], (IEnumerable<ExternalArtifact>)new ExternalArtifact[1]
            {
                Haze.artifacts["HAZE CORE"]
            }, (IEnumerable<Type>)new Type[3]
            {
                typeof (DodgeColorless),
                typeof (CannonColorless),
                typeof (BasicShieldColorless)
            }, (IEnumerable<Type>)new Type[1]
            {
                new ShieldPrep().GetType()
            });
            starterShip.AddLocalisation(nameof(Haze), "A wide and evasive bomber ship with two cockpits.");
            registry.RegisterStartership(starterShip);
        }
    }
}