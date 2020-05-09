using TaleWorlds.CampaignSystem;

namespace Accompany
{
    class AccompanyCampaignBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, OnGameStart);
            CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, OnGameStart);
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
    }
}
