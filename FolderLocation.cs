namespace Adhoc.Importer
{
    public class FolderLocation
    {
        public static readonly string App = Directory.GetCurrentDirectory();

        public static readonly string XmlDocs = Path.Combine(App, "XmlDocs");

        public static readonly string Brand = Path.Combine(XmlDocs, "Brand.xml");
    }
}
