
/*
思路 ： 现根据当前点获取buffer中的深度，该深度为当前点的真正深度，根据深度与三角形相似，
可以重建深度点的坐标，并将其转化到模型空间中进行比较是否在立方体内，如果在，则绘制

*/

Shader "Decal/DepthDecalNew"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "Queue" = "Transparent+100" }
		Blend SrcAlpha OneMinusSrcAlpha
		//LOD 100

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
				float4 pos : SV_POSITION;
				float4 screenPosNoW : TEXCOORD1;
				float3 viewPos : TEXCOORD2;
			};

			sampler2D _CameraDepthTexture;
			//sampler2D_float _CameraDepthTexture; //可以使用全精度的
			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (a2v v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				//计算该点在屏幕上的坐标（采样深度图）
				o.screenPosNoW = ComputeScreenPos(o.pos);
				o.viewPos = UnityObjectToViewPos(v.vertex) * float3(1, 1, -1);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//重建坐标以及计算
				fixed2 screenUv = i.screenPosNoW.xy / i.screenPosNoW.w;
				//转化到（0,1），乘以far平面就可以得到真实的深度，与原点的z值就可以做比较计算
				fixed depth = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screenUv));
				fixed readDepth = depth * _ProjectionParams.z;//.z远裁剪平面，.y为近裁剪平面

				float3 rebuildViewPos = float3(i.viewPos * readDepth / i.viewPos.z);

				//转换到世界坐标中，再转换回模型坐标中
				//float3 rebuildModulePos = mul(UNITY_MATRIX_T_MV, rebuildViewPos);

				//转化到世界空间坐标
				float4 rebuildWorldPos = mul(unity_CameraToWorld, float4(rebuildViewPos, 1.0));
				//转化为物体空间坐标
				float3 rebuildModulePos = mul(unity_WorldToObject, rebuildWorldPos);

				clip(float3(0.5, 0.5, 0.5) - abs(rebuildModulePos));

				//采样用xy yz xz都是可以的,只是对应在不同方向上用不同的分量进行采样才有对应的好效果
				//猜测理解：为什么加上0.5，因为坐标是根据(0.5,0.5,0.5)这个点来进行相减对比的，所以这个点的最高点一定是0.5
				//而UV的区间范围是(0,1),所以加上0.5
				float2 uv = rebuildModulePos.xy + 0.5;  
				fixed4 col = tex2D(_MainTex, uv);

				return col;
			}
			ENDCG
		}
	}
}
