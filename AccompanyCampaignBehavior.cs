using TaleWorlds.CampaignSystem;

namespace Accompany
{
    class AccompanyCampaignBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, OnGameLoaded);
        }

        public override void SyncData(IDataStore dataStore)
        {
        }

        private void OnGameLoaded(CampaignGameStarter campaignGameStarter)
        {
            PartyInputUtils.OnInitialize();
            PartyInfoLayer.OnInitialize();
            PartyInfoLayer.AddToGlobalLayer();
        }
    }
}
