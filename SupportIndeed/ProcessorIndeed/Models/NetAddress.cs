namespace ProcessorIndeed.Models
{
    public class NetAddress
    {
        public string PipeName { get; set;}
        public int Port { get; set; }
        public string IPAddress { get; set; }
        public string FullAddress => $"net.pipe://{IPAddress}/{PipeName}:{Port}";
    }
}
