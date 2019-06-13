/*
	在片段着色器中，将点根据深度图重建世界坐标，重建后的像素世界坐标包含了其余像素？
	再将其转化到CUBE坐标中，判断是否在CUBE之外，如果在CUBE之外则裁剪不需要贴花
	用重建后的点的OBJ坐标当做UV?

	float4 _ProjectionParams : 投影参数
	x is 1.0 or -1.0, negative if currently rendering with a flipped projection matrix
	x为1.0 或者-1.0如果当前渲染使用的是一个反转的投影矩阵那么为负。
	y is camera's near plane y是摄像机的近剪裁平面
	z is camera's far plane z是摄像机远剪裁平面
	w is 1/FarPlane. w是1/远剪裁平面
*/

Shader "Decal/DepthDecal"
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
				float4 screenPos : TEXCOORD1;
				float4 ray : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _CameraDepthTexture;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.screenPos = ComputeScreenPos(v.vertex);
				//o.ray = mul(UNITY_MATRIX_MV, v.vertex) * float3(-1, -1, 1,1);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//深度图UV应该是屏幕的UV！所以用screenPos来获取当前点的深度信息
				fixed2 screenUV = i.screenPos.xy / i.screenPos.w;
				fixed depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture , screenUV);
				depth = Linear01Depth(depth);

				//(i.ray * depth

				fixed4 col = tex2D(_MainTex, i.uv);

				return col;
			}
			ENDCG
		}
	}
}
