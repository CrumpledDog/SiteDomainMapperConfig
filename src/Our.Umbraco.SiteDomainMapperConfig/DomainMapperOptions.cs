﻿using System.Collections.Generic;

namespace Our.Umbraco.SiteDomainMapperConfig
{
    public class DomainMapperOptions
	{
		public List<Site> Sites { get; set; }
        public List<Binding> Bindings { get; set; }
	}

	public class Site
	{
		public string SiteName { get; set; }
		public string[] Domains { get; set; }
	}

    public class Binding
    {
        public string[] SiteNames { get; set; }
    }
}
