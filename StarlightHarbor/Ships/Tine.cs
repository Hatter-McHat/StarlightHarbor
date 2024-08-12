
using CobaltCoreModding.Definitions;
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


#nullable enable
namespace StarlightHarbor.Ships
{
    internal class Tine :
      ISpriteManifest,
      IManifest,
      IShipPartManifest,
      IShipManifest,
      IStartershipManifest,
      IArtifactManifest
    {
        public static Dictionary<string, ExternalSprite> sprites = new Dictionary<string, ExternalSprite>();
        public static Dictionary<string, ExternalPart> parts = new Dictionary<string, ExternalPart>();
        public static Dictionary<string, ExternalArtifact> artifacts = new Dictionary<string, ExternalArtifact>();
        private ExternalShip? tine;

        public DirectoryInfo? ModRootFolder { get; set; }



        public string Name => "starlight.harbor.tine";

        public IEnumerable<DependencyEntry> Dependencies => (IEnumerable<DependencyEntry>)new DependencyEntry[0];

        public ILogger? Logger { get; set; }

        //public static ExternalGlossary? ErisStrifeEngineGlossary { get; private set; }

        public DirectoryInfo? GameRootFolder { get; set; }

        private void addSprite(string name, ISpriteRegistry artRegistry)
        {
            if (this.ModRootFolder == null)
                throw new Exception("Root Folder not set");
            string fileName = Path.Combine(this.ModRootFolder.FullName, "Sprites", "Tine", Path.GetFileName(name + ".png"));
            Tine.sprites.Add(name, new ExternalSprite("starlight.harbor.tine." + name, new FileInfo(fileName)));
            artRegistry.RegisterArt(Tine.sprites[name]);
        }

        private void addPart(
          string name,
        string sprite,
          PType type,
          bool flip,
          PDamMod damageMod,
          IShipPartRegistry registry)
        {
            Dictionary<string, ExternalPart> parts = Tine.parts;
            string key = name;
            string globalName = "starlight.harbor.tine." + name;
            Part partObjectTemplate = new Part();
            partObjectTemplate.active = true;
            partObjectTemplate.damageModifier = damageMod;
            partObjectTemplate.type = type;
            partObjectTemplate.flip = flip;
            ExternalPart externalPart = new ExternalPart(globalName, (object)partObjectTemplate, Tine.sprites[sprite] ?? throw new Exception());
            parts.Add(key, externalPart);
            registry.RegisterPart(Tine.parts[name]);
        }

        private void addArtifact(
          string name,
          string art,
          string desc,
          Type artifact,
          IArtifactRegistry registry)
        {
            Tine.artifacts.Add(name, new ExternalArtifact("starlight.harbor" + name, artifact, Tine.sprites[art], (IEnumerable<ExternalGlossary>)new ExternalGlossary[0], (ExternalDeck)null, (IEnumerable<int>)null, (IEnumerable<string>)new string[1]));
            Tine.artifacts[name].AddLocalisation(name, desc);
            registry.RegisterArtifact(Tine.artifacts[name]);
        }

        public void LoadManifest(ISpriteRegistry artRegistry)
        {
            this.addSprite("tine_cockpit", artRegistry);
            this.addSprite("tine_cannon", artRegistry);
            this.addSprite("tine_launcher", artRegistry);
            this.addSprite("tine_wing", artRegistry);
            this.addSprite("tine_wing_b", artRegistry);
            this.addSprite("tine_structure", artRegistry);
            //non-part
            this.addSprite("tine_chassis", artRegistry);
            this.addSprite("tine_mine", artRegistry);

        }

        public void LoadManifest(IShipPartRegistry registry)
        {
            //add parts here, flip boolean
            //oh boy, different naming conventions
            this.addPart("wing0", "tine_wing", (PType)3, false, 0, registry);
            this.addPart("launcher", "tine_launcher", (PType)2, false, 0, registry);
            this.addPart("cannon", "tine_cannon", (PType)1, false, 0, registry);
            this.addPart("structure", "tine_structure", (PType)4, false, 0, registry);
            this.addPart("cockpit", "tine_cockpit", (PType)0, false, 0, registry);
            this.addPart("wing1", "tine_wing", (PType)3, true, 0, registry);
        }

        public void LoadManifest(IArtifactRegistry registry)
        {
            this.addArtifact("TINE MINE", "tine_mine", "Launch a <c=midrow>space mine</c> at the end of every turn. Start combat with one droneshift.", typeof(TineMine), registry);
        }

        public void LoadManifest(IShipRegistry shipRegistry)
        {
            Ship shipObjectTemplate = new Ship();
            shipObjectTemplate.baseDraw = 5;
            shipObjectTemplate.baseEnergy = 3;
            shipObjectTemplate.heatTrigger = 3;
            shipObjectTemplate.heatMin = 0;
            shipObjectTemplate.hull = 12;
            shipObjectTemplate.hullMax = 12;
            shipObjectTemplate.shieldMaxBase = 4;
            //left to right, ship made here
            ExternalPart[] parts = new ExternalPart[6]
            {
                Tine.parts["wing0"],
                Tine.parts["cockpit"],
                Tine.parts["structure"],
                Tine.parts["cannon"],
                Tine.parts["launcher"],
                Tine.parts["wing1"],

            };
            this.tine = new ExternalShip("starlight.harbor.tineShip", (object)shipObjectTemplate, (IEnumerable<ExternalPart>)parts, Tine.sprites["tine_chassis"] ?? throw new Exception());
            shipRegistry.RegisterShip(this.tine);
        }

        public void LoadManifest(IStartershipRegistry registry)
        {
            if (this.tine == null)
                return;
            ExternalStarterShip starterShip = new ExternalStarterShip("starlight.harbor.tineStarter", this.tine.GlobalName, (IEnumerable<ExternalCard>)new ExternalCard[0], (IEnumerable<ExternalArtifact>)new ExternalArtifact[1]
            {
               //change from 0 to 1 when uncommenting
               Tine.artifacts["TINE MINE"]
            }, (IEnumerable<Type>)new Type[3]
            {
                typeof (DodgeColorless),
                typeof (BasicShieldColorless),
                typeof (DroneshiftColorless)
            }, (IEnumerable<Type>)new Type[2]
            {
                new ArmoredBay().GetType(),
                new ShieldPrep().GetType()
            });
            starterShip.AddLocalisation(nameof(Tine), "Prickly.\nConstantly launches mines, be careful!");
            registry.RegisterStartership(starterShip);
        }
    }
}