using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class CamEffect : MonoBehaviour {

    public Material EffectMaterial;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(EffectMaterial != null)
            Graphics.Blit(source, destination, EffectMaterial);
    }

}
