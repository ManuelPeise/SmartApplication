namespace Logic.Shared.Interfaces
{
    public interface IAiTrainingDataCollector: IDisposable
    {
        Task CollectTrainingData();
    }
}
