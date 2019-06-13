Shader "CommonShader/RimLight"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Threshold( "Threshold" , Range(0,0.5) ) = 0.3
		_LightColor( "LightColor" , Color ) = (1,1,1,1)
		_RimPower( "RimPower" , Range(1,2)) = 1.3
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
			#include "Lighting.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 worldPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _Threshold;
			fixed4 _LightColor;
			fixed _RimPower;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				float3 viewDir = normalize( UnityWorldSpaceViewDir(i.worldPos.xyz) );

				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * _LightColor.xyz;

				fixed difference = max(0, dot(viewDir , i.worldNormal));
				fixed rim = saturate(1 - difference - _Threshold);
				//if (difference < _Threshold)
				//{
					col.rgb = col.rgb + _LightColor.xyz * rim * _RimPower;
				//}

				return col;
			}
			ENDCG
		}
	}
}
