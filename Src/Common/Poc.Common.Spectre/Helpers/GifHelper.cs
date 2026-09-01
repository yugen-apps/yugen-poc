using SixLabors.ImageSharp;
using Spectre.Console;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Common.Spectre.Helpers;

public static class GifHelper
{
    public static async Task Play(string path, LiveDisplayContext ctx, CancellationToken cancellationToken)
    {
        using var gif = await Image.LoadAsync(path, cancellationToken);
        var metadata = gif.Frames.RootFrame.Metadata.GetGifMetadata();

        while (!cancellationToken.IsCancellationRequested)
        {
            for (var i = 0; i < gif.Frames.Count; i++)
            {
                var delay = gif.Frames[i].Metadata.GetGifMetadata().FrameDelay;
                using var clone = gif.Frames.CloneFrame(i);

                await using var memoryStream = new MemoryStream();
                await clone.SaveAsBmpAsync(memoryStream, cancellationToken: cancellationToken);
                memoryStream.Position = 0;

                var canvasImage = new CanvasImage(memoryStream).MaxWidth(50);
                ctx.UpdateTarget(canvasImage);

                // FrameDelay is measured in 1/100th second.
                // Let's half the delay since we're encoding per frame.
                await Task.Delay(TimeSpan.FromMilliseconds(delay * 5), cancellationToken)
                          .ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
            }
        }
    }
}