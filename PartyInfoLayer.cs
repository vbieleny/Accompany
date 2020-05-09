using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.Library;

namespace Accompany
{
    class PartyInfoLayer : GlobalLayer
    {
        public static PartyInfoLayer Instance { get; private set; }
        public PartyInfoLayerViewModel DataSource { get; } = new PartyInfoLayerViewModel();
        public static bool Added { get; private set; } = false;

        public readonly GauntletMovie Movie;

        private PartyInfoLayer()
        {
            var gauntletLayer = new GauntletLayer(1);
            gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            Movie = gauntletLayer.LoadMovie("PartyInfoLayer", DataSource);
            Layer = gauntletLayer;
        }

        public static void OnInitialize()
        {
            if (Instance == null)
            {
                Instance = new PartyInfoLayer();
            }
        }

        public static void AddToGlobalLayer()
        {
            Added = true;
            ScreenManager.AddGlobalLayer(PartyInfoLayer.Instance, true);
        }

        public static void RemoveFromGlobalLayer()
        {
            Added = false;
            ScreenManager.RemoveGlobalLayer(PartyInfoLayer.Instance);
        }

        protected override void OnTick(float dt)
        {
            base.OnTick(dt);
            DataSource.Tick(dt);
        }
    }
}