///只是使用位置变化把图片贴上去而已，没有做三角形剔除,需要第二套UV
Shader "Decal/Decal"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DecalTex("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			//Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct a2v
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv_decal : TEXCOORD1;
			};

			struct v2f
			{
				float4 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _DecalTex;
			float4 _DecalTex_ST;
			
			v2f vert (a2v v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv.xy = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
				o.uv.zw = v.uv_decal * _DecalTex_ST.xy + _DecalTex_ST.zw;
				
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col_src = tex2D(_MainTex, i.uv.xy);
				fixed4 col_dst = tex2D(_DecalTex, i.uv.zw);

				fixed4 col = lerp(col_src , col_dst,1);

				return col;
			}
			ENDCG
		}
	}
}
