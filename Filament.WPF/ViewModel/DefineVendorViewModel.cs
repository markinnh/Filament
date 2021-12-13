using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Filament_Data.JsonModel;
namespace Filament.WPF.ViewModel
{
    [ViewModelDescriptor(Category ="Definitions",Description ="Define a vendor",Title ="Vendor")]
    public class DefineVendorViewModel:BaseViewModel<VendorDefn>
    {

        public DefineVendorViewModel() : base() { }
        private ICommand followLinkCommand;
        public ICommand FollowLinkCommand
        {
            get
            {
                if (followLinkCommand == null)
                    followLinkCommand = new Helpers.RelayCommand<object>(FollowLinkAction);
                return followLinkCommand;
            }
            set
            {
                followLinkCommand = value;
            }
        }

        private void FollowLinkAction(object obj)
        {
            if (obj is Uri uri)
            {
                Process.Start(new ProcessStartInfo(uri.AbsoluteUri));
            }
        }


        protected override void SaveNew()
        {
            if (InAddNew)
            {
                DataFile.Document.Vendors.Add(EditItem);

                InAddNew = false;
            }
        }
    }
}
