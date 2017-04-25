using MimeSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace Safir.Manager
{
    public class Song
    {
        private TagLib.File song;
        private TagLib.Id3v2.PopularimeterFrame popularimeter;

        #region Constructors

        public Song(Uri filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException("filePath");

            if (!filePath.IsFile)
                throw new ArgumentException("Not a file");

            if (!FileExtension.Valid(filePath))
                throw new ArgumentException("Filetype not supported");

            song = TagLib.File.Create(filePath.AbsolutePath);

            popularimeter = FileExtension.PopularimeterFrame<TagLib.Id3v2.PopularimeterFrame>(song);
        }

        #endregion

        public string Title
        {
            get
            {
                return song.Tag.Title;
            }

            set
            {
                song.Tag.Title = value;
            }
        }

        public string[] Artists
        {
            get
            {
                return song.Tag.Performers;
            }

            set
            {
                song.Tag.Performers = value;
            }
        }

        public string Album
        {
            get
            {
                return song.Tag.Album;
            }

            set
            {
                song.Tag.Album = value;
            }
        }

        public string[] AlbumArtists
        {
            get
            {
                return song.Tag.AlbumArtists;
            }

            set
            {
                song.Tag.AlbumArtists = value;
            }
        }

        public string[] Genres
        {
            get
            {
                return song.Tag.Genres;
            }

            set
            {
                song.Tag.Genres = value;
            }
        }

        public string[] Composers
        {
            get
            {
                return song.Tag.Composers;
            }

            set
            {
                song.Tag.Composers = value;
            }
        }

        public TimeSpan Duration
        {
            get
            {
                return song.Properties.Duration;
            }
        }

        public uint Year
        {
            get
            {
                return song.Tag.Year;
            }

            set
            {
                song.Tag.Year = value;
            }
        }

        public DateTime ReleaseDate
        {
            get
            {
                var dateString = $"{Year}";
                return DateTime.Parse(dateString);
            }

            set
            {
                song.Tag.Year = Convert.ToUInt32(value.Year);
            }
        }

        public uint Track
        {
            get
            {
                return song.Tag.Track;
            }

            set
            {
                song.Tag.Track = value;
            }
        }

        public uint DiskNo
        {
            get
            {
                return song.Tag.Disc;
            }

            set
            {
                song.Tag.Disc = value;
            }
        }

        public bool Compilation
        {
            get
            {
                return song.Tag.Performers.Length > 1;
            }
        }

        public string Lyrics
        {
            get
            {
                return song.Tag.Lyrics;
            }

            set
            {
                song.Tag.Lyrics = value;
            }
        }

        public int Rating
        {
            get
            {
                return popularimeter.Rating;
            }

            set
            {
                var bytes = BitConverter.GetBytes(value);
                if (bytes.Length > 1)
                    throw new ArgumentException("Value is too large");
                popularimeter.Rating = bytes[0];
            }
        }

        public ulong PlayCount
        {
            get
            {
                return popularimeter.PlayCount;
            }
        }

        public string Comments
        {
            get
            {
                return song.Tag.Comment;
            }

            set
            {
                song.Tag.Comment = value;
            }
        }

    }
}
