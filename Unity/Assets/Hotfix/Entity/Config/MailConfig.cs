using ETModel;

namespace ETHotfix
{
	[Config(AppType.ClientH |  AppType.ClientM | AppType.Gate | AppType.Map)]
	public partial class MailConfigCategory : ACategory<MailConfig>
	{
	}

	public class MailConfig : IConfig
	{
		public long Id { get; set; }
        public string MailAddress;
		public string MailAuthorizationCode;
	}
}
