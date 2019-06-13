/*
	只需要知道4个点的射线并在vertex顶点函数中选择对应的那条计算出来就可以了，会将其插值传递给片段着色器
	不需要去计算了
*/

Shader "DepthTest/RBWorldRay"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			ZTest Always Cull Off ZWrite Off

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
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 ray : TEXCOORD1;
			};

			sampler2D _CameraDepthTexture;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4x4 _RayMatrix;
			
			v2f vert (a2v v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;// TRANSFORM_TEX(v.uv, _MainTex);

				//屏幕坐标，根据uv选择在哪一个射线区间
				if (v.uv.x < 0.5 && v.uv.y > 0.5)
					o.ray = _RayMatrix[0];
				else if (v.uv.x < 0.5 && v.uv.y < 0.5)
					o.ray = _RayMatrix[1];
				else if (v.uv.x > 0.5 && v.uv.y > 0.5)
					o.ray = _RayMatrix[2];
				else if (v.uv.x > 0.5 && v.uv.y < 0.5)
					o.ray = _RayMatrix[3];

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture , i.uv);
				depth = LinearEyeDepth(depth);

				float3 worldPos = _WorldSpaceCameraPos + i.ray * depth;

				// sample the texture
				fixed4 col = fixed4(worldPos,1);
				
				return col;
			}
			ENDCG
		}
	}
}
