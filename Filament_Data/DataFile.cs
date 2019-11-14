﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Filament_Data
{
    public class DataFile
    {
        /// <summary>
        /// The data filename, current version is 0.9
        /// </summary>
        const string dataFilename = "FilamentDataV0_9.json";
        public string Filename => System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), dataFilename);
        public DataDocument Document { get; set; }
        public DataFile()
        {
#if DEBUG
            // while debugging, expects to return either a DataDocument from file or a prepopulated DataDocument
            Document = DataDocument.GetDocument(Filename);
#else
            Document = DataDocument.GetDataDocument(Filename);
#endif
        }
        public void Save()
        {
            JsonTransfer.SerializeObject(Filename, Document);
        }
    }
}
