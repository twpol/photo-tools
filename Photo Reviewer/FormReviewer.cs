using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Environment;

namespace Photo_Reviewer
{
    public partial class FormReviewer : Form
    {
        const float LowQualityMaxSize = 512;

        const int PreloadCount = 10;
        const int KeepHighQualityLoadedCount = 2;

        public FormReviewer()
        {
            InitializeComponent();

            // Start browsing from user's Pictures folder, as that's where their albums are likely to be.
            folderBrowserDialog.SelectedPath = Environment.GetFolderPath(SpecialFolder.MyPictures);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                // Add WS_EX_COMPOSITED to avoid flickering when panel repaints.
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        string AlbumFilePath;
        List<Photo> Photos = new List<Photo>();
        int SelectedIndex = -1;
        Image SelectedPhoto;

        [DefaultProperty("FileName")]
        class Photo
        {
            [Category("Camera")]
            public string Make { get; private set; }
            [Category("Camera")]
            public string Model { get; private set; }
            [Category("Camera")]
            public string Software { get; private set; }

            [Category("File")]
            public string Name { get; private set; }
            [Category("File")]
            public string File { get; private set; }
            [Category("File")]
            public long Size { get; private set; }
            [Category("File")]
            public DateTime Created { get; private set; }
            [Category("File")]
            public DateTime Modified { get; private set; }

            [Category("Photo")]
            public Size Dimensions { get; private set; }
            [Category("Photo")]
            public PhotoOrientation Orientation { get; private set; }
            [Category("Photo")]
            public DateTime Taken { get; private set; }
            [Category("Photo")]
            public DateTime Digitized { get; private set; }

            [Category("State")]
            public bool Flagged { get; private set; }
            [Category("State")]
            public bool Deleted { get; private set; }

            public Photo(string fileName, bool flagged, bool deleted)
            {
                var info = new FileInfo(fileName);
                Name = Path.GetFileNameWithoutExtension(fileName);
                File = fileName;
                Size = info.Length;
                Created = info.CreationTimeUtc;
                Modified = info.LastWriteTimeUtc;

                Flagged = flagged;
                Deleted = deleted;
            }

            Image Image;
            Image ImageHighQuality;

            public async Task<Image> Load(bool highQuality = false)
            {
                if (Image == null)
                    Image = await LoadPhoto();

                if (ImageHighQuality == null && highQuality)
                    ImageHighQuality = await LoadPhoto(true);

                return ImageHighQuality != null ? ImageHighQuality : Image;
            }

            public void Unload(bool highQuality = false)
            {
                if (ImageHighQuality != null)
                    ImageHighQuality.Dispose();

                ImageHighQuality = null;

                if (highQuality)
                    return;

                if (Image != null)
                    Image.Dispose();

                Image = null;
            }

            async Task<Image> LoadPhoto(bool highQuality = false)
            {
                using (var photo = await Task.Run(() => Image.FromFile(File)))
                {
                    if (!highQuality)
                    {
                        Dimensions = photo.Size;
                        try
                        {
                            var orientation = photo.GetPropertyItem(0x112/* PropertyTagOrientation */);
                            Orientation = (PhotoOrientation)orientation.Value[0];
                        }
                        catch (ArgumentException) { }
                        Make = GetStringProperty(photo, 0x10F/* PropertyTagEquipMake */);
                        Model = GetStringProperty(photo, 0x110/* PropertyTagEquipModel */);
                        Software = GetStringProperty(photo, 0x131/* PropertyTagSoftwareUsed */);
                        Taken = GetDateTimeProperty(photo, 0x9003/* PropertyTagExifDTOrig */);
                        Digitized = GetDateTimeProperty(photo, 0x9004/* PropertyTagExifDTDigitized */);
                    }
                    return ClonePhoto(photo, highQuality);
                }
            }

            Image ClonePhoto(Image original, bool highQuality = false)
            {
                var scale = Math.Min(
                    LowQualityMaxSize / original.Width,
                    LowQualityMaxSize / original.Height
                );
                return highQuality ? new Bitmap(original) : new Bitmap(original, (int)(original.Width * scale), (int)(original.Height * scale));
            }

            public void ToggleFlagged()
            {
                if (!Flagged && Deleted)
                    ToggleDeleted();

                var oldName = File;
                var newName = Path.GetDirectoryName(File);
                if (Flagged)
                    newName = Path.GetDirectoryName(newName);
                else
                    newName = Path.Combine(newName, "Flagged");
                if (!Flagged && !Directory.Exists(newName))
                    Directory.CreateDirectory(newName);
                newName = Path.Combine(newName, Path.GetFileName(File));
                System.IO.File.Move(oldName, newName);
                File = newName;
                Flagged = !Flagged;
            }

            public void ToggleDeleted()
            {
                if (!Deleted && Flagged)
                    ToggleFlagged();

                var oldName = File;
                var newName = Path.GetDirectoryName(File);
                if (Deleted)
                    newName = Path.GetDirectoryName(newName);
                else
                    newName = Path.Combine(newName, "Deleted");
                if (!Deleted && !Directory.Exists(newName))
                    Directory.CreateDirectory(newName);
                newName = Path.Combine(newName, Path.GetFileName(File));
                System.IO.File.Move(oldName, newName);
                File = newName;
                Deleted = !Deleted;
            }
        }

        enum PhotoOrientation
        {
            Unknown,
            Upright,
            UprightFlipped,
            Rotated180,
            Rotated180Flipped,
            Rotated270Flipped,
            Rotated270,
            Rotated90Flipped,
            Rotated90,
        }

        async Task SetAlbum(string albumFilePath)
        {
            await Task.Yield();
            AlbumFilePath = "Loading...";
            await UpdateDisplay();
            AlbumFilePath = albumFilePath;
            Photos = LoadPhotos(AlbumFilePath);
            SelectedIndex = Photos.Count > 0 ? 0 : -1;
            await UpdateDisplay();
            panelPhoto.Focus();
        }

        static List<Photo> LoadPhotos(string filePath)
        {
            var photos = new ConcurrentBag<Photo>();
            Parallel.ForEach(Directory.GetFiles(filePath, "*", SearchOption.AllDirectories), (file) =>
            {
                photos.Add(LoadPhoto(filePath, file));
            });
            return new List<Photo>(photos.Where(p => p != null).OrderBy(p => p.Name));
        }

        static Photo LoadPhoto(string albumFilePath, string filePath)
        {
            if (!filePath.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) && !filePath.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) && !filePath.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                return null;

            var container = Path.GetFileName(Path.GetDirectoryName(filePath));
            var flagged = "Flagged".Equals(container, StringComparison.OrdinalIgnoreCase);
            var deleted = "Deleted".Equals(container, StringComparison.OrdinalIgnoreCase);
            var album = flagged || deleted ? Path.GetDirectoryName(Path.GetDirectoryName(filePath)) : Path.GetDirectoryName(filePath);
            if (!album.Equals(albumFilePath, StringComparison.OrdinalIgnoreCase))
                return null;

            return new Photo(filePath, flagged, deleted);
        }

        async Task UpdateDisplay()
        {
            labelFolder.Text = AlbumFilePath;
            var photoCount = Photos.Count;
            var photoSelected = SelectedIndex;
            var photo = SelectedIndex == -1 ? null : Photos[SelectedIndex];

            labelFlagged.ForeColor = SelectedIndex != -1 && photo.Flagged ? SystemColors.HighlightText : SystemColors.GrayText;
            labelFlagged.BackColor = SelectedIndex != -1 && photo.Flagged ? SystemColors.Highlight : SystemColors.Control;
            labelDeleted.ForeColor = SelectedIndex != -1 && photo.Deleted ? SystemColors.HighlightText : SystemColors.GrayText;
            labelDeleted.BackColor = SelectedIndex != -1 && photo.Deleted ? SystemColors.Highlight : SystemColors.Control;

            if (progressBarPosition.Maximum != photoCount || progressBarPosition.Value != SelectedIndex)
            {
                if (progressBarPosition.Maximum != photoCount)
                {
                    progressBarPosition.Value = 0;
                    progressBarPosition.Maximum = photoCount;
                }
                if (SelectedIndex == -1)
                {
                    progressBarPosition.Value = 0;
                    SelectedPhoto = null;
                }
                else
                {
                    progressBarPosition.Value = SelectedIndex;
                    SelectedPhoto = await photo.Load();
                }

                panelPhoto.Refresh();
                propertyGridPhoto.SelectedObject = photo;

                if (SelectedIndex != -1)
                {
                    var highQualityPhoto = await Task.Run(async () =>
                    {
                        for (var i = 0; i < Photos.Count; i++)
                        {
                            if (Math.Abs(SelectedIndex - i) <= PreloadCount)
                            {
                                await Photos[i].Load();
                                if (Math.Abs(SelectedIndex - i) > KeepHighQualityLoadedCount)
                                    Photos[i].Unload(true);
                            }
                            else
                            {
                                Photos[i].Unload();
                            }
                        }

                        await Task.Delay(1000);

                        if (photoSelected == SelectedIndex)
                            return await photo.Load(true);

                        return null;
                    });

                    if (photoSelected == SelectedIndex && highQualityPhoto != null)
                    {
                        SelectedPhoto = highQualityPhoto;
                        panelPhoto.Refresh();
                    }
                }
            }
        }

        static DateTime GetDateTimeProperty(Image image, int propId)
        {
            var text = GetStringProperty(image, propId);
            if (string.IsNullOrEmpty(text))
                return DateTime.MinValue;
            try
            {
                return DateTime.ParseExact(text, "yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            }
            catch (FormatException) { }
            return DateTime.MaxValue;
        }

        static string GetStringProperty(Image image, int propId)
        {
            try
            {
                return Encoding.ASCII.GetString(image.GetPropertyItem(propId).Value).Replace("\0", "");
            }
            catch (ArgumentException) { }
            return "";
        }

        void panelPhoto_Paint(object sender, PaintEventArgs e)
        {
            using (var g = e.Graphics)
            {
                if (SelectedPhoto == null)
                {
                    g.FillRectangle(SystemBrushes.Window, panelPhoto.ClientRectangle);
                }
                else
                {
                    g.TranslateTransform(panelPhoto.ClientRectangle.Width / 2, panelPhoto.ClientRectangle.Height / 2);
                    var scale = Math.Min(
                        (float)panelPhoto.ClientRectangle.Width / SelectedPhoto.Width,
                        (float)panelPhoto.ClientRectangle.Height / SelectedPhoto.Height
                    );
                    g.ScaleTransform(scale, scale);
                    if (Photos[SelectedIndex].Orientation == PhotoOrientation.Rotated90)
                        g.RotateTransform(270);
                    else if (Photos[SelectedIndex].Orientation == PhotoOrientation.Rotated270)
                        g.RotateTransform(90);
                    // TODO: Implement the four "flipped" orientations.
                    g.TranslateTransform(-SelectedPhoto.Width / 2, -SelectedPhoto.Height / 2);
                    g.DrawImageUnscaled(SelectedPhoto, 0, 0);
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // When the photo is focused, force all key events to go to it even if they'd normally do focus navigation.
            if (panelPhoto.Focused)
            {
                var e = new KeyEventArgs(keyData);
                OnKeyDown(e);
                if (e.Handled)
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        async void FormReviewer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.O && e.Modifiers == Keys.None)
            {
                e.Handled = true;
                buttonBrowse_Click(sender, EventArgs.Empty);
            }
            else if (e.KeyCode == Keys.Right && e.Modifiers == Keys.None)
            {
                e.Handled = true;
                if (SelectedIndex < Photos.Count - 1)
                {
                    SelectedIndex++;
                    await UpdateDisplay();
                }
            }
            else if (e.KeyCode == Keys.Right && e.Modifiers == Keys.Shift)
            {
                e.Handled = true;
                var index = Photos.FindIndex(SelectedIndex + 1, p => p.Flagged);
                if (index != -1)
                {
                    SelectedIndex = index;
                    await UpdateDisplay();
                }
            }
            else if (e.KeyCode == Keys.Right && e.Modifiers == Keys.Control)
            {
                e.Handled = true;
                var index = Photos.FindIndex(SelectedIndex + 1, p => p.Deleted);
                if (index != -1)
                {
                    SelectedIndex = index;
                    await UpdateDisplay();
                }
            }
            else if (e.KeyCode == Keys.Left && e.Modifiers == Keys.None)
            {
                e.Handled = true;
                if (SelectedIndex > 0)
                {
                    SelectedIndex--;
                    await UpdateDisplay();
                }
            }
            else if (e.KeyCode == Keys.Left && e.Modifiers == Keys.Shift)
            {
                e.Handled = true;
                var index = Photos.FindLastIndex(SelectedIndex - 1, p => p.Flagged);
                if (index != -1)
                {
                    SelectedIndex = index;
                    await UpdateDisplay();
                }
            }
            else if (e.KeyCode == Keys.Left && e.Modifiers == Keys.Control)
            {
                e.Handled = true;
                var index = Photos.FindLastIndex(SelectedIndex - 1, p => p.Deleted);
                if (index != -1)
                {
                    SelectedIndex = index;
                    await UpdateDisplay();
                }
            }
            else if (e.KeyCode == Keys.J && e.Modifiers == Keys.None)
            {
                e.Handled = true;
                textBoxJump.Left = (ClientRectangle.Width - textBoxJump.Width) / 2;
                textBoxJump.Top = (ClientRectangle.Height - textBoxJump.Height) / 2;
                textBoxJump.Text = "";
                textBoxJump.Visible = true;
                textBoxJump.Focus();
            }
            else if (e.KeyCode == Keys.Insert && e.Modifiers == Keys.None)
            {
                e.Handled = true;
                Photos[SelectedIndex].ToggleFlagged();
                await UpdateDisplay();
            }
            else if (e.KeyCode == Keys.Delete && e.Modifiers == Keys.None)
            {
                e.Handled = true;
                Photos[SelectedIndex].ToggleDeleted();
                await UpdateDisplay();
            }
        }

        async void textBoxJump_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.None)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                panelPhoto.Focus();
                textBoxJump.Visible = false;
                var text = textBoxJump.Text.Trim();
                var percent = text.EndsWith("%", StringComparison.Ordinal);
                if (percent)
                    text = text.Substring(0, text.Length - 1);
                int index;
                if (int.TryParse(text, out index))
                {
                    if (percent)
                        index = index * Photos.Count / 100;
                    if (index < 0 || index >= Photos.Count)
                    {
                        Console.Beep();
                    }
                    else
                    {
                        SelectedIndex = index;
                        await UpdateDisplay();
                    }
                }
            }
        }

        async void buttonBrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                await SetAlbum(folderBrowserDialog.SelectedPath);
        }

        async void labelState_Click(object sender, EventArgs e)
        {
            if (sender == labelFlagged)
                Photos[SelectedIndex].ToggleFlagged();
            else if (sender == labelDeleted)
                Photos[SelectedIndex].ToggleDeleted();
            await UpdateDisplay();
        }

        void panelPhoto_Click(object sender, EventArgs e)
        {
            panelPhoto.Focus();
        }
    }
}
