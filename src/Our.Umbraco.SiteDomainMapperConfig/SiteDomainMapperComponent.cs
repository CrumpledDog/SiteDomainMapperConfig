using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Routing;

namespace Our.Umbraco.SiteDomainMapperConfig
{
	public class SiteDomainMapperComponent : IComponent
	{
        private readonly SiteDomainMapper? _siteDomainMapper;
		private readonly DomainMapperOptions _domainMapperOptions;
		private readonly ILogger<SiteDomainMapperComponent> _logger;

		public SiteDomainMapperComponent(ISiteDomainMapper siteDomainMapper, IOptions<DomainMapperOptions> domainMapperOptions, ILogger<SiteDomainMapperComponent> logger)
		{
			_domainMapperOptions = domainMapperOptions.Value;
			_logger = logger;

			// SiteDomainMapper can be overwritten, so ensure it's the default one which contains the AddSite
			if (siteDomainMapper is SiteDomainMapper concreteSiteDomainMapper)
			{
				_siteDomainMapper = concreteSiteDomainMapper;
			}
		}
		/// <inheritdoc/>
		public void Initialize()
		{
            if (_domainMapperOptions.Sites != null)
            {
                foreach (var site in _domainMapperOptions.Sites)
                {
                    _siteDomainMapper?.AddSite(site.SiteName, site.Domains);
                    _logger.LogInformation("SiteDomainMapper added site {SiteName} with {Domains} domains", site.SiteName, string.Join(", ", site.Domains));
                }
            }
            if (_domainMapperOptions.Bindings != null)
            { 
                foreach (var binding in _domainMapperOptions.Bindings)
                {
                    _siteDomainMapper?.BindSites(binding.SiteNames);
                    _logger.LogInformation("SiteDomainMapper added binding with {SiteNames} sites", string.Join(", ", binding.SiteNames));
                }
			}
		}

		/// <inheritdoc/>
		public void Terminate()
		{ }
	}

    public class SiteDomainMapperComponentComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            var domainMapperSection = builder.Config.GetSection("DomainMapper");
            var sites = domainMapperSection.GetSection("Sites").Get<List<Site>>();

            var bindings = domainMapperSection.GetSection("Bindings").Get<string[][]>();

            var bindingsList = new List<Binding>();
            foreach (var binding in bindings)
            {
                var bindingItem = new Binding { SiteNames = binding };
                bindingsList.Add(bindingItem);
            }

            builder.Services.Configure<DomainMapperOptions>(options => {
                options.Sites = sites;
                options.Bindings = bindingsList;
            });

            builder.Components().Append<SiteDomainMapperComponent>();
        }
    }
}
