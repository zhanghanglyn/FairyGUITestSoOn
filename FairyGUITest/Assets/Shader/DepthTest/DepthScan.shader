Shader "DepthTest/DepthScan"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ScanDepth("ScanDepth" , Range(0,1)) = 0.1
		_Width("Width" , Range(0,0.5)) = 0.02
		_Warp("_Warp" , Range(0,1)) = 0.1
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

			struct a2v
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float3 uv : TEXCOORD0; //用第三个分量来存sin的值
				float4 vertex : SV_POSITION;
				float4 viewPos : TEXCOORD1;
			};

			sampler2D _CameraDepthTexture;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _ScanDepth;
			fixed _Width;
			fixed _Warp;
			
			v2f vert (a2v v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);

				//取出该点在screen中的POS用来计算后续深度
				//o.viewPos = ComputeScreenPos(o.vertex);
				//o.viewPos = o.viewPos / o.vertex.w;

				o.uv.z = sin(_Time.y) / 10;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv.xy);
				depth = Linear01Depth(depth);

				fixed _sinDep = i.uv.z * _ScanDepth;
				if (_sinDep + _Width > depth && depth > _sinDep)
				{
					//可以用各种情况来修改UV
					i.uv.xy += _Warp;
				}

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv.xy);
				if (_sinDep + _Width > depth && depth > _sinDep)
				{
					col = col * fixed4(0.1,0.5,0.4,1);
				}

				return col;
			}
			ENDCG
		}
	}
}
