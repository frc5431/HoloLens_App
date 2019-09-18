using NetworkTables;
using System;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace HoloLens
{
    public sealed partial class MainPage : Page
    {
        private static readonly double RefreshPerSecond = 20;
        private static readonly TimeSpan period = TimeSpan.FromSeconds(1 / RefreshPerSecond);
        private readonly int Team = 5431;
        private readonly NetworkTable Fms;
        private readonly ThreadPoolTimer PeriodicTimer;

        public MainPage()
        {
            InitializeComponent();

            NetworkTable.SetClientMode();
            NetworkTable.SetTeam(Team);
            NetworkTable.Initialize();

            Fms = NetworkTable.GetTable("FMSInfo");
            Threaded();
            PeriodicTimer = ThreadPoolTimer.CreatePeriodicTimer((source) => Threaded(), period);
        }

        ~MainPage()
        {
            PeriodicTimer.Cancel();
        }

        private async void Threaded()
        {
            String time         = Fms.GetString("a","0:00");
            int redScore        = Convert.ToInt32(Fms.GetNumber("b", 0));
            int blueScore       = Convert.ToInt32(Fms.GetNumber("c", 0));

            await Dispatcher.RunAsync(CoreDispatcherPriority.High,
                () => updateTextBox(time, redScore, blueScore));
        }

        private void updateTextBox(String time, int red, int blue)
        {
            TextBox.Text =
                $"Time Left: {time}\n" +
                $"Red: {red} || Blue: {blue}";
        }
    }
}
