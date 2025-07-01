using System.Numerics;
using System.Reflection;
using JetBrains.Annotations;
using SandBox.View.Map;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Accompany
{
    internal class PartyInfoLayerViewModel : ViewModel
    {
        private PartyBase _clickedParty;
        private Vector2 _mousePosition;
        private int _positionX, _positionY;
        private bool _isVisible, _isShowInEncyclopediaVisible;

        public void Tick()
        {
            if (!PartyInfoLayer.Added) return;
            if (MapScreen.Instance != null)
            {
                if (Input.IsKeyPressed(InputKey.LeftMouseButton))
                {
                    _mousePosition.X = Input.MousePositionPixel.X;
                    _mousePosition.Y = Input.MousePositionPixel.Y;
                    if (!PartyInfoLayer.Instance.Movie.RootView.Target.IsPointInsideMeasuredArea(_mousePosition))
                    {
                        IsVisible = false;
                    }                    
                }

                if (MapScreen.Instance.IsEscapeMenuOpened)
                {
                    IsVisible = false;
                }
            }
            else if (Input.IsKeyPressed(InputKey.Escape))
            {
                IsVisible = false;
            }
        }

        [UsedImplicitly]
        private void ExecuteAccompany()
        {
            MobileParty.MainParty.Ai.SetMoveEscortParty(ClickedParty.MobileParty);
            if (Campaign.Current.GetSimplifiedTimeControlMode() == CampaignTimeControlMode.Stop)
            {
                Campaign.Current.SetTimeSpeed(1);
            }
            
            MapCameraViewHelper.GetInstance().SetCameraMode(MapCameraView.CameraFollowMode.FollowParty);
            PartyBase.MainParty.SetAsCameraFollowParty();
            IsVisible = false;
        }

        [UsedImplicitly]
        private void ExecuteShowInEncyclopedia()
        {
            if (ClickedParty.IsMobile && ClickedParty.MobileParty.IsLordParty)
            {
                Campaign.Current.EncyclopediaManager.GoToLink(ClickedParty.MobileParty.LeaderHero.EncyclopediaLink);
            }
        }

        public PartyBase ClickedParty
        {
            get => _clickedParty;
            set
            {
                if (value == _clickedParty) return;
                _clickedParty = value;
                OnPropertyChanged(nameof(PartyName));
            }
        }

        [DataSourceProperty]
        public int PositionX
        {
            get => (int) (_positionX / (PartyInfoLayer.Instance == null ? 1f : PartyInfoLayer.Instance.Movie.Context.Scale));
            set
            {
                if (value == _positionX) return;
                _positionX = value;
                var rootSize = Screen.RealScreenResolution;
                var size = PartyInfoLayer.Instance.Movie.RootView.Target.MeasuredSize;
                if (_positionX + size.X > rootSize.X)
                {
                    _positionX = (int) (rootSize.X - size.X);
                }
                PartyInfoLayer.Instance.Layer.UpdateLayout();
            }
        }

        [DataSourceProperty]
        public int PositionY
        {
            get => (int) (_positionY / (PartyInfoLayer.Instance == null ? 1f : PartyInfoLayer.Instance.Movie.Context.Scale));
            set
            {
                if (value == _positionY) return;
                _positionY = value;
                var rootSize = Screen.RealScreenResolution;
                var size = PartyInfoLayer.Instance.Movie.RootView.Target.MeasuredSize;
                if (_positionY + size.Y > rootSize.Y)
                {
                    _positionY = (int)(rootSize.Y - size.Y);
                }
                PartyInfoLayer.Instance.Layer.UpdateLayout();
            }
        }

        [DataSourceProperty]
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (value == _isVisible) return;
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        [DataSourceProperty]
        public bool IsShowInEncyclopediaVisible
        {
            get => _isShowInEncyclopediaVisible;
            set
            {
                if (value == _isShowInEncyclopediaVisible) return;
                _isShowInEncyclopediaVisible = value;
                OnPropertyChanged(nameof(IsShowInEncyclopediaVisible));
            }
        }

        [DataSourceProperty]
        public string PartyName => ClickedParty == null
            ? new TextObject("{=BINjcIsY}Unknown Party").ToString()
            : ClickedParty.Name.ToString();

        [DataSourceProperty]
        public string AccompanyText => new TextObject("{=LxdXABCM}Accompany").ToString();
        
        [DataSourceProperty]
        public string ShowInEncyclopediaText => new TextObject("{=68lxzu0R}Show in Encyclopedia").ToString();
    }
    
    public static class MapCameraViewHelper
    {
        private static MapCameraView _cachedInstance;
    
        public static MapCameraView GetInstance()
        {
            if (_cachedInstance != null)
            {
                return _cachedInstance;
            }
            
            var instanceProperty = typeof(MapCameraView).GetProperty(
                "Instance", 
                BindingFlags.Static | BindingFlags.NonPublic);
            
            _cachedInstance = instanceProperty?.GetValue(null) as MapCameraView;
            return _cachedInstance;
        }
    }

}
