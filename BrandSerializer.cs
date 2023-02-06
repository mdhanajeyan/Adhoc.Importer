using System.Xml.Serialization;

namespace Adhoc.Importer
{
    public class BrandSerializer
    {
        public static Columns Deserialize()
        {
            Columns? columns;

            XmlSerializer serializer = new(typeof(Columns), new XmlRootAttribute("columns"));

            using (StreamReader reader = new StreamReader(FolderLocation.Brand))
            {
                columns = serializer.Deserialize(reader) as Columns;
            }

            return columns;
        }
    }
}
