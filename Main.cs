using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade;

namespace Accompany
{
    public class Main : MBSubModuleBase
    {
        public override void OnCampaignStart(Game game, object starterObject)
        {
            if (game.GameType is Campaign campaign)
            {
                CampaignGameStarter gameInitializer = (CampaignGameStarter)starterObject;
                gameInitializer.AddBehavior(new AccompanyCampaignBehavior());
            }
        }

        public override void OnGameLoaded(Game game, object initializerObject)
        {
            if (game.GameType is Campaign campaign)
            {
                CampaignGameStarter gameInitializer = (CampaignGameStarter)initializerObject;
                gameInitializer.AddBehavior(new AccompanyCampaignBehavior());
            }
        }

        protected override void OnApplicationTick(float dt)
        {
            if (PartyInputUtils.IsInitialized && Campaign.Current != null && Campaign.Current.GameStarted && Input.IsKeyPressed(InputKey.RightMouseButton))
            {
                PartyBase party = PartyInputUtils.GetHoverParty();
                if (party != null && !party.MobileParty.IsMainParty)
                {
                    PartyInfoLayer.Instance.DataSource.IsVisible = true;
                    PartyInfoLayer.Instance.DataSource.PositionX = (int)(Input.MousePositionPixel.X);
                    PartyInfoLayer.Instance.DataSource.PositionY = (int)(Input.MousePositionPixel.Y);
                    PartyInfoLayer.Instance.DataSource.FollowParty = party;
                }
            }
        }
    }
}
