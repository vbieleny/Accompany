using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace Accompany
{
    internal class AccompanyCampaignBehavior : CampaignBehaviorBase
    {
        private TutorialContexts _lastContext = TutorialContexts.None;

        public override void RegisterEvents()
        {
            CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, OnGameStart);
            CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, OnGameStart);
            CampaignEvents.OnGameOverEvent.AddNonSerializedListener(this, OnGameEnd);
            CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, OnGameMenuOpened);
            CampaignEvents.PartyVisibilityChangedEvent.AddNonSerializedListener(this, OnPartyVisibilityChanged);
            Game.Current.EventManager.RegisterEvent<TutorialContextChangedEvent>(OnMapChanged);
        }

        public override void SyncData(IDataStore dataStore)
        {
        }

        private static void OnGameStart(CampaignGameStarter campaignGameStarter)
        {
            PartyInfoLayer.OnInitialize();
        }

        private static void OnGameEnd()
        {
            PartyInfoLayer.Instance.DataSource.IsVisible = false;
        }

        private void OnMapChanged(TutorialContextChangedEvent changeEvent)
        {
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (changeEvent.NewContext)
            {
                case TutorialContexts.MapWindow:
                    AddToGlobalLayer();
                    break;
                case TutorialContexts.EncyclopediaWindow:
                    RemoveFromGlobalLayer();
                    break;
                default:
                {
                    if (_lastContext == TutorialContexts.EncyclopediaWindow &&
                        changeEvent.NewContext == TutorialContexts.None)
                    {
                        AddToGlobalLayer();
                    }
                    else if (changeEvent.NewContext == TutorialContexts.None)
                    {
                        RemoveFromGlobalLayer();
                    }

                    break;
                }
            }

            _lastContext = changeEvent.NewContext;
        }

        private static void OnGameMenuOpened(MenuCallbackArgs args)
        {
            if (Game.Current.GameStateManager.ActiveState is MapState mapState && mapState.AtMenu)
            {
                PartyInfoLayer.Instance.DataSource.IsVisible = false;
            }
        }

        private static void OnPartyVisibilityChanged(PartyBase party)
        {
            if (!party.IsVisible && PartyInfoLayer.Instance.DataSource.ClickedParty == party)
            {
                PartyInfoLayer.Instance.DataSource.IsVisible = false;
            }
        }

        private static void AddToGlobalLayer()
        {
            if (!PartyInfoLayer.Added)
            {
                PartyInfoLayer.AddToGlobalLayer();
            }
        }

        private static void RemoveFromGlobalLayer()
        {
            if (!PartyInfoLayer.Added) return;
            PartyInfoLayer.Instance.DataSource.IsVisible = false;
            PartyInfoLayer.RemoveFromGlobalLayer();
        }
    }
}
