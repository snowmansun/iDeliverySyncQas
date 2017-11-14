using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Threading.Tasks;

namespace eBest.Mobile.SyncEntities
{

    /// <summary>
    ///ScriptEntity
    ///
    /// </summary>
    [XmlRoot("Scripts")]
    public class ScriptEntity
    {
        public ScriptEntity()
        {

        }

        [XmlElement("Script")]
        public List<Script> Scripts = new List<Script>();

        public Script FindScript(string version)
        {
            Script script;
            if(string.IsNullOrEmpty(version))
            {
                script = Scripts[0];
            }
            else
            {
                script = Scripts.Find(
                            delegate(Script item)
                            {
                                return item.Version == version;
                            });
            }
            if(script != null)
                return script;

            // 第一个配置的版本号V2，作为默认值不参与比较
            for (int i = Scripts.Count - 1; i >= 1; i--)
            {
                if (CompareVersion(version,Scripts[i].Version))
                {
                    script = Scripts[i];
                }
            }

            if (script == null)
                script = Scripts[0];

            return script;
        }

        /// <summary>
        /// 判断ClientVersion是否大于配置Version
        /// </summary>
        /// <param name="clientVersion"></param>
        /// <param name="configVersion"></param>
        /// <returns></returns>
        public static bool CompareVersion(string clientVersion, string configVersion)
        {
            bool result = false;
            // 前边加1，为了防止 0.01 = 0.1的情况
            if (int.Parse("1" + clientVersion.Replace(".", "")) > int.Parse("1" + configVersion.Replace(".", "")))
            {
                result = true;
            }

            return result;
        }
    }

    [XmlType("ScriptType")]
    public enum ScriptType
    {
        [XmlEnum(Name = "Unknown")]
        Unknown,
        [XmlEnum(Name = "SQL")]
        SQL,
        [XmlEnum(Name = "Assembly")]
        Assembly
    }



    public class Script
    {

        [XmlAttribute("Version", DataType = "string")]
        public string Version { get; set; }

        [XmlElement(ElementName = "Down", Type = typeof(DownScript))]
        public DownScript Down = new DownScript();

        [XmlElement(ElementName = "Up", Type = typeof(UpScript))]
        public UpScript Up = new UpScript();

        [XmlArray("Columns"), XmlArrayItem("Column")]
        public ColumnCollection Columns = new ColumnCollection();

        [XmlIgnore()]
        public ColumnCollection UploadColumns
        {
            get
            {
                ColumnCollection lst = new ColumnCollection();
                foreach (Column col in Columns)
                {
                    if (col.NeedUpload) lst.Add(col);
                }
                return lst;
            }
        }

        [XmlIgnore()]
        public ColumnCollection InsertColumns
        {
            get
            {
                ColumnCollection lst = new ColumnCollection();
                foreach (Column col in Columns)
                {
                    if (col.NeedInsert) lst.Add(col);
                }
                return lst;
            }
        }

        [XmlIgnore()]
        public ColumnCollection DownloadColumns
        {
            get
            {
                ColumnCollection lst = new ColumnCollection();
                foreach (Column col in Columns)
                {
                    if (col.NeedDownload) lst.Add(col);
                }
                return lst;
            }
        }

        public string GetColumnList()
        {
            List<string> lst = new List<string>();
            foreach (Column col in Columns)
            {
                lst.Add(col.Name);
            }
            return string.Join(",", lst.ToArray());
        }

        public string GetUploadColumnList()
        {
            List<string> lst = new List<string>();
            foreach (Column col in Columns)
            {
                if (col.NeedUpload) lst.Add(col.Name);
            }
            return string.Join(",", lst.ToArray());
        }

        public string GetInsertColumnList()
        {
            List<string> lst = new List<string>();
            foreach (Column col in Columns)
            {
                if (col.NeedInsert) lst.Add(col.Name);
            }
            return string.Join(",", lst.ToArray());
        }

        public string GetDownloadColumnList()
        {
            List<string> lst = new List<string>();
            foreach (Column col in Columns)
            {
                if (col.NeedDownload) lst.Add(col.Name);
            }
            return string.Join(",", lst.ToArray());
        }

        public Column GetColumn(string columnName)
        {
            return Columns.Find(
                    delegate(Column col)
                    {
                        return col.Name == columnName;
                    }
                );
        }

    }

    public class ColumnCollection : List<Column> { }

    public class Column
    {
        public Column() { }

        public Column(string name, string type, bool key, int length)
        {
            Name = name;
            Type = type;
            IsPrimaryKey = key;
            if (length != 0) Length = length;
        }

        [XmlAttribute("Name", DataType = "string")]
        public string Name { get; set; }

        [XmlAttribute("Type", DataType = "string")]
        public string Type { get; set; }


        [XmlAttribute("Key", DataType = "boolean")]
        public bool IsPrimaryKey
        {
            get;
            set;
        }

        [XmlAttribute("Length", DataType = "int")]
        public int Length { get; set; }

        private bool needUpload = true;
        [XmlAttribute("Upload", DataType = "boolean")]
        public bool NeedUpload
        {
            get { return needUpload; }
            set { needUpload = value; }
        }

        private bool needInsert = true;
        [XmlAttribute("Insert", DataType = "boolean")]
        public bool NeedInsert
        {
            get { return needInsert; }
            set { needInsert = value; }
        }

        private bool needDownload = true;
        [XmlAttribute("Download", DataType = "boolean")]
        public bool NeedDownload
        {
            get { return needDownload; }
            set { needDownload = value; }
        }
    }

    public class DownScript
    {
        [XmlAttribute("Type")]
        public ScriptType Type { get; set; }

        private string code = "";
        [XmlText()]
        public string Code
        {
            get { return code; }
            set { code = Convert.ToString(value).Trim().Replace("\r\n", ""); }
        }
    }

    public class UpScript
    {
        [XmlAttribute("Type")]
        public ScriptType Type { get; set; }

        private string insertCode = "";
        [XmlElement("I")]
        public string InsertCode
        {
            get { return insertCode; }
            set { insertCode = Convert.ToString(value).Trim().Replace("\r\n", ""); }
        }

        private string updateCode = "";
        [XmlElement("U")]
        public string UpdateCode
        {
            get { return updateCode; }
            set { updateCode = Convert.ToString(value).Trim().Replace("\r\n", ""); }
        }
    }
}
