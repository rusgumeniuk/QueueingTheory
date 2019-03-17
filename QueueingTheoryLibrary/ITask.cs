namespace QueueingTheoryLibrary
{
    public interface ITask
    {
        bool IsSolved { get; set; }
        void Solve();
        string GetResult();        
    }
}
