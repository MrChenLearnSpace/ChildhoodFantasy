using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class MaterialsDebug : MonoBehaviour
{
   // [Range(0,0.5f)]
    public float OutLine;
    public Color OutlineColor;
    public float temp;

    public Texture DiffuseRamp;
    public Texture SpecularRamp;
    public Color TintBase;
    [Header("RampLayer1")]
    public float RampLayerOffset1;
    public Color TintLayer1;

     [Header("RampLayer2")]
    public float RampLayerOffset2;
    [Range(0, 1.0f)]
    public float RampLayerSoftness2;
    public Color TintLayer2;
   
        [Header("RampLayer3")]
    public float RampLayerOffset3;
    [Range(0, 1.0f)]
    public float RampLayerSoftness3;
    public Color TintLayer3;
  
        [Header("Specular")]
    public float Shineness;
    public Color SpecularColor;
    public float SpecularIntensity;
    public float SpecularSmooth;
    [Header("Env")]
    [Range(-2,2)]
    public float _RimMax;
    [Range(-2, 2)]
    public float _RimMin;
    public float _Roughness;
    public float _EnvIntensity;

    public List<Material> materials;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        for(int i=0;i<materials.Count;i++) {
            materials[i].SetFloat("_Outline", OutLine);
            materials[i].SetFloat("temp", temp);
            materials[i].SetTexture("_DiffuseRamp",DiffuseRamp);
            materials[i].SetTexture("_SpecularRamp", SpecularRamp);

            materials[i].SetFloat("_RampLayerOffset1", RampLayerOffset1);
            materials[i].SetFloat("_RampLayerOffset2", RampLayerOffset2);
            materials[i].SetFloat("_RampLayerSoftness2", RampLayerSoftness2);  
            materials[i].SetFloat("_RampLayerOffset3", RampLayerOffset3);
            materials[i].SetFloat("_RampLayerSoftness3", RampLayerSoftness3);
            materials[i].SetFloat("_Shineness", Shineness);
            materials[i].SetFloat("_SpecularIntensity", SpecularIntensity);
            materials[i].SetFloat("_SpecularSmooth", SpecularSmooth);
            materials[i].SetFloat("_RimMax", _RimMax);
            materials[i].SetFloat("_RimMin", _RimMin);
            materials[i].SetFloat("_Roughness", _Roughness);
            materials[i].SetFloat("_EnvIntensity", _EnvIntensity);

            materials[i].SetColor("_OutlineColor", OutlineColor);
            materials[i].SetColor("_TintBase", TintBase);
            materials[i].SetColor("_TintLayer1", TintLayer1);
            materials[i].SetColor("_TintLayer2", TintLayer2);
            materials[i].SetColor("_TintLayer3", TintLayer3);
            materials[i].SetColor("_SpecularColor", SpecularColor);

           

        }
    }
}
