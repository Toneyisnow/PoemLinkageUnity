using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormulaDefinition
{
    private string sourceCharIdA = string.Empty;
    private string sourceCharIdB = string.Empty;
    private string targetCharId = string.Empty;


    public static FormulaDefinition LoadFromArray(string[] arr)
    {
        if (arr == null || arr.Length != 3)
        {
            return null;
        }

        FormulaDefinition def = new FormulaDefinition();

        def.sourceCharIdA = arr[0];
        def.sourceCharIdB = arr[1];
        def.targetCharId = arr[2];

        return def;
    }

}
