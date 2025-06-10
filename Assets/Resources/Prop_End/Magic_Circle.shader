Shader "Custom/DoubleSided_MaskAndColor"
{
    Properties
    {
        _MainTex ("Mask Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        _AlphaCutoff ("Mask Cutoff", Range(0,1)) = 0.1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Cull Off            // ¾ç¸é ·»´õ¸µ
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Color;
            float _AlphaCutoff;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float mask = tex2D(_MainTex, i.uv).r;

                if (mask < _AlphaCutoff)
                    discard;

                return float4(_Color.rgb, mask);
            }
            ENDCG
        }
    }
}