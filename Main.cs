using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade;

namespace Accompany
{
    public class Main : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            PartyInfoLayer.OnInitialize();
            PartyInfoLayer.AddToGlobalLayer();
        }

        public override void OnGameInitializationFinished(Game game)
        {
            PartyInputUtils.OnInitialize();
        }

        protected override void OnApplicationTick(float dt)
        {
            if (Campaign.Current != null && Campaign.Current.GameStarted && Input.IsKeyPressed(InputKey.RightMouseButton))
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
