// See https://aka.ms/new-console-template for more information
bool NeedMigration = false;
DataDefinitions.DataSeed.Seed<SqliteContext.FilamentContext>(ref NeedMigration);
