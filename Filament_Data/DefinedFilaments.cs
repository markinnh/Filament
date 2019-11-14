using System;
using System.Collections.Generic;
using System.Text;

namespace Filament_Data
{
    public class DefinedFilaments : DictionaryContainer<int, FilamentDefn>
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

            var filament1 = FilamentDefn.CreateAndAddFilamentDefn(FilamentDefn.StandardFilamentDiameter, DefinedDensity.BasicPLA, MaterialType.PLA, document);
            Add(filament1.FilamentID, filament1);

            var filament2 = FilamentDefn.CreateAndAddFilamentDefn(FilamentDefn.StandardFilamentDiameter, DefinedDensity.BasicABS, MaterialType.ABS, document);
            Add(filament2.FilamentID, filament2);
        }
    }
}
