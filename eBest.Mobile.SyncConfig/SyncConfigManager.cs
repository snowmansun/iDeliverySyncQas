using System;
using System.Text;
using System.Configuration;
using System.Collections.Generic;

namespace eBest.Mobile.SyncConfig
{
    public sealed class SyncConfigManager : ConfigurationSection
    {
        [ConfigurationProperty("packages")]
        public PackageSection Packages { get { return (PackageSection)base["packages"]; } }

        [ConfigurationProperty("commons")]
        public CommonSection Common { get { return (CommonSection)base["commons"]; } }

        [ConfigurationProperty("logic")]
        public logic Logic { get { return (logic)base["logic"]; } }
    }

    [ConfigurationCollection(typeof(package), AddItemName = "package")]
    public class PackageSection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new package();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((package)element).Name;
        }

        public package this[int index]
        {
            get { return (package)base.BaseGet(index); }
        }

        new public package this[string name]
        {
            get { return (package)base.BaseGet(name); }
        }
    }

    [ConfigurationCollection(typeof(common), AddItemName = "common")]
    public class CommonSection : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new common();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((common)element).Name;
        }

        public common this[int index]
        {
            get { return (common)base.BaseGet(index); }
        }

        new public common this[string name]
        {
            get { return (common)base.BaseGet(name); }
        }
    }

    /// <summary>
    /// 手机端版本信息配置节点；
    /// </summary>
    public class package : ConfigurationElement
    {
        /// <summary>
        /// 主键
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return this["name"].ToString(); }
            set { this["name"] = value; }
        }

        /// <summary>
        /// 手机端版本号
        /// </summary>
        [ConfigurationProperty("versionNum", IsRequired = true)]
        public string VersionNum
        {
            get { return this["versionNum"].ToString(); }
            set { this["versionNum"] = value; }
        }

        /// <summary>
        /// 手机端下载路径信息
        /// </summary>
        [ConfigurationProperty("file", IsRequired = true)]
        public string File
        {
            get { return this["file"].ToString(); }
            set { this["file"] = value; }
        }

        /// <summary>
        /// 版本更新描述文件路径信息
        /// </summary>
        [ConfigurationProperty("upDesc", IsRequired = true)]
        public string UpDesc
        {
            get { return this["upDesc"].ToString(); }
            set { this["upDesc"] = value; }
        }
    }

    /// <summary>
    /// 公共信息配置节点；
    /// </summary>
    public class common : ConfigurationElement
    {
        /// <summary>
        /// 主键
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return this["name"].ToString(); }
            set { this["name"] = value; }
        }

        /// <summary>
        /// 主键值
        /// </summary>
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return this["value"].ToString(); }
            set { this["value"] = value; }
        }
    }

    /// <summary>
    /// 同步版本配置节点；
    /// </summary>
    public class logic : ConfigurationElement
    {
        /// <summary>
        /// 版本号
        /// </summary>
        [ConfigurationProperty("number", IsRequired = true)]
        public string Number
        {
            get { return this["number"].ToString(); }
            set { this["number"] = value; }
        }

        /// <summary>
        /// 程序集
        /// </summary>
        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get { return this["type"].ToString(); }
            set { this["type"] = value; }
        }
    }
}
