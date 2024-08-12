
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
using static OneOf.Types.TrueFalseOrNull;


#nullable enable
namespace StarlightHarbor.Ships
{
    internal class Juno :
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
        private ExternalShip? juno;

        public DirectoryInfo? ModRootFolder { get; set; }
        


        public string Name => "starlight.harbor.juno";

        public IEnumerable<DependencyEntry> Dependencies => (IEnumerable<DependencyEntry>)new DependencyEntry[0];

        public ILogger? Logger { get; set; }

        //public static ExternalGlossary? ErisStrifeEngineGlossary { get; private set; }

        public DirectoryInfo? GameRootFolder { get; set; }

        private void addSprite(string name, ISpriteRegistry artRegistry)
        {
            if (this.ModRootFolder == null)
                throw new Exception("Root Folder not set");
            string fileName = Path.Combine(this.ModRootFolder.FullName, "Sprites","Juno", Path.GetFileName(name + ".png"));
            Juno.sprites.Add(name, new ExternalSprite("starlight.harbor.juno." + name, new FileInfo(fileName)));
            artRegistry.RegisterArt(Juno.sprites[name]);
        }

        private void addPart(
          string name,
        string sprite,
          PType type,
          bool flip,
          PDamMod damageMod,
          IShipPartRegistry registry)
        {
            Dictionary<string, ExternalPart> parts = Juno.parts;
            string key = name;
            string globalName = "starlight.harbor.juno." + name;
            Part partObjectTemplate = new Part();
            partObjectTemplate.active = true;
            partObjectTemplate.damageModifier = damageMod;
            partObjectTemplate.type = type;
            partObjectTemplate.flip = flip;
            ExternalPart externalPart = new ExternalPart(globalName, (object)partObjectTemplate, Juno.sprites[sprite] ?? throw new Exception());
            parts.Add(key, externalPart);
            registry.RegisterPart(Juno.parts[name]);
        }

        private void addArtifact(
          string name,
          string art,
          string desc,
          Type artifact,
          IArtifactRegistry registry)
        {
            Juno.artifacts.Add(name, new ExternalArtifact("starlight.harbor" + name, artifact, Juno.sprites[art], (IEnumerable<ExternalGlossary>)new ExternalGlossary[0], (ExternalDeck)null, (IEnumerable<int>)null, (IEnumerable<string>)new string[1]));
            Juno.artifacts[name].AddLocalisation(name, desc);
            registry.RegisterArtifact(Juno.artifacts[name]);
        }

        public void LoadManifest(ISpriteRegistry artRegistry)
        {
            this.addSprite("juno_cockpit", artRegistry);
            this.addSprite("juno_cannon", artRegistry);
            this.addSprite("juno_launcher", artRegistry);
            this.addSprite("juno_wing_a", artRegistry);
            this.addSprite("juno_wing_b", artRegistry);
            this.addSprite("juno_structure", artRegistry);
            //non-part
            this.addSprite("juno_chassis", artRegistry);
            this.addSprite("juno_plating", artRegistry);
            this.addSprite("juno_heart", artRegistry);

        }

        public void LoadManifest(IShipPartRegistry registry)
        {
            //add parts here, flip boolean
            //oh boy, different naming conventions
            this.addPart("wing0", "juno_wing_b", (PType)3, false, (PDamMod) 2, registry);
            this.addPart("launcher0", "juno_launcher", (PType)2, false, 0, registry);
            this.addPart("cannon0", "juno_cannon", (PType)1, false, 0, registry);
            this.addPart("structure0", "juno_structure", (PType)4, false, 0, registry);
            this.addPart("wing1", "juno_wing_a", (PType)3, false, (PDamMod) 2, registry);
            this.addPart("cockpit", "juno_cockpit", (PType)0, false, 0, registry);
            this.addPart("wing2", "juno_wing_a", (PType)3, false, (PDamMod) 2, registry);
            this.addPart("structure1", "juno_structure", (PType)4, false, 0, registry);
            this.addPart("cannon1", "juno_cannon", (PType)1, false, 0, registry);
            this.addPart("launcher1", "juno_launcher", (PType)2, false, 0, registry);
            this.addPart("wing3", "juno_wing_b", (PType)3, false, (PDamMod) 2, registry);

        }

        public void LoadManifest(IArtifactRegistry registry)
        {
            this.addArtifact("JUNO HEAVY SHIELD", "juno_plating", " Gain <c=keyword>3</c> more <c=healing>hull</c> and <c=hull>max hull</c> when you defeat a boss. <c=downside>You can not have more than 2 <c=status>EVADE</c> at a time. At the start of each turn, lose 1 <c=status>SHIELD</c>.</c>", typeof(JunoPlating), registry);
            this.addArtifact("JUNO REACTOR", "juno_heart", "Every 4 times being hit, gain 1 <c=energy>ENERGY.</c>", typeof(JunoHeart), registry);
        }

        public void LoadManifest(IShipRegistry shipRegistry)
        {
            Ship shipObjectTemplate = new Ship();
            shipObjectTemplate.baseDraw = 5;
            shipObjectTemplate.baseEnergy = 3;
            shipObjectTemplate.heatTrigger = 3;
            shipObjectTemplate.heatMin = 0;
            shipObjectTemplate.hull = 18;
            shipObjectTemplate.hullMax = 18;
            shipObjectTemplate.shieldMaxBase = 5;
            //left to right, ship made here
            ExternalPart[] parts = new ExternalPart[11]
            {
                Juno.parts["wing0"],
                Juno.parts["launcher0"],
                Juno.parts["cannon0"],
                Juno.parts["structure0"],
                Juno.parts["wing1"],
                Juno.parts["cockpit"],
                Juno.parts["wing2"],
                Juno.parts["structure1"],
                Juno.parts["cannon1"],
                Juno.parts["launcher1"],
                Juno.parts["wing3"],
            };
            this.juno = new ExternalShip("starlight.harbor.junoShip", (object)shipObjectTemplate, (IEnumerable<ExternalPart>)parts, Juno.sprites["juno_chassis"] ?? throw new Exception());
            shipRegistry.RegisterShip(this.juno);
        }

        public void LoadManifest(IStartershipRegistry registry)
        {
            if (this.juno == null)
                return;
            ExternalStarterShip starterShip = new ExternalStarterShip("starlight.harbor.junoStarter", this.juno.GlobalName, (IEnumerable<ExternalCard>)new ExternalCard[0], (IEnumerable<ExternalArtifact>)new ExternalArtifact[1]
            {
               //change from 0 to 1 when uncommenting
               Juno.artifacts["JUNO HEAVY SHIELD"]
            }, (IEnumerable<Type>)new Type[3]
            {
                typeof (DodgeColorless),
                typeof (BasicShieldColorless),
                typeof (CannonColorless)
            }, (IEnumerable<Type>)new Type[1]
            {
                new ShieldPrep().GetType()
            }, (IEnumerable<ExternalArtifact>)new ExternalArtifact[1]
            {
                Juno.artifacts["JUNO REACTOR"]
            });
            starterShip.AddLocalisation(nameof(Juno), "Enormous, glacial, and utterly unstoppable.");
            registry.RegisterStartership(starterShip);
        }
    }
}