using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Threading.Tasks;

namespace eBest.Mobile.Entities
{
    /// <summary>
    /// Synchronization data entity
    /// Edit by Younest 2011-09-28
    /// </summary>
    [XmlRoot("tables")]
    public class SyncDataEntity
    {
        public SyncDataEntity()
        {

        }

        [XmlElement("t")]
        public List<SyncTable> Tables = new List<SyncTable>();
    }


    public class SyncTable
    {
        [XmlAttribute("n", DataType = "string")]
        public string Name { get; set; }

        [XmlAttribute("p", DataType = "string")]
        public string ParamValues { get; set; }

        [XmlElement("r")]
        public List<string> Rows = new List<string>();

    }
}
