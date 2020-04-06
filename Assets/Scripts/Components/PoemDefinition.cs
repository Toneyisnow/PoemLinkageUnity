using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft;
using Newtonsoft.Json;

public class PoemDefinition
{
    public int Id
    {
        get; set;
    }

    public List<string> Title
    {
        get; set;
    }

    public List<string> Author
    {
        get; set;
    }

    public List<List<string>> Content
    {
        get; set;
    }

    public static PoemDefinition LoadFromJsonText(string jsonText)
    {
        PoemDefinition def = new PoemDefinition();

        def = JsonConvert.DeserializeObject<PoemDefinition>(jsonText);

        return def;
    }
}
