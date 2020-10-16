namespace MS.AFORO255.Cross.Consul.Fabio
{
    public class FabioOptions
    {
        public bool Enabled { get; set; }
        public string Url { get; set; }
        public string Service { get; set; }
        public int RequestRetries { get; set; }
    }
}