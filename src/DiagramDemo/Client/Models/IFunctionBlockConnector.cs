namespace DiagramDemo.Client.Models
{
    public interface IFunctionBlockConnector
    {
        string Description { get; set; }
        bool EventEnabled { get; set; }
        bool IsInput { get; set; }
        bool LoggingEnabled { get; set; }
        string Name { get; set; }
        FunctionBlockPoolingMode PoolingMode { get; set; }
        int RowIndex { get; set; }
    }
}
