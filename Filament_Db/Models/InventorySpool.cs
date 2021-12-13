using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;
using Filament_Db.DataContext;

namespace Filament_Db.Models
{
    public class InventorySpool : DatabaseObject
    {
        public static event InDataOpsChangedHandler? InDataOpsChanged;

        private static bool inDataOps;
        public static bool InDataOps
        {
            get => inDataOps;
            set
            {
                inDataOps = value;

                // Notify all the InventorySpool objects of change to InDataOps state, allowing them to update the UI.
                InDataOpsChanged?.Invoke(EventArgs.Empty);
            }
        }
        public override bool InDataOperations => inDataOps;

        public override bool IsModified { get => base.IsModified || DepthMeasurements.Count(dm => dm.IsModified) > 0; set => base.IsModified = value; }
        public int InventorySpoolId { get; set; }

        public int FilamentDefnId { get; set; }

        private FilamentDefn? filamentDefn;

        public FilamentDefn? FilamentDefn
        {
            get => filamentDefn;
            set
            {
                if (Set(ref filamentDefn, value) && filamentDefn != null)
                    FilamentDefnId = filamentDefn.FilamentDefnId;
            }
        }

        public int SpoolDefnId { get; set; }
        private SpoolDefn? spoolDefn;

        public SpoolDefn? SpoolDefn
        {
            get => spoolDefn;
            set
            {
                if (Set<SpoolDefn?>(ref spoolDefn, value) && spoolDefn != null)
                    SpoolDefnId = spoolDefn.SpoolDefnID;
            }
        }

        private string? colorName;

        public string ColorName
        {
#pragma warning disable CS8603 // Possible null reference return.
            get => colorName;
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning disable CS8601 // Possible null reference assignment.
            set => Set<string>(ref colorName, value);
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        private DateTime dateOpened = DateTime.Today;

        public DateTime DateOpened
        {
            get => dateOpened;
            set => Set(ref dateOpened, value);
        }
        [NotMapped]
        public int AgeInDays => (DateTime.Today - DateOpened).Days;
        [NotMapped]
        public string Name => $"{ColorName} - {InventorySpoolId}";

        //private string? ignoreThis="Ignore This";
        //[NotMapped]
        //public string IgnoreThis
        //{
        //    get => ignoreThis="Ignore This";
        //    set => Set<string>(ref ignoreThis, value);
        //}

        public virtual ICollection<DepthMeasurement> DepthMeasurements { get; set; }
        [NotMapped]
        public IEnumerable<FilamentDefn>? Filaments => GetFilaments();

        protected IEnumerable<FilamentDefn>? GetFilaments()
        {
            if (Setting.GetSetting("SelectShowFlag") is Setting setting)
            {
                if (setting.Value == "ShowAll")
                    return FilamentContext.GetAllFilaments();
                else
                    return FilamentContext.GetFilaments(fi => !fi.StopUsing);
            }
            else
                return FilamentContext.GetAllFilaments();
        }
        public InventorySpool()
        {
            DepthMeasurements = new ObservableCollection<DepthMeasurement>();
            if (!InDataOpsChanged?.GetInvocationList().Contains(InventorySpool_InDataOpsChanged) ?? true)
                InDataOpsChanged += InventorySpool_InDataOpsChanged;
        }
        ~InventorySpool()
        {
            if (InDataOpsChanged?.GetInvocationList().Contains(InventorySpool_InDataOpsChanged) ?? false)
                InDataOpsChanged -= InventorySpool_InDataOpsChanged;
        }
        private void InventorySpool_InDataOpsChanged(EventArgs args)
        {
            OnPropertyChanged(nameof(CanEdit));
            //throw new NotImplementedException();
        }
        public override void UpdateItem()
        {
            throw new NotImplementedException();
        }
    }
}
