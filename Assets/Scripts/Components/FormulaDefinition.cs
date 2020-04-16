using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormulaDefinition
{
    public static FormulaDefinition LoadFromArray(string[] arr)
    {
        if (arr == null || arr.Length != 3)
        {
            return null;
        }

        FormulaDefinition def = new FormulaDefinition();

        def.SourceA = arr[0];
        def.SourceB = arr[1];
        def.Target = arr[2];

        return def;
    }

    public string SourceA
    {
        get; private set;
    }

    public string SourceB
    {
        get; private set;
    }

    public string Target
    {
        get; private set;
    }

}
