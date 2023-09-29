using DataDefinitions.Interfaces;
using LiteDB;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions.Models
{
    public class NoteDefn : TaggedDatabaseObject, IDate
    {
        public static event InDataOpsChangedHandler InDataOpsChanged;

        private static bool inDataOps;
        public static bool InDataOps
        {
            get => inDataOps;
            set
            {
                inDataOps = value;

                // Notify all the FilamentDefn objects of change to InDataOps state, allowing them to update the UI.
                InDataOpsChanged?.Invoke(EventArgs.Empty);
            }
        }
        public override bool InDataOperations => inDataOps;
        public override bool IsValid => _enteredDateTime != default && !string.IsNullOrEmpty(Note);
        [BsonId]
        public int NoteDefnId { get; set; }
        internal override int KeyID { get => NoteDefnId; set => NoteDefnId = value; }
        private string _note;

        public string Note
        {
            get => _note;
            set => Set<string>(ref _note, value);
        }

        private DateTime _enteredDateTime = DateTime.Today;

        public DateTime EnteredDateTime
        {
            get => _enteredDateTime;
            set => Set<DateTime>(ref _enteredDateTime, value);
        }
        public DateTime DateTime { get => _enteredDateTime; }
    }
}
