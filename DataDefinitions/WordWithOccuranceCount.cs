namespace DataDefinitions
{
    public class WordWithOccuranceCount:Observable
    {
        private string word;

        public string Word
        {
            get => word;
            set => Set<string>(ref word, value);
        }
        private int occuranceCount;

        public int OccuranceCount
        {
            get => occuranceCount;
            set => Set<int>(ref occuranceCount, value);
        }
        internal WordWithOccuranceCount(string word,  int occuranceCount)
        {
            Word = word;
            OccuranceCount = occuranceCount;
        }
    }
}
