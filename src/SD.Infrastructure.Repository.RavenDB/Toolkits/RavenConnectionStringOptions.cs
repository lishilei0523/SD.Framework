namespace SD.Infrastructure.Repository.RavenDB.Toolkits
{
    internal class RavenConnectionStringOptions
    {
        public string[] Urls { get; set; }
        public string DefaultDatabase { get; set; }
        public override string ToString()
        {
            return $"Urls: {this.Urls}, DefaultDatabase: {this.DefaultDatabase}";
        }
    }
}
