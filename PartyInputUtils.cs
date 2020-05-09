using SandBox;
using SandBox.View.Map;
using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace Accompany
{
    internal static class PartyInputUtils
    {
        private static Dictionary<GameEntity, PartyVisual> _visualsOfEntities;
        private static Ray _mouseRay = new Ray(Vec3.Zero, Vec3.Up, float.MaxValue);
        private static readonly GameEntity[] _intersectedEntities = new GameEntity[32];
        private static readonly Intersection[] _intersectedInfos = new Intersection[32];

        public static bool IsInitialized { get; private set; }

        public static void OnInitialize()
        {
            try
            {
                _visualsOfEntities = (Dictionary<GameEntity, PartyVisual>) (typeof(MapScreen)
                    .GetProperty("VisualsOfEntities",
                        BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    .GetValue(null));
                IsInitialized = true;
            }
            catch (Exception)
            {
                InformationManager.ShowInquiry(new InquiryData("Error in initializing Accompany", "Accompany mod could not be initialized. Game will continue without this mod.", 
                    true, false, "I understand", null, null, null));
            }
        }

        public static PartyBase GetHoverParty()
        {
            PartyBase hoverParty = null;
            Scene scene = ((MapScene)Campaign.Current.MapSceneWrapper).Scene;
            Vec3 near = Vec3.Zero, far = Vec3.Zero;
            MapScreen.Instance.SceneLayer.SceneView.TranslateMouse(ref near, ref far);
            Vec3 pos = far - near;
            float maxDistance = pos.Normalize();
            _mouseRay.Reset(near, pos, maxDistance);
            int size = scene.SelectEntitiesCollidedWith(ref _mouseRay, _intersectedEntities, _intersectedInfos);
            for (int i = 0; i < size; ++i)
            {
                if (_intersectedEntities[i] == null) continue;
                if (!_visualsOfEntities.ContainsKey(_intersectedEntities[i]) || !_intersectedEntities[i].IsVisibleIncludeParents()) continue;
                PartyBase party = PartyBase.FindParty(_visualsOfEntities[_intersectedEntities[i]].PartyIndex);
                if (!party.IsMobile) continue;
                hoverParty = party;
                break;
            }
            Array.Clear(_intersectedEntities, 0, size);
            Array.Clear(_intersectedInfos, 0, size);
            return hoverParty;
        }
    }
}
