using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace Accompany
{
    class AccompanyCampaignBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, OnGameStart);
            CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, OnGameStart);
            CampaignEvents.OnGameOverEvent.AddNonSerializedListener(this, OnGameEnd);
            Game.Current.EventManager.RegisterEvent<TutorialContextChangedEvent>(OnMapChanged);
        }

        public override void SyncData(IDataStore dataStore)
        {
        }

        private void OnGameStart(CampaignGameStarter campaignGameStarter)
        {
            PartyInputUtils.OnInitialize();
            PartyInfoLayer.OnInitialize();
            PartyInfoLayer.AddToGlobalLayer();
        }

        private void OnGameEnd()
        {
            PartyInfoLayer.Instance.DataSource.IsVisible = false;
        }

        private void OnMapChanged(TutorialContextChangedEvent changeEvent)
        {
            if (changeEvent.NewContext == TutorialContexts.MapWindow)
            {
                if (!PartyInfoLayer.Added)
                {
                    PartyInfoLayer.AddToGlobalLayer();
                }
            }
            else if (changeEvent.NewContext == TutorialContexts.None)
            {
                if (PartyInfoLayer.Added)
                {
                    PartyInfoLayer.Instance.DataSource.IsVisible = false;
                    PartyInfoLayer.RemoveFromGlobalLayer();
                }
            }
        }
    }
}
