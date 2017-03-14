using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class NeedUtils : MonoBehaviour {

    public static void InitializeNeeds(PillAi pill)
    {
        var gc = GameObject.FindObjectOfType<GameController>();
        if (pill.GetNeedsDictionary() == null)
        {
            pill.SetNeedsDictionary(new Dictionary<NeedType, double>());
        }
        foreach (var key in gc.DefaultNeedVaules.Keys)
        {
            pill.AppendNewNeed(new KeyValuePair<NeedType, double>(key, gc.DefaultNeedVaules[key]));
        }
    }
}
