Shader "Custom/ToonFace"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Normal("Normal",2D) = "bump" {}


        _Outline ("Outline",Float) = 0.1
        _LightY ("LightY",Range(-1,1)) = 0
        _OutlineColor ("OutlineColor", Color) =(0,0,0,1)
        [Header(Tint Base)]
    _DiffuseRamp ("DiffuseRamp", 2D) = "Black" {}
        _SpecularRamp ("SpecularRamp", 2D) = "Black" {}
        _TintBase ("Tint Base", COLOR) = (0,0,0,1)
        [Header(RampLayer1)]
        _RampLayerOffset1 ("RampLayerOffset1", Range(-0.5,0.5)) = 0.5
        _TintLayer1 ("Tint_Layer1", COLOR) = (0,0,0,1)
        [Header(RampLayer2)]
        _RampLayerOffset2 ("RampLayerOffset2", Range(-0.5,0.5)) = 0.5
        _RampLayerSoftness2 ("RampLayerSoftness2", Range(0,1)) = 0.5
        _TintLayer2 ("Tint Layer2", COLOR) = (0,0,0,1)
        [Header(RampLayer3)]
        _RampLayerOffset3 ("RampLayerOffset3", Range(-0.5,0.5)) = 0.5
        _RampLayerSoftness3 ("RampLayerSoftness3", Range(0,1)) = 0.5
        _TintLayer3 ("Tint Layer3", COLOR) = (0,0,0,1)
        [Header(Specular)]
         _Shineness ("Shineness", Float) = 0.5
         _SpecularColor ("Specular Color", COLOR) = (0,0,0,1)
         _SpecularIntensity ("SpecularIntensity", Float) = 1
         _SpecularSmooth ("Specular Smooth", Float) = 1

         [Header(Rim)]
        _RimMin("RimMin", Range(-2,2)) = 0.5
        _RimMax("RimMax", Range(-2,2)) = 0.5
        _Roughness("Roughness", Float) = 0
        _RE("_RE", Float) = 0
        _EnvIntensity("Env Intensity", Float) = 0


    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100


       
        Pass
        {
           Blend SrcAlpha OneMinusSrcAlpha
            Tags
            {
                "LightMode" = "UniversalForward"
            }
            //Zwrite off
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ Anti_Aliasing_ON

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 VertexColor:COLOR;

            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalDir: TEXCOORD1;
                float3 tangentDir: TEXCOORD2;
                float3 binormalDir: TEXCOORD3;
                float4 VertexColor: TEXCOORD4;
                float3 pos_world: TEXCOORD5;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _Normal;
            sampler2D _DiffuseRamp;
          sampler2D _SpecularRamp;
           samplerCUBE _Envmap;
            float4 _TintBase;
            float4 _TintLayer1;
            float _RampLayerOffset1;

            float4 _TintLayer2;
            float _RampLayerOffset2;
            float _RampLayerSoftness2;
            
            float4 _TintLayer3;
            float _RampLayerOffset3;
            float _RampLayerSoftness3;

            float _Shineness;
            float _SpecularIntensity;
            float4 _SpecularColor;
            float _SpecularSmooth;
            

            float _RimMax;
            float _RimMin;
            float _Roughness;
            float _EnvIntensity;
            float4 _Envmap_HDR;
            float _RE; 
            float _LightY;
             half3 DecodeHDR    (half4 data, half4 decodeInstructions) {
                // Take into account texture alpha if decodeInstructions.w is true(the alpha value affects the RGB channels) 

                half alpha = decodeInstructions.w * (data.a - 1.0) + 1.0  ;
                // If Linear mode is not supported we can skip exponent part 
            #if defined (UNITY_COLORSPACE_GAMMA) 
             return   (decodeInstructions.x * alpha) * data.rgb; 
            #else
                #if defined (UNITY_USE_NATIVE_HDR)
                    return decodeInstructions.x * data.rgb;
                // Multiplier for future HDRI relative to absolute conversion. 
                #else return (decodeInstructions.x *  pow (alpha, decodeInstructions.y)) * data.rgb;
                #endif        
            #endif
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.pos_world = mul(unity_ObjectToWorld, v.vertex);
                o.normalDir = mul(unity_ObjectToWorld,float4( v.normal,0.0)).xyz;
                o.tangentDir=normalize( mul(unity_ObjectToWorld,v.tangent).xyz);
                o.binormalDir=normalize( cross(o.normalDir,o.tangentDir)*v.tangent.w);
                o.VertexColor = v.VertexColor;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            float4 _Color;

            float4 frag(v2f i) : SV_Target
            {
                half4 Base_col = tex2D(_MainTex,i.uv);
                float4 SHADOW_COORDS = TransformWorldToShadowCoord(i.pos_world);
                float3 normal_world = normalize(i.normalDir);
                Light mainLight = GetMainLight(SHADOW_COORDS);
               // half shadow = dot(mainLight.direction,normal_world);
                half atten = MainLightRealtimeShadow(SHADOW_COORDS);


                half3 base_col = tex2D(_MainTex, i.uv).xyz;
               // half3 normal_world= normalize(i.normalDir);
                half3 tangent_world= normalize(i.tangentDir);
                half3 binormal_world= normalize(i.binormalDir);
                half3 pos_world= normalize(i.pos_world);
                half3 normal_data = UnpackNormal(tex2D(_Normal,i.uv));
                half3x3 TBN = half3x3 (tangent_world,binormal_world,normal_world);
                
                normal_world = mul(normal_data,TBN);
                half3 ViewDir = normalize(_WorldSpaceCameraPos.xyz-pos_world);
                half3 lightDir = normalize(mainLight.direction);
              //  lightDir = half3(lightDir.x,_LightY,lightDir.z);
                //漫反射
                half NdotL =  max(0,dot(normal_world.xz,lightDir.xz));
                half half_lambert = (NdotL*atten+1.0)*0.5;
                //half half_lambert = half_lambert*ao //本来要乘上AO因为素材没有算了

                half3 tint_Base_color1 = base_col;

                //第一层ramp
                half2 uv_ramp1 = half2(half_lambert+_RampLayerOffset1,0.5);
                half toon_diffuse1 = tex2D(_DiffuseRamp,uv_ramp1).r;
                half3 tint_color1 = lerp(half3(1,1,1),_TintLayer1,toon_diffuse1*_TintLayer1.a);
                
                //第二层ramp
                half2 uv_ramp2 = half2(half_lambert+_RampLayerOffset2,1-i.VertexColor.g+_RampLayerSoftness2);//1-i.VertexColor.g可以用_RampLayerSoftness2代替越大越柔和
                half toon_diffuse2 = tex2D(_DiffuseRamp,uv_ramp2).g;
                half3 tint_color2 = lerp(half3(1,1,1),_TintLayer2,toon_diffuse2*_TintLayer2.a);
                
                //第三层ramp
                half2 uv_ramp3 = half2(half_lambert+_RampLayerOffset3,1-i.VertexColor.b+_RampLayerSoftness3);//1-i.VertexColor.b可以用_RampLayerSoftness3代替越大越柔和
                half toon_diffuse3 = tex2D(_DiffuseRamp,uv_ramp3).b;
                half3 tint_color3 = lerp(half3(1,1,1),_TintLayer3,toon_diffuse3*_TintLayer3.a);
                
                half3 final_diffuse = tint_Base_color1 * tint_color1 *tint_color2 *tint_color3  ;
                //高光
                //half3 half_R = normalize(lightDir+ViewDir);
                half3 R = normalize(reflect(-lightDir,normal_world));
                half specular_term = max(pow(dot(R,ViewDir),_Shineness),0.0001);//本来要乘上AO因为素材没有算了
                specular_term = smoothstep(0.5-_SpecularSmooth*0.5,0.5+_SpecularSmooth*0.5,specular_term);//风格化高光
            
                half3 final_specular = base_col  * specular_term *_SpecularColor *atten *mainLight.color *_SpecularIntensity  ;
               
                 //边缘光,环境光
                half NDL = NdotL>0 ? 1:0;
                half fresnel = 1- dot(ViewDir,normal_world);
                half rim = smoothstep(_RimMin,_RimMax,fresnel);//本来要乘上AO因为素材没有算了
                rim = rim * NDL;

                half3 r = reflect(-ViewDir,normal_world);
                half roughness = lerp(0,0.95,saturate(_Roughness));
                roughness = roughness * (1.7-0.7*roughness );
                half mip_level = roughness * 6.0;
                half4 color_cubemap = texCUBElod(_Envmap,half4(r,mip_level));
                half3 color_env = DecodeHDR(color_cubemap,_Envmap_HDR);
                half3 final_env =lerp(rim * _EnvIntensity,color_env * rim * _EnvIntensity,_RE);

               half3 final_col = final_diffuse+final_specular + final_env ;
                
                //return Base_col;
                return float4(final_col,1);
            }
            ENDHLSL
        }
         Pass
          {
          Name "OUTLINE"
          Cull Front
          ZWrite On
          ColorMask RGBA
          Blend SrcAlpha OneMinusSrcAlpha
              HLSLPROGRAM
              #pragma vertex vert
              #pragma fragment frag
              #include "UnityCG.cginc"
              struct appdata {
                  float4 vertex : POSITION;
                  float3 normal : NORMAL;
                  float4 texCoord : TEXCOORD0;
                  float4 vertexColor : COLOR;
              };
              struct v2f {
                float4 pos : POSITION;
                float4 color : COLOR;
                float4 tex : TEXCOORD0;
              };
              sampler2D _MainTex;
              float _Outline;
              float4 _OutlineColor;
              v2f vert(appdata v) {
                  // just make a copy of incoming vertex data but scaled according to normal direction
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                  float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
                  float2 offset = TransformViewToProjection(norm.xy);
                 // float2 offset = (norm.xy);
                o.pos.xy += offset * _Outline*0.0001;
                o.tex = v.texCoord;
                o.color = v.vertexColor;
                return o;
              }
              fixed4 frag (v2f i) : SV_Target
              {
                  half4 base_col = tex2D(_MainTex,i.tex);
                  return lerp( fixed4(_OutlineColor.xyz,1),base_col,_OutlineColor.w);

              }
              ENDHLSL
          }
       
         UsePass "Universal Render Pipeline/Lit/ShadowCaster"

    }

}
