using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataDefinitions
{
    public static class ContextExtensions
    {
        public static int SetDataItemsState<TItem>(this DbContext context, IEnumerable<TItem> items, EntityState state) where TItem : class
        {
            if (items != null)
            {
                foreach (TItem item in items)
                    context.Entry<TItem>(item).State = state;

                return items.Count();
            }
            else
                return 0;
        }
        
    }
}
