namespace DiagramDemo.Client.Models
{
    public class FunctionBlockConnector<T> : IFunctionBlockConnector
    {
        public string Description { get; set; }
        public bool EventEnabled { get; set; }
        public bool IsInput { get; set; }
        public bool LoggingEnabled { get; set; }
        public string Name { get; set; }
        public FunctionBlockPoolingMode PoolingMode { get; set; }
        public int RowIndex { get; set; }
    }
}
