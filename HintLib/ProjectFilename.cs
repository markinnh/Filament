using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HintLib
{
    public class ProjectFilename : HashedObject
    {
        public int ProjectFilenameId { get; set; }
        public override bool InDatabase => ProjectFilenameId != default;

        private string hintProjectFilename;
        [JsonIgnore, MaxLength(256)]
        public string HintProjectFilename
        {
            get => hintProjectFilename;
            set => Set<string>(ref hintProjectFilename, value);
        }
        public int HintProjectId { get; set; }
        [JsonIgnore]
        public virtual HintProject? HintProject { get; set; }

        public override void SetItemModifiedState(bool state)
        {
            IsModified = state;
        }
        public override void UpdateItem()
        {
            //using (Context.HintContext ctx = new Context.HintContext())
            //{
            //    var needsAdding = !InDatabase;
            //    if (IsModified)
            //    {
            //        if (InDatabase )
            //        {
            //            ctx.Entry(this).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            //            ctx.Update(this);
            //            ctx.SaveChanges();
            //            IsModified = false;
            //        }
            //        else 
            //        {
            //            ctx.Entry(this).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            //            ctx.Add(this);
            //            ctx.SaveChanges(true);
            //            IsModified = false;
            //        }
            //    }
            //}

        }
    }
}
