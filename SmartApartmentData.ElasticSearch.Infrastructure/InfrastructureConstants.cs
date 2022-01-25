namespace SmartApartmentData.ElasticSearch.Infrastructure;

public static class ElasticSearchConstants
{
    public const string PropertyIndexName = "property";
    public const string ManagementIndexName = "management";
    public const string EnglishStopTokenFilter = "english_stop_words";
    public const string AutoCompleteTokenizer = "auto_complete_tokenizer";
    public const string KeywordAnalyzer = "keyword_analyzer";
    public const string AutoCompleteAnalyzer = "auto_complete_analyzer";
    public const string CustomStandardAnalyzer = "custom_standard_analyzer";
    public const string ManagementFilePath = "mgmt.json";
    public const string PropertiesFilePath = "properties.json";

}