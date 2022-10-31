using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class MaterialStorage : MonoBehaviour
{
    private void Awake()
    {
        shiftColor("_Red", true);
        shiftColor("_Blue", true);
        shiftColor("_Green", true);
    }

    public void shiftColor(string c, bool show)
    {
        int x = 0;
        if (show)
            x = 1;

        var mats = GetComponent<Renderer>().sharedMaterials;
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i].SetFloat(c, x);
        }
    }

    
}
