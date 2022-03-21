using SandBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace Caravanalyzer
{
    internal class CaravanCombatMarker
    {
        private Material material;
        private readonly Decal decal;
        private readonly GameEntity decalEntity;

        internal CaravanCombatMarker()
        {
            // Process cribbed from MapCursor.Initialize()
            MapScene mapScene = Campaign.Current.MapSceneWrapper as MapScene;

            decal = Decal.CreateDecal();

            decalEntity = GameEntity.CreateEmpty(mapScene.Scene);
            decalEntity.Name = "caravanCombatDecal";
            decalEntity.AddComponent(decal);

            mapScene.Scene.AddDecalInstance(decal, "editor_set", deletable: true);
        }

        internal void ReconfigureFor(CaravanCombatLogEntry logEntry)
        {
            SetPositionFor(logEntry);
            SetDecalMaterialFor(logEntry.winningSide);
        }

        private void SetPositionFor(CaravanCombatLogEntry logEntry)
        {
            MapScene mapScene = Campaign.Current.MapSceneWrapper as MapScene;
            MatrixFrame frame = decalEntity.GetFrame();
            float height = 0;
            mapScene.GetHeightAtPoint(logEntry.location, ref height);
            Vec3 frameOrigin = new Vec3(logEntry.location.X, logEntry.location.y, height);
            frame.origin = frameOrigin;
            frame.rotation = Mat3.Identity;
            frame.Scale(Vec3.One * 10f);
            decalEntity.SetFrame(ref frame);
        }

        private void SetDecalMaterialFor(BattleSideEnum winningSide)
        {
            string decalResource;
            switch (winningSide)
            {
                // borrowing the map cursor decals until I make my own.
                // treating attacker running away as a victory mostly because I don't have a separate decal.
                case BattleSideEnum.Attacker:
                    decalResource = "map_cursor_invalid_decal";
                    break;
                default:
                    decalResource = "map_cursor_valid_decal";
                    break;
            }
            material = Material.GetFromResource(decalResource);
            decal.SetMaterial(material);
        }

        public void Dispose()
        {
            decalEntity.ManualInvalidate();
            decalEntity.Remove(0);
        }
    }
}
