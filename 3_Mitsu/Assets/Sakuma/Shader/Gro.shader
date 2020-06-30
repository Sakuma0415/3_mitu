Shader "Unlit/Gro"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_MainTex2 ("Texture2", 2D) = "white" {}
		_MainTex3 ("Texture3", 2D) = "white" {}
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
				float2 uv2 : TEXCOORD1;
				float2 uv3 : TEXCOORD2;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float2 uv3 : TEXCOORD2;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            
			sampler2D _MainTex;
			sampler2D _MainTex2;
			sampler2D _MainTex3;

            float4 _MainTex_ST;
			float4 _MainTex2_ST;
			float4 _MainTex3_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = TRANSFORM_TEX(v.uv2, _MainTex2);
				o.uv3 = TRANSFORM_TEX(v.uv3, _MainTex3);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 col2 = tex2D(_MainTex2, i.uv2);
				fixed4 col3 = tex2D(_MainTex3, i.uv3);

				fixed4 end=(col*col3.r)+(col2*(1-col3.r));
				end.a=0.5;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, end);
                return end;
            }
            ENDCG
        }
    }
}