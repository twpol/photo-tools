using System;
using System.IO;

namespace Photo_Reviewer
{
    public class Photo(string path) : IComparable<Photo>
    {
        public static string GetRatingFolderName(PhotoRating rating) => rating switch
        {
            PhotoRating.Unrated => ".unrated",
            PhotoRating.Rated0 => ".rated-0",
            PhotoRating.Rated1 => ".rated-1",
            PhotoRating.Rated2 => ".rated-2",
            _ => throw new ArgumentOutOfRangeException(nameof(rating)),
        };

        public readonly string Path = path;

        public readonly DateTime DateTime = File.GetLastWriteTimeUtc(path);

        public int CompareTo(Photo? other)
        {
            if (other == null) return 1;
            return DateTime == other.DateTime ? 0 : DateTime > other.DateTime ? 1 : -1;
        }
    }
}
