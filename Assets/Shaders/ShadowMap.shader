Shader "Unlit/ShadowMap"
{
     Properties
    {
        _MainTex("主贴图", 2D) = "white" {}
    }
        SubShader
        {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
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

            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.worldNormal = mul(unity_ObjectToWorld,float4( v.normal,0.0)).xyz;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            float4 _Color;

            float4 frag(v2f i) : SV_Target
            {
                float4 SHADOW_COORDS = TransformWorldToShadowCoord(i.worldPos);
                float3 normal_world = normalize(i.worldNormal);
                Light mainLight = GetMainLight(SHADOW_COORDS);
                half shadow = dot(mainLight.direction,normal_world);
                half shadow1 = MainLightRealtimeShadow(SHADOW_COORDS);
                //shadow1=pow(shadow1,10);
                shadow *= shadow1;
                return float4(shadow, shadow, shadow,1);
            }
            ENDHLSL
        }
         UsePass "Universal Render Pipeline/Lit/ShadowCaster"
        //pass {
        //    Name "ShadowCast"

        //    Tags{ "LightMode" = "ShadowCaster" }
        //    HLSLPROGRAM
        //    #pragma vertex vert
        //    #pragma fragment frag
        //    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        //        struct appdata
        //    {
        //        float4 vertex : POSITION;
        //    };

        //    struct v2f
        //    {
        //        float4 pos : SV_POSITION;
        //    };

        //    sampler2D _MainTex;
        //    float4 _MainTex_ST;

        //    v2f vert(appdata v)
        //    {
        //        v2f o;
        //        o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
        //        return o;
        //    }
        //    float4 frag(v2f i) : SV_Target
        //    {
        //        float4 color;
        //        color.xyz = float3(0.0, 0.0, 0.0);
        //        return color;
        //    }
        //    ENDHLSL
        //}
    }
}
