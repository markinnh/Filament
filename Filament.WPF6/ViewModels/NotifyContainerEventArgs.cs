using System;

namespace Filament.WPF6.ViewModels
{
    public class NotifyContainerEventArgs:EventArgs
    {
        public string[] Elements { get; set; }
        public NotifyContainerEventArgs(string[] elements) => Elements = elements;
    }
}
