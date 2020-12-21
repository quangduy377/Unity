
Shader "Redeye/Particle/Multiply" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
	}

Category {
    Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
    Blend Zero SrcColor
    Cull Off Lighting Off ZWrite Off

    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_particles
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
            uniform fixed4 _TintColor;

            struct appdata_t {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };

            v2f vert (appdata_t v)
            {
                v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 textureColor = i.color * tex2D(_MainTex, i.texcoord);
                fixed4 col = lerp(half4(1,1,1,1), textureColor, textureColor.a); // texture towards black 
                UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(1,1,1,1)); // fog towards white 
                return col;
            }
            ENDCG
        }
    }
}
}
