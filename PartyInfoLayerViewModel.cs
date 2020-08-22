using SandBox.View.Map;
using System.Numerics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace Accompany
{
    class PartyInfoLayerViewModel : ViewModel
    {
        private PartyBase _followParty;
        private Vector2 _mousePosition;
        private int _positionX, _positionY;
        private bool _isVisible;

        public void Tick(float dt)
        {
            if (!PartyInfoLayer.Added) return;
            if (MapScreen.Instance != null && Input.IsKeyPressed(InputKey.LeftMouseButton))
            {
                _mousePosition.X = Input.MousePositionPixel.X;
                _mousePosition.Y = Input.MousePositionPixel.Y;
                if (!PartyInfoLayer.Instance.Movie.RootView.Target.IsPointInsideMeasuredArea(_mousePosition))
                {
                    IsVisible = false;
                }
            }
            else if (Input.IsKeyPressed(InputKey.Escape))
            {
                IsVisible = false;
            }
        }

        private void ExecuteAccompany()
        {
            MobileParty.MainParty.SetMoveEscortParty(FollowParty.MobileParty);
            if (Campaign.Current.GetSimplifiedTimeControlMode() == CampaignTimeControlMode.Stop)
            {
                Campaign.Current.SetTimeSpeed(1);
            }
            MapScreen.Instance.CurrentCameraFollowMode = MapScreen.CameraFollowMode.FollowParty;
            Campaign.Current.CameraFollowParty = PartyBase.MainParty;
            IsVisible = false;
        }

        public PartyBase FollowParty
        {
            get => _followParty;
            set
            {
                if (value == _followParty) return;
                _followParty = value;
                OnPropertyChanged("PartyName");
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
                OnPropertyChanged("IsVisible");
            }
        }

        [DataSourceProperty]
        public string PartyName => FollowParty == null ? "Party" : FollowParty.Name.ToString();
    }
}
