using System;
using SandBox;
using SandBox.View.Map;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace Accompany
{
    internal static class PartyInputUtils
    {
        private static Ray _mouseRay = new Ray(Vec3.Zero, Vec3.Up);
        private static readonly UIntPtr[] IntersectedEntityIDs = new UIntPtr[128];
        private static readonly Intersection[] IntersectedInfos = new Intersection[128];

        public static PartyBase GetHoverParty()
        {
            PartyBase hoverParty = null;
            var scene = ((MapScene) Campaign.Current.MapSceneWrapper).Scene;
            Vec3 near = Vec3.Zero, far = Vec3.Zero;
            MapScreen.Instance.SceneLayer.SceneView.TranslateMouse(ref near, ref far);
            var pos = far - near;
            var maxDistance = pos.Normalize();
            _mouseRay.Reset(near, pos, maxDistance);
            var size = scene.SelectEntitiesCollidedWith(ref _mouseRay, IntersectedInfos, IntersectedEntityIDs);
            for (var i = 0; i < size; ++i)
            {
                var intersectedEntityId = IntersectedEntityIDs[i];
                if (intersectedEntityId == UIntPtr.Zero)
                {
                    continue;
                }
                
                MapScreen.VisualsOfEntities.TryGetValue(intersectedEntityId, out var visualEntity);
                if (visualEntity == null || !visualEntity.StrategicEntity.IsVisibleIncludeParents())
                {
                    continue;
                }
                
                var party = visualEntity.PartyBase;
                if (!party.IsMobile) continue;
                hoverParty = party;
                break;
            }

            Array.Clear(IntersectedEntityIDs, 0, size);
            Array.Clear(IntersectedInfos, 0, size);
            return hoverParty;
        }
    }
}
