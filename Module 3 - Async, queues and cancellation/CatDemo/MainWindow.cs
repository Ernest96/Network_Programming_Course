using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;

namespace CatDemo
{
    // One window, one UI thread.
    //
    // The UI thread draws the window, moves the cat and handles the clicks.
    //   Sync  - the UI thread counts the primes itself. Until it is done it can
    //           do nothing else: the cat freezes, the clock stops, clicks wait.
    //   Async - a pool thread counts (Task.Run), the UI thread only awaits.
    //           It stays free the whole time, so the cat keeps spinning.
    class MainWindow : Window
    {
        static readonly IBrush Lime = new SolidColorBrush(Color.Parse("#D9F201"));
        static readonly IBrush Muted = new SolidColorBrush(Color.Parse("#9AA095"));

        readonly int uiThread = Environment.CurrentManagedThreadId;

        readonly Image cat = new Image();
        readonly TextBlock clock = new TextBlock();
        readonly NumericUpDown limit = new NumericUpDown();
        readonly Button syncButton = new Button();
        readonly Button asyncButton = new Button();
        readonly TextBlock result = new TextBlock();

        public MainWindow()
        {
            Title = "Module 3 - sync vs async";
            Width = 560;
            Height = 680;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Background = Brushes.Black;

            BuildLayout();

            GifPlayer player = new GifPlayer(cat, Path.Combine(AppContext.BaseDirectory, "cat.gif"));
            player.Start();

            // A second sign of life: a clock the UI thread updates every 100 ms.
            DispatcherTimer clockTimer = new DispatcherTimer();
            clockTimer.Interval = TimeSpan.FromMilliseconds(100);
            clockTimer.Tick += (sender, e) => clock.Text = $"UI thread {uiThread} is alive: {DateTime.Now:HH:mm:ss.f}";
            clockTimer.Start();

            syncButton.Click += OnSyncClick;
            asyncButton.Click += OnAsyncClick;
        }

        // SYNC: the UI thread does the work itself.
        void OnSyncClick(object sender, RoutedEventArgs e)
        {
            int max = GetLimit();
            result.Text = "Sync: counting...";     // you never see this - the UI thread is
                                                   // too busy to draw it
            Stopwatch sw = Stopwatch.StartNew();
            int count = CountPrimes(max);          // the UI thread is stuck here
            sw.Stop();

            ShowResult("Sync", count, max, sw.ElapsedMilliseconds, Environment.CurrentManagedThreadId);
        }

        // ASYNC: a pool thread does the work, the UI thread only waits for it.
        // async void is allowed here - and only here: this is an event handler.
        async void OnAsyncClick(object sender, RoutedEventArgs e)
        {
            int max = GetLimit();
            syncButton.IsEnabled = false;
            asyncButton.IsEnabled = false;
            result.Text = "Async: counting... the cat keeps spinning";

            Stopwatch sw = Stopwatch.StartNew();
            int worker = 0;
            int count = await Task.Run(() =>
            {
                worker = Environment.CurrentManagedThreadId;
                return CountPrimes(max);           // runs on a pool thread
            });
            sw.Stop();

            // After await we are back on the UI thread - only it may touch the window.
            ShowResult("Async", count, max, sw.ElapsedMilliseconds, worker);
            syncButton.IsEnabled = true;
            asyncButton.IsEnabled = true;
        }

        void ShowResult(string mode, int count, int max, long ms, int thread)
        {
            string who;
            if (thread == uiThread)
                who = "the UI thread itself - the cat had to wait";
            else
                who = "a pool thread - the UI thread stayed free";

            result.Text = $"{mode}: {count:N0} primes below {max:N0} in {ms:N0} ms\n" +
                          $"counted on thread {thread}, {who}";
        }

        int GetLimit()
        {
            decimal value = limit.Value ?? 20_000_000;
            return (int)value;
        }

        // CPU work: count the primes below max, one number at a time.
        static int CountPrimes(int max)
        {
            int count = 0;
            for (int n = 2; n < max; n++)
                if (IsPrime(n))
                    count++;
            return count;
        }

        static bool IsPrime(int n)
        {
            if (n < 2) return false;
            if (n % 2 == 0) return n == 2;

            for (int d = 3; (long)d * d <= n; d += 2)
                if (n % d == 0)
                    return false;

            return true;
        }

        void BuildLayout()
        {
            StackPanel panel = new StackPanel { Margin = new Thickness(28), Spacing = 16 };

            panel.Children.Add(new TextBlock
            {
                Text = "sync vs async",
                FontSize = 28,
                FontWeight = FontWeight.Bold,
                Foreground = Lime,
            });
            panel.Children.Add(new TextBlock
            {
                Text = "Press a button and watch the cat.",
                Foreground = Muted,
            });

            cat.Width = 340;
            cat.Height = 231;
            panel.Children.Add(new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(14),
                Padding = new Thickness(8),
                HorizontalAlignment = HorizontalAlignment.Center,
                Child = cat,
            });

            clock.Foreground = Muted;
            clock.HorizontalAlignment = HorizontalAlignment.Center;
            panel.Children.Add(clock);

            limit.Minimum = 1_000_000;
            limit.Maximum = 200_000_000;
            limit.Increment = 5_000_000;
            limit.Value = 20_000_000;
            limit.FormatString = "N0";
            limit.Width = 190;

            StackPanel limitRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 12 };
            limitRow.Children.Add(new TextBlock { Text = "Count primes below", VerticalAlignment = VerticalAlignment.Center });
            limitRow.Children.Add(limit);
            panel.Children.Add(limitRow);

            syncButton.Content = "Sync - on the UI thread";
            asyncButton.Content = "Async - await Task.Run";
            asyncButton.Classes.Add("accent");
            syncButton.Padding = new Thickness(18, 10);
            asyncButton.Padding = new Thickness(18, 10);

            StackPanel buttons = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 12 };
            buttons.Children.Add(syncButton);
            buttons.Children.Add(asyncButton);
            panel.Children.Add(buttons);

            result.Foreground = Brushes.White;
            result.FontSize = 15;
            result.TextWrapping = TextWrapping.Wrap;
            panel.Children.Add(result);

            Content = panel;
        }
    }
}
