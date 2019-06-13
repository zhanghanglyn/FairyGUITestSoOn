/*
	思路，计算当前点的深度与屏幕其余深度进行对比（视空间中），用差值来作为透明度
*/

Shader "DepthTest/BeachTestShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_InvFade ("_InvFade",Range(0.01,3.0)) = 0.1

	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }
		LOD 100

		Pass
		{
			Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

			//Blend SrcAlpha OneMinusSrcAlpha  //效果会完全不同
			Blend One OneMinusSrcColor
			ZWrite Off
			Cull Off
			ColorMask RGB
			Lighting Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			//#pragma target 2.0
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
				float4 projPos : TEXCOORD1;
				
			};

			//sampler2D _CameraDepthTexture;
			UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture); //尝试了下，好像直接写sampler2D也没有问题
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _InvFade;
			
			v2f vert (a2v v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos( v.vertex );
				o.uv.xy = v.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw;

				//ComputeScreenPos 计算出的并不是真的screenPos，而是没有除以w的POS，因为如果在此计算，则不好进行线性差值
				o.projPos = ComputeScreenPos(o.pos);
				//#define COMPUTE_EYEDEPTH(o) o = -UnityObjectToViewPos( v.vertex ).z
				COMPUTE_EYEDEPTH(o.projPos.z);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//根据上面的屏幕空间位置，进行透视采样深度图（tex2dproj，即带有透视除法的采样，相当于tex2d（xy/w）），
				//得到当前像素对应在屏幕深度图的深度，并转化到视空间，线性化（深度图中已有的不透明对象的深度）
				float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));

				//根据自身与世界中的深度差值计算a
				fixed fade = saturate(_InvFade * (sceneZ - i.projPos.z));

				fixed4 col = tex2D(_MainTex, i.uv.xy);
				col.a *= fade;
				col.rgb *= fade;  //颜色相乘只是为了效果，也可以不乘,看使用那种blend模式，One OneMinusSrcColor需要乘
				return col;
			}
			ENDCG
		}
	}
}
