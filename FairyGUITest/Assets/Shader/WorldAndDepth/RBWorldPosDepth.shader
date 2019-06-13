//通过深度重建世界坐标 需要计算一个当前的VP矩阵的逆矩阵，把NDC中的坐标返回计算世界坐标

Shader "DepthTest/RBWorldPosDepth"
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
			};

			sampler2D _CameraDepthTexture;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4x4 viewProjectInverseMatrix;

			v2f vert (a2v v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//取出非线性的深度，并转化到NDC中
				float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture , i.uv);
				#if defined(UNITY_REVERSED_Z)
					depth = 1 - depth;
				#endif
				//depth = depth * 2 - 1;
				//同理将i.uv坐标的xy进行逆转换到NDC坐标中
				float4 ndcPos = float4(i.uv.x * 2 - 1, i.uv.y * 2 - 1, depth * 2 - 1, 1);

				//乘上当前view和投影矩阵的逆矩阵，将其变化到世界空间中，这里有个很绕的问题，本来变换到
				//NDC控件是需要除以w的，但是有个维度损失的问题，大佬推导逆运算时反而需要除以一个w
				float4 worldPos = mul(viewProjectInverseMatrix, ndcPos);
				worldPos = worldPos / worldPos.w;

				// sample the texture
				//fixed4 col = fixed4(0.2,0.5,0.4 ,1);
				fixed4 col = worldPos;
				return col;
			}
			ENDCG
		}
	}
}
