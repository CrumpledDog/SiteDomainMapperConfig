using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Routing;
using Microsoft.Extensions.Logging;

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

	public class SiteDomainMapperComponentComposer : ComponentComposer<SiteDomainMapperComponent>
	{ }
}
