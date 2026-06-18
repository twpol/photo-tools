using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

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

        readonly List<Photo> Photos = [];

        public readonly string Path = path;

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

        public Photo Current => Photos[CurrentIndex];

        public void Load()
        {
            foreach (var file in Directory.GetFiles(Path, "*", SearchOption.AllDirectories))
            {
                Photos.Add(new(file));
            }
        }
    }
}
