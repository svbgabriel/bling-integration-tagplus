namespace BlingIntegrationTagplus.Models
{
    public class Config
    {
        public string BlingApiKey { get; }
        public string BlingInitialDate { get; }
        public string TagplusToken { get; }
        public string BlingApiUrl { get; }
        public string TagplusApiUrl { get; }
        public string BlingOrderNum { get; }

        public Config(string BlingApiKey, string BlingInitialDate, string TagplusToken, string BlingApiUrl, string TagplusApiUrl, string BlingOrderNum)
        {
            this.BlingApiKey = BlingApiKey;
            this.BlingInitialDate = BlingInitialDate;
            this.TagplusToken = TagplusToken;
            this.BlingApiUrl = BlingApiUrl;
            this.TagplusApiUrl = TagplusApiUrl;
            this.BlingOrderNum = BlingOrderNum;
        }
    }
}
