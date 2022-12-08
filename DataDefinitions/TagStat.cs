namespace DataDefinitions
{
    public class TagStat : Observable
    {
        private string tag;
        public string Tag { get => tag; set => Set(ref tag, value); }
        private int count;
        public int Count { get => count; set => Set(ref count, value); }

        public TagStat(string tag, int count)
        {
            Tag = tag;
            Count = count;
        }
        public static bool operator ==(TagStat lhs, TagStat rhs)
        {
            if (lhs == null || rhs == null) return false;

            return lhs.tag == rhs.tag;
        }
        public static bool operator !=(TagStat lhs, TagStat rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            else if (ReferenceEquals(obj, null))
            {
                return false;
            }
            else
                return false;

            //throw new System.NotImplementedException();
        }
    }
}
