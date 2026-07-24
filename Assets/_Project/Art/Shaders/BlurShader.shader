Shader "Custom/Blur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0, 5)) = 1
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
            float _BlurSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Blur simples (média de 9 pixels)
                float2 texelSize = _BlurSize * 0.001;
                fixed4 col = tex2D(_MainTex, i.uv);
                col += tex2D(_MainTex, i.uv + float2(-texelSize.x, -texelSize.y));
                col += tex2D(_MainTex, i.uv + float2(0, -texelSize.y));
                col += tex2D(_MainTex, i.uv + float2(texelSize.x, -texelSize.y));
                col += tex2D(_MainTex, i.uv + float2(-texelSize.x, 0));
                col += tex2D(_MainTex, i.uv + float2(texelSize.x, 0));
                col += tex2D(_MainTex, i.uv + float2(-texelSize.x, texelSize.y));
                col += tex2D(_MainTex, i.uv + float2(0, texelSize.y));
                col += tex2D(_MainTex, i.uv + float2(texelSize.x, texelSize.y));
                return col / 9.0;
            }
            ENDCG
        }
    }
}