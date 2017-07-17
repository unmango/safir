// <copyright file="DefaultValueSection.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Core.ConfigFile
{
    using System.Configuration;
    using DefaultValues;

    public class DefaultValueSection : ConfigurationSection
    {
        [ConfigurationProperty("values", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(DefaultValueCollection))]
        public DefaultValueCollection Values
        {
            get
            {
               return this["values"] as DefaultValueCollection;
            }
        }
    }
}
