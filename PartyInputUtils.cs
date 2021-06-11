using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SandBox;
using SandBox.View.Map;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Accompany
{
    internal static class PartyInputUtils
    {
        private static Dictionary<GameEntity, PartyVisual> _visualsOfEntities;
        private static Ray _mouseRay = new Ray(Vec3.Zero, Vec3.Up);
        private static readonly GameEntity[] IntersectedEntities = new GameEntity[128];
        private static readonly UIntPtr[] IntersectedEntityIDs = new UIntPtr[128];
        private static readonly Intersection[] IntersectedInfos = new Intersection[128];

        public static bool IsInitialized { get; private set; }

        public static void OnInitialize()
        {
            try
            {
                _visualsOfEntities = (Dictionary<GameEntity, PartyVisual>) typeof(MapScreen)
                    .GetProperty("VisualsOfEntities",
                        BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    ?.GetValue(null);
                IsInitialized = true;
            }
            catch (Exception)
            {
                var title = new TextObject("{=2grG3IdZ}Error in initializing Accompany");
                var text = new TextObject("{=R1SnVOSp}Accompany mod could not be initialized. Game will continue without this mod.");
                var affirmativeText = new TextObject("{=iaYZG73X}I understand");
                InformationManager.ShowInquiry(new InquiryData(title.ToString(), text.ToString(), true, false,
                    affirmativeText.ToString(), null, null, null));
            }
        }

        public static PartyBase GetHoverParty()
        {
            PartyBase hoverParty = null;
            var scene = ((MapScene) Campaign.Current.MapSceneWrapper).Scene;
            Vec3 near = Vec3.Zero, far = Vec3.Zero;
            MapScreen.Instance.SceneLayer.SceneView.TranslateMouse(ref near, ref far);
            var pos = far - near;
            var maxDistance = pos.Normalize();
            _mouseRay.Reset(near, pos, maxDistance);
            var size = scene.SelectEntitiesCollidedWith(ref _mouseRay, IntersectedEntities, IntersectedInfos,
                IntersectedEntityIDs);
            for (var i = 0; i < size; ++i)
            {
                if (IntersectedEntities[i] == null) continue;
                if (!_visualsOfEntities.ContainsKey(IntersectedEntities[i]) ||
                    !IntersectedEntities[i].IsVisibleIncludeParents()) continue;
                var party = FindParty(_visualsOfEntities[IntersectedEntities[i]].PartyIndex);
                if (!party.IsMobile) continue;
                hoverParty = party;
                break;
            }

            Array.Clear(IntersectedEntities, 0, size);
            Array.Clear(IntersectedEntityIDs, 0, size);
            Array.Clear(IntersectedInfos, 0, size);
            return hoverParty;
        }
        
        private static PartyBase FindParty(int index)
        {
            var mobileParty = Campaign.Current.CampaignObjectManager.Find<MobileParty>(x => x.Party.Index == index);
            return mobileParty != null ? mobileParty.Party : Settlement.All.FirstOrDefault(x => x.Party.Index == index)?.Party;
        }
    }
}
