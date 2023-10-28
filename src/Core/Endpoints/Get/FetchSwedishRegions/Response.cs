namespace Shop.Core.Endpoints.Get.FetchSwedishRegions.Responses
{
    public class Response 
    {
        public List<VariableValueTexts> variables { get; set; } = new List<VariableValueTexts>();
    }

    public class VariableValueTexts 
    {
        public List<string> valueTexts { get; set; }
    }
}
