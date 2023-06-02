using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class MaterialStorage : MonoBehaviour
{
    Volume effects;
    public Color refColor = Color.white;

    private void Awake()
    {
        effects = GetComponent<Volume>();

        shiftColor("_Red", true);
        shiftColor("_Blue", true);
        shiftColor("_Green", true);
    }

    public void shiftColor(string c, bool show)
    {
        int x = 0;
        if (show)
            x = 1;

        // loop through stored materials and change their shaders. Have you tried figuring out what a sub shader is yet? Sounds a lot easier.
        var mats = GetComponent<Renderer>().sharedMaterials;
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i].SetFloat(c, x);
        }

        AdjustRefColor(c, show);

        // shift any extra color out of view with a volume effect (lights and stuff)
        ColorAdjustments ca;
        if(effects.profile.TryGet<ColorAdjustments>(out ca))
            ca.colorFilter.value = refColor;
    }

    // adjust the reference color to match the currently selected dimensions
    private void AdjustRefColor(string c, bool show)
    {
        // I know it's dirty but I can't think of anything better at 9:43 PM
        // first check if we are showing or hiding a color then use a switch to determin what color to shwo or hide
        if (show)
        {
            switch (c)
            {
                case "_Red":
                    refColor = new Color(1, refColor.g, refColor.b);
                    break;
                case "_Green":
                    refColor = new Color(refColor.r, 1, refColor.b);
                    break;
                case "_Blue":
                    refColor = new Color(refColor.r, refColor.g, 1);
                    break;
            }
        }
        else
        {
            switch (c)
            {
                case "_Red":
                    refColor = new Color(0, refColor.g, refColor.b);
                    break;
                case "_Green":
                    refColor = new Color(refColor.r, 0, refColor.b);
                    break;
                case "_Blue":
                    refColor = new Color(refColor.r, refColor.g, 0);
                    break;
            }
        }
    }
}
