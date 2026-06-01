using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace MiniSpotify.Helper
{
    public static class AlbumArtHelper
    {
        /// <summary>Albüm kapak byte'larının başlık+sanatçı anahtarıyla saklandığı önbellek.</summary>
        private static readonly ConcurrentDictionary<string, byte[]> _byteCache = new();

        /// <summary>LRU sırasını takip eden anahtar listesi.</summary>
        private static readonly List<string> _cacheKeys = new();

        /// <summary>Önbelleğe thread-safe erişim için kilit.</summary>
        private static readonly object _cacheLock = new();

        /// <summary>Lazy yüklenen varsayılan kapak resmi.</summary>
        private static Image? _defaultArt;
        /// <summary>
        /// Varsayılan albüm kapağı. İlk erişimde oluşturulur, sonra önbellekten döner.
        /// </summary>
        public static Image DefaultArt
        {
            get
            {
                if (_defaultArt != null) return _defaultArt;
                _defaultArt = GenerateDefaultArt();
                return _defaultArt;
            }
        }
        /// <summary>
        /// WinRT thumbnail stream'ini byte dizisine çevirir.
        /// Önce önbelleğe bakar, yoksa stream'i okuyup önbelleğe ekler.
        /// </summary>
        /// <param name="title">Şarkı başlığı (önbellek anahtarı için).</param>
        /// <param name="artist">Sanatçı adı (önbellek anahtarı için).</param>
        /// <param name="thumbnail">WinRT thumbnail stream referansı.</param>
        /// <param name="ct">İptal token'ı.</param>
        /// <returns>Byte dizisi olarak kapak resmi, yoksa null.</returns>
        public static async Task<byte[]?> GetThumbnailBytesAsync(string? title, string? artist,
            IRandomAccessStreamReference? thumbnail, CancellationToken ct = default)
        {
            if (thumbnail == null) return null;

            var cacheKey = $"{title}|{artist}";
            if (_byteCache.TryGetValue(cacheKey, out var cachedBytes))
                return cachedBytes;

            try
            {
                using var streamRef = await thumbnail.OpenReadAsync().AsTask(ct);
                using var winRtStream = streamRef.AsStreamForRead();
                using var ms = new MemoryStream();
                await winRtStream.CopyToAsync(ms, ct);
                var bytes = ms.ToArray();

                AddToCache(cacheKey, bytes);
                return bytes;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Byte dizisini LRU önbelleğe ekler. Kapasite aşılırsa en eski kaydı siler.
        /// </summary>
        private static void AddToCache(string key, byte[] bytes)
        {
            lock (_cacheLock)
            {
                if (_byteCache.ContainsKey(key)) return;
                while (_cacheKeys.Count >= 10)
                {
                    var oldest = _cacheKeys[0];
                    _cacheKeys.RemoveAt(0);
                    _byteCache.TryRemove(oldest, out _);
                }
                _byteCache[key] = bytes;
                _cacheKeys.Add(key);
            }
        }

        /// <summary>
        /// Kapak resmi bulunamadığında gösterilecek varsayılan ♫ resmini oluşturur.
        /// 128x128 koyu gri arka plan üzerinde büyük nota sembolü.
        /// </summary>
        private static Image GenerateDefaultArt()
        {
            var bmp = new Bitmap(128, 128);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.FromArgb(40, 40, 44));
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var font = new Font("Segoe UI Symbol", 64, FontStyle.Regular);
            using var brush = new SolidBrush(Color.FromArgb(120, 120, 124));
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString("\u266B", font, brush, new RectangleF(0, 0, 128, 128), sf);
            return bmp;
        }

    }
}
