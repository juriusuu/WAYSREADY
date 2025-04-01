Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _EmissionColor ("Emission Color", Color) = (1, 1, 0, 1) // Bright yellow by default
        _EmissionIntensity ("Emission Intensity", Float) = 1.0 // Intensity of the emission
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _EmissionColor; // Emission color property
            float _EmissionIntensity; // Emission intensity property

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // Add the emission color multiplied by the intensity
                col.rgb += _EmissionColor.rgb * _EmissionIntensity;

                // Apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}