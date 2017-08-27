// <copyright file="DefaultValueElement.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Core.ConfigFile.DefaultValues
{
    using System;
    using System.Configuration;

    public class DefaultValueElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        public string Key
        {
            get { return this["key"] as string; }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("value", IsRequired = true, IsKey = false)]
        public string Value
        {
            get {
                return
                    //(this["value"] as string).ReplaceEnvironmentVariables();
                    Environment.ExpandEnvironmentVariables(this["value"] as string);
            }

            set {
                this["value"] = value;
            }
        }
    }
}
