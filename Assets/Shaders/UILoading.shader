Shader "UI/UILoading"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NothingColor("NothingColor", COLOR) = (0,0,0,0)
        //_OriginColor("_OriginColor", COLOR) = (0,0,0,0)
       // _LoadingColor("_LoadingColor", COLOR) = (1,1,1,1)
        _Loading("Loading", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
        
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _NothingColor;
            float4 _OriginColor;
            //float4 _LoadingColor;
            float _Loading;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                half value1 = col.a;//区分无关区域;
                half value2 = step(i.uv.x,_Loading);
                fixed4 col2 = lerp(_NothingColor,value1.xxxx,value2);
                //return i.uv.xxxx;
                fixed4 fin_col = lerp(_NothingColor,col2,value1);//区分无关区域;

                return fin_col;
            }
            ENDCG
        }
    }
}
