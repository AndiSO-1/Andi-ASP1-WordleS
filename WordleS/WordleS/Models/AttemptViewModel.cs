namespace WordleS.Models
{
    public class AttemptViewModel
    {
        public Attempt Attempt { get; set; }

        public List<Array> CheckedChars { get; set; }
    }
}
