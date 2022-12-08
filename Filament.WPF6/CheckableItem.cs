using DataDefinitions;
using DataDefinitions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filament.WPF6
{
	/// <summary>
	/// Checkable Item, for UI consumption
	/// </summary>
	/// <typeparam name="T">Observable type</typeparam>
	/// <remarks>
	/// requires an Observable type to prevent memory leaks
	/// </remarks>
    public class CheckableItem<T>:Observable where T : Observable
    {
		private bool isChecked;

		public bool IsChecked
		{
			get => isChecked;
			set => Set<bool>(ref isChecked, value);
		}

		private T? item;

		public T Item
		{
			get => item;
			set => Set<T>(ref item, value);
		}
		public CheckableItem(bool isChecked,T item)
		{
			IsChecked = isChecked;
			Item = item;
		}
	}
	public class CheckableTagStat : CheckableItem<TagStat>
	{
		public CheckableTagStat(bool isChecked,TagStat tagStat) : base(isChecked, tagStat) { }
	}
}
