public class MyClassEvent 
{
    public static readonly string MYCLASS_SCORED = "MYCLASS_SCORED";
    public static readonly string MYCLASS_DIED = "MYCLASS_DIED";

    public struct MyClassScoredEvent
    {
        public int Score { get; private set; }

        public MyClassScoredEvent(int score)
        {
            Score = score;
        }
    }

    public struct MyClassDiedEvent
    {
        public string KillerName { get; private set; }

        public MyClassDiedEvent(string killerName)
        {
            KillerName = killerName;
        }
    }

    public struct MyClassWinnedEvent { }
}
