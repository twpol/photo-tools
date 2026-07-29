using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Photo_Reviewer
{
    public class PhotoFolder(string path) : IReadOnlyList<Photo>
    {
        #region IReadOnlyList<Photo> members
        public Photo this[int index] => Photos[index];
        public int Count => Photos.Count;
        public IEnumerator<Photo> GetEnumerator() => Photos.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        readonly string[] Extensions = [
            ".bmp",
            ".gif",
            ".jpeg",
            ".jpg",
            ".png",
            ".tiff",
        ];

        readonly List<Photo> Photos = [];

        public readonly string Path = path;

        public bool IsInitialised { get; private set; } = Directory.Exists(System.IO.Path.Combine(path, Photo.GetRatingFolderName(PhotoRating.Rated0)));

        public bool IsLoaded { get; private set; }

        public int CurrentIndex
        {
            get;
            set
            {
                ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(CurrentIndex));
                ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value, Photos.Count, nameof(CurrentIndex));
                field = value;
            }
        }

        public bool CurrentValid => CurrentIndex >= 0 && CurrentIndex < Photos.Count;

        public Photo Current => Photos[CurrentIndex];

        public async Task Initialise(PhotoRating baseRating)
        {
            if (IsInitialised) throw new InvalidOperationException("Can only initialise folders once");
            if (baseRating != PhotoRating.Unrated) throw new ArgumentException("Only 'Unrated' allowed", nameof(baseRating));
            foreach (var rating in Enum.GetValues<PhotoRating>())
            {
                if (rating != baseRating)
                {
                    var path = System.IO.Path.Combine(Path, Photo.GetRatingFolderName(rating));
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                }
            }
            IsInitialised = true;
        }

        public void Load()
        {
            if (IsLoaded) throw new InvalidOperationException("Can only load folders once");
            foreach (var file in Directory.GetFiles(Path, "*", SearchOption.AllDirectories))
            {
                if (Extensions.Contains(System.IO.Path.GetExtension(file), StringComparer.OrdinalIgnoreCase)) Photos.Add(new(file));
            }
            Photos.Sort();
            IsLoaded = true;
        }
    }
}
