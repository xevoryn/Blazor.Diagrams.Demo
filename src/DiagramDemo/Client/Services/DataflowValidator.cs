using DiagramDemo.Client.Models;

namespace DiagramDemo.Client.Services
{
    /// <summary>
    /// Beispielklasse für externe Validationslogik.
    /// </summary>
    public static class DataflowValidator
    {
        public static bool CanAttachTo(IFunctionBlockConnector sourceConnector, IFunctionBlockConnector destinationConncetor)
        {
            if (sourceConnector.IsInput == destinationConncetor.IsInput)
                return false;

            if (sourceConnector.GetType().GetGenericArguments()[0] != destinationConncetor.GetType().GetGenericArguments()[0])
                return false;

            return true;
        }
    }
}
