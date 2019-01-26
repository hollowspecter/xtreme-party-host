using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbishSpawner { 


    public static bool AddRubbishToObject(GameObject addingObject, float _disgustness, float timeToClean = 5.0f)
    {
        Rubbish rubbish = addingObject.AddComponent<Rubbish>();
        if (!rubbish)
            return false;
        rubbish.disgustness = _disgustness;
        rubbish.setNeededTime(timeToClean);
        return true;
    }
}
