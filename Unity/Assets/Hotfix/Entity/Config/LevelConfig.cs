using ETModel;

namespace ETHotfix
{
	[Config(AppType.ClientH |  AppType.ClientM | AppType.Gate | AppType.Map)]
	public partial class LevelConfigCategory : ACategory<LevelConfig>
	{
	}

	public class LevelConfig : IConfig
	{
		public long Id { get; set; }
        public int Num;
		public string Name;
		public string Description;
	}
}
