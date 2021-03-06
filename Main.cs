﻿using JetBrains.Annotations;
using SandBox.View.Map;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade;

namespace Accompany
{
    [UsedImplicitly]
    public class Main : MBSubModuleBase
    {
        public override void OnCampaignStart(Game game, object starterObject)
        {
            if (!(game.GameType is Campaign)) return;
            var gameInitializer = (CampaignGameStarter) starterObject;
            gameInitializer.AddBehavior(new AccompanyCampaignBehavior());
        }

        public override void OnGameLoaded(Game game, object initializerObject)
        {
            if (!(game.GameType is Campaign)) return;
            var gameInitializer = (CampaignGameStarter) initializerObject;
            gameInitializer.AddBehavior(new AccompanyCampaignBehavior());
        }

        protected override void OnApplicationTick(float dt)
        {
            if (!PartyInputUtils.IsInitialized || !(ScreenManager.TopScreen is MapScreen) ||
                MapScreen.Instance.IsEscapeMenuOpened || Campaign.Current == null || !Campaign.Current.GameStarted ||
                !Input.IsKeyPressed(InputKey.RightMouseButton) ||
                MapScreen.Instance.EncyclopediaScreenManager.IsEncyclopediaOpen) return;
            if (!(Game.Current.GameStateManager.ActiveState is MapState mapState) || mapState.AtMenu ||
                PlayerCaptivity.IsCaptive) return;
            var party = PartyInputUtils.GetHoverParty();
            if (party == null || party.MobileParty.IsMainParty || !party.IsMobile || !party.IsVisible) return;
            PartyInfoLayer.Instance.DataSource.IsShowInEncyclopediaVisible = party.MobileParty.IsLordParty;
            PartyInfoLayer.Instance.DataSource.IsVisible = true;
            PartyInfoLayer.Instance.DataSource.PositionX = (int) Input.MousePositionPixel.X;
            PartyInfoLayer.Instance.DataSource.PositionY = (int) Input.MousePositionPixel.Y;
            PartyInfoLayer.Instance.DataSource.ClickedParty = party;
        }
    }
}
