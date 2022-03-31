namespace WordleS.Models
{
    public partial class Game
    {
        public bool AllowNewTry(int nb)
        {
            return nb < MaxAttempt;
        }
    }
}
