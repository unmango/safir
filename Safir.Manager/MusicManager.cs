// <copyright file="MusicManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Manager
{
    using System.IO;
    using Safir.Data.Entities;
    using Safir.Data.Entities.Repositories;

    public class MusicManager
    {
        private readonly SongRepository _songs;

        public MusicManager(
            SongRepository songs) {
            _songs = songs;
        }

        public void AddFile(string path) {
            if (!File.Exists(path)) return;
            var file = TagLib.File.Create(path);
            //_songs.Insert(file);
        }

        public void AddFolder(string path) {
            if (!Directory.Exists(path)) return;
            foreach (var file in Directory.EnumerateFiles(path)) {
                AddFile(file);
            }

            foreach (var dir in Directory.EnumerateDirectories(path)) {
                AddFolder(dir);
            }
        }
    }
}
