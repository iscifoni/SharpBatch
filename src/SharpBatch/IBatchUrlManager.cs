namespace SharpBatch
{
    public interface IBatchUrlManager
    {
        bool isBatch { get; }
        string RequestBatchAction { get; }
        string RequestBatchName { get; }
        BatchUrlManagerCommand RequestCommand { get; }
    }
}