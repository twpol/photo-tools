using System;
using System.IO;

namespace Photo_Reviewer
{
    public class Photo(string path) : IComparable<Photo>
    {
        public readonly string Path = path;

        public readonly DateTime DateTime = File.GetLastWriteTimeUtc(path);

        public int CompareTo(Photo? other)
        {
            if (other == null) return 1;
            return DateTime == other.DateTime ? 0 : DateTime > other.DateTime ? 1 : -1;
        }
    }
}
