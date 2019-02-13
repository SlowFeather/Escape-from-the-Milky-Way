using ETModel;

namespace ETHotfix
{
	[Config(AppType.ClientH |  AppType.ClientM | AppType.Gate | AppType.Map)]
	public partial class ProjectConfigCategory : ACategory<ProjectConfig>
	{
	}

	public class ProjectConfig : IConfig
	{
		public long Id { get; set; }
		public string Name;
		public string Duration;
	}
}
