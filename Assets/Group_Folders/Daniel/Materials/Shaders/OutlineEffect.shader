Shader "Custom/OutlineEffect" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range(0.001, 0.1)) = 0.01
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _OutlineColor;
            float _OutlineWidth;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // Sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // Calculate the outline
                fixed4 outlineCol = _OutlineColor;
                float4 nearbyCol;

                nearbyCol = tex2D(_MainTex, i.uv + float2(_OutlineWidth, 0));
                outlineCol.a *= 1 - clamp(nearbyCol.a - col.a, 0, 1);

                nearbyCol = tex2D(_MainTex, i.uv + float2(-_OutlineWidth, 0));
                outlineCol.a *= 1 - clamp(nearbyCol.a - col.a, 0, 1);

                nearbyCol = tex2D(_MainTex, i.uv + float2(0, _OutlineWidth));
                outlineCol.a *= 1 - clamp(nearbyCol.a - col.a, 0, 1);

                nearbyCol = tex2D(_MainTex, i.uv + float2(0, -_OutlineWidth));
                outlineCol.a *= 1 - clamp(nearbyCol.a - col.a, 0, 1);

                // Blend the outline with the original color
                return lerp(outlineCol, col, col.a);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}