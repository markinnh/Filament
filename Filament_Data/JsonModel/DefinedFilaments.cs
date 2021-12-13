using System;
using System.Collections.Generic;
using System.Text;

namespace Filament_Data.JsonModel
{
    public class DefinedFilaments : DocumentLinkedCollection<FilamentDefn>
    {
        //public Dictionary<string,FilamentDefn> Definitions { get; private set; }
        public DefinedFilaments(bool prepopulate,IDocument document)
        {
            EstablishLink(document);
            if (prepopulate)
            {
                Initialize(document);
            }
        }
        public DefinedFilaments()
        {

        }
        public void Initialize(IDocument document = null)
        {
            if (document == null)
                document = Document;

            var filament1 = FilamentDefn.CreateAndAddFilamentDefn(FilamentDefn.StandardFilamentDiameter, Constants.BasicPLADensity, MaterialType.PLA, document);
            Add( filament1);

            var filament2 = FilamentDefn.CreateAndAddFilamentDefn(FilamentDefn.StandardFilamentDiameter, Constants.BasicABSDensity, MaterialType.ABS, document);
            Add(filament2);

            Add(FilamentDefn.CreateAndAddFilamentDefn(document,Constants.BasicNylonDensity,MaterialType.Nylon));
            Add(FilamentDefn.CreateAndAddFilamentDefn(document,Constants.BasicPETGDensity,MaterialType.PETG));
            Add(FilamentDefn.CreateAndAddFilamentDefn(document,Constants.BasicPolycarbonateDensity,MaterialType.PC));
        }
    }
}
