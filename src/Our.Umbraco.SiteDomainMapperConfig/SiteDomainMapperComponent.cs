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
            var sites = builder.Config.GetSection("DomainMapper").Get<List<Site>>();
            builder.Services.Configure<DomainMapperOptions>(options => options.Sites = sites);

            builder.Components().Append<SiteDomainMapperComponent>();
        }
    }
}
