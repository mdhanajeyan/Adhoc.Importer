using System.Xml.Serialization;

namespace Adhoc.Importer
{
    [XmlType("column")]
    public class Column
    {
        [XmlElement("name")]
        public string Name { get; set; }
        
        [XmlElement("entity")]
        public string Entity { get; set; }

        [XmlElement("entityColumn")]
        public string EntityColumn { get; set; }

        [XmlElement("targetTable")]
        public string TargetTable { get; set; }

        [XmlElement("targetColumn")]
        public string TargetColumn { get; set; }

        [XmlElement("foreignKey")]
        public bool ForeignKey { get; set; }


    }
    [XmlRoot("columns")]
    public class Columns
    {
        public Columns()
        {
            Items = new List<Column>();
        }
        [XmlElement("column")]
        public List<Column> Items { get; set; }
    }
}
