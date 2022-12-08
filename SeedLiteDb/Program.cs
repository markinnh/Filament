// See https://aka.ms/new-console-template for more information
var vendors = DataDefinitions.Seed.InitialVendorDefinitions();
var db = new DataDefinitions.LiteDBSupport.LiteDBDal();
db.SetFilename(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Filament", "FilamentDataLiteDb.db"));
db.SeedDatabase();

