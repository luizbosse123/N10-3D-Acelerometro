using System;
using Urho;
using Urho.Forms;
using Xamarin.Forms;
using DeviceMotion.Plugin;
using DeviceMotion.Plugin.Abstractions;
using System.Diagnostics;

namespace UrhoSharp
{
    public class App : Xamarin.Forms.Application
    {
        public App()
        {
            MainPage = new NavigationPage(new UrhoPage { });
        }
    }

    public class UrhoPage : ContentPage
    {
        UrhoSurface urhoSurface;
        Charts urhoApp;
        Slider selectedBarSlider;
        double valorX, valorY, valorZ;

        public UrhoPage()
        {
            var restartBtn = new Button { Text = "Restart" };
            restartBtn.Clicked += (sender, e) => StartUrhoApp();

            urhoSurface = new UrhoSurface();
            urhoSurface.VerticalOptions = LayoutOptions.FillAndExpand;

            Slider rotationSlider = new Slider(0, 500, 250);

            CrossDeviceMotion.Current.Start(MotionSensorType.Accelerometer, MotionSensorDelay.Game);
            CrossDeviceMotion.Current.SensorValueChanged += (s, a) =>
            {

                valorX = Math.Round(((MotionVector)a.Value).X, 2);
                valorY = Math.Round(((MotionVector)a.Value).Y, 2);
                valorZ = Math.Round(((MotionVector)a.Value).Z, 2);

                Debug.WriteLine("A: {0},{1},{2}", valorX, valorY, valorZ);
                urhoApp?.Rotate((float)(valorX));                

            };

            selectedBarSlider = new Slider(0, 5, 2.5);
            selectedBarSlider.ValueChanged += OnValuesSliderValueChanged;

            Title = " UrhoSharp + Xamarin.Forms";
            Content = new StackLayout
            {
                Padding = new Thickness(12, 12, 12, 40),
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    urhoSurface,
                    restartBtn,
                    new Label { Text = "ROTATION::" },
                    rotationSlider,
                    new Label { Text = "SELECTED VALUE:" },
                    selectedBarSlider,
                }
            };
        }

        protected override void OnDisappearing()
        {
            UrhoSurface.OnDestroy();
            base.OnDisappearing();
        }

        void OnValuesSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (urhoApp?.SelectedBar != null)
                urhoApp.SelectedBar.Value = (float)e.NewValue;
        }

        private void OnBarSelection(Bar bar)
        {
            //reset value
            selectedBarSlider.ValueChanged -= OnValuesSliderValueChanged;
            selectedBarSlider.Value = bar.Value;
            selectedBarSlider.ValueChanged += OnValuesSliderValueChanged;
        }

        protected override async void OnAppearing()
        {
            StartUrhoApp();
        }

        async void StartUrhoApp()
        {
            urhoApp = await urhoSurface.Show<Charts>(new ApplicationOptions(assetsFolder: null) { Orientation = ApplicationOptions.OrientationType.LandscapeAndPortrait });
        }
    }
}