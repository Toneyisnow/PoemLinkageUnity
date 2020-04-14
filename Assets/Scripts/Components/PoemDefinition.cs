using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft;
using Newtonsoft.Json;

public class PoemDefinition
{
    [JsonProperty(PropertyName = "id")]
    public string Id
    {
        get; set;
    }

    [JsonProperty(PropertyName = "title")]
    public List<string> Title
    {
        get; set;
    }

    [JsonProperty(PropertyName = "author")]
    public List<string> Author
    {
        get; set;
    }

    [JsonProperty(PropertyName = "line_count")]
    public int LineCount
    {
        get; set;
    }

    [JsonProperty(PropertyName = "column_count")]
    public int ColumnCount
    {
        get; set;
    }

    [JsonProperty(PropertyName = "content")]
    public List<List<string>> Content
    {
        get; set;
    }

    /// <summary>
    /// Should be updated to config file
    /// </summary>
    public int SentenceLength
    {
        get
        {
            if (this.Content == null || this.Content[0] == null)
            {
                return 0;
            }

            return this.Content[0].Count;
        }
    }

    public static PoemDefinition LoadFromJsonText(string jsonText)
    {
        PoemDefinition def = new PoemDefinition();

        def = JsonConvert.DeserializeObject<PoemDefinition>(jsonText);

        return def;
    }
}
