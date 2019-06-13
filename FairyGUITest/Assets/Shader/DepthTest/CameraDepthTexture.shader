/*
	远裁剪面不能太大，如果摄像机的远裁剪面距离很大，那么这张图的输出就会整体偏黑，
	因为离我们较近的物体距离占远裁剪面的距离太小了，几乎为0，所以就是黑的
*/
Shader "DepthTest/CameraDepthTexture"
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

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
			};

			sampler2D _CameraDepthTexture;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//o.uv = v.vertex.xy;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed depthTexture = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture , i.uv);

				fixed depth = Linear01Depth(depthTexture);

				fixed4 col = fixed4(depth, depth, depth, 1);
				return col;
			}
			ENDCG
		}
	}
}
