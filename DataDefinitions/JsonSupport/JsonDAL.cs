using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using DataDefinitions.Interfaces;

namespace DataDefinitions.JsonSupport
{
    public class JsonDAL
    {
        public IJsonFilamentDocument Document { get; set; }
        public string Filename { get; protected set; }
        public void InitializeFromFile(string fileName)
        {
            Filename = fileName;
            if (File.Exists(fileName))
                Document = JsonFilamentDocument.LoadFile(fileName);
            else
            {
                Document = new JsonFilamentDocument();
                Document.SeedDocument();
            }
        }
        public void SaveDocument()
        {
            if (!string.IsNullOrEmpty(Filename))
                Document.SaveFile(Filename);
            else
                throw new Exception("Unable to save the file, the filename is not initialized.");
        }
        public void InitializeFromContent(string content)
        {
            Document = JsonSerializer.Deserialize<JsonFilamentDocument>(content);
        }
        public string JsonContent()=>JsonSerializer.Serialize(Document);
    }
}
