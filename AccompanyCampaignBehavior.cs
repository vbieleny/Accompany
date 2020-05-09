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
    }
}
