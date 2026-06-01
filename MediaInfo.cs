using System;
using System.Collections.Generic;
using System.Text;
using Windows.Media.Control;

namespace MiniSpotify
{
    public record MediaInfo(
        string Title,
        string Artist,
        byte[] ThumbnailBytes,
        string SourceAppUserModelId,
        GlobalSystemMediaTransportControlsSessionPlaybackStatus PlaybackStatus,
        GlobalSystemMediaTransportControlsSession? Session);
}
