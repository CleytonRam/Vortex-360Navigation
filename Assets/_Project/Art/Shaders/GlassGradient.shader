Shader "Custom/GlassGradient"
{
    Properties
    {
        _TopColor ("Top Color", Color) = (0.1, 0.2, 0.4, 1)
        _BottomColor ("Bottom Color", Color) = (0.3, 0.6, 0.9, 1)
        _Opacity ("Opacity", Range(0,1)) = 0.8
        _Gloss ("Gloss (Brilho)", Range(0,1)) = 0.3
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // Habilita transparência
            ZWrite Off

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

            float4 _TopColor;
            float4 _BottomColor;
            float _Opacity;
            float _Gloss;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Gradiente vertical (de cima para baixo)
                float gradient = 1 - i.uv.y; // 1 no topo, 0 embaixo
                float4 gradColor = lerp(_BottomColor, _TopColor, gradient);

                // Efeito "vidro" (um brilho falso baseado na posição UV)
                float gloss = sin(i.uv.x * 20 + i.uv.y * 10) * 0.5 + 0.5;
                gloss *= _Gloss;
                gradColor.rgb += gloss * 0.3;

                // Aplica a opacidade
                gradColor.a = _Opacity;

                return gradColor;
            }
            ENDCG
        }
    }
}