using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using SkiaSharp;

namespace CatDemo
{
    // Plays an animated GIF in an Image control.
    //
    // The frames are switched by a DispatcherTimer, and a DispatcherTimer
    // ticks ON THE UI THREAD. So when the UI thread is busy, the cat stops.
    class GifPlayer
    {
        readonly Image target;
        readonly List<Bitmap> frames = new List<Bitmap>();
        readonly DispatcherTimer timer = new DispatcherTimer();
        int current = 0;

        public GifPlayer(Image target, string path)
        {
            this.target = target;

            int delayMs = LoadFrames(path);
            timer.Interval = TimeSpan.FromMilliseconds(delayMs);
            timer.Tick += OnTick;
        }

        public void Start()
        {
            target.Source = frames[0];
            timer.Start();
        }

        void OnTick(object sender, EventArgs e)
        {
            current = (current + 1) % frames.Count;
            target.Source = frames[current];
        }

        // Decodes every frame of the GIF with SkiaSharp, the graphics library
        // Avalonia itself draws with. Returns the delay between frames.
        int LoadFrames(string path)
        {
            using SKCodec codec = SKCodec.Create(path);
            SKImageInfo info = new SKImageInfo(codec.Info.Width, codec.Info.Height);

            for (int i = 0; i < codec.FrameCount; i++)
            {
                using SKBitmap bitmap = new SKBitmap(info);
                codec.GetPixels(info, bitmap.GetPixels(), new SKCodecOptions(i));

                using SKImage image = SKImage.FromBitmap(bitmap);
                using SKData png = image.Encode(SKEncodedImageFormat.Png, 100);
                using Stream stream = png.AsStream();
                frames.Add(new Bitmap(stream));
            }

            int delay = codec.FrameInfo[0].Duration;
            if (delay <= 0)
                delay = 50;
            return delay;
        }
    }
}
