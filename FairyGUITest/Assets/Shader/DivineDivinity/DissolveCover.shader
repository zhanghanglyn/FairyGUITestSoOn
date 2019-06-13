/*
	并不是一个后期屏幕特效，但是需要计算在输出时的屏幕的位置，如果刚好位于屏幕中间的区域则溶解
	还可以加上优化，比如拉的越近，则与中心的宽度就应该越宽
*/

Shader "DivineDivinity/DissolveCover"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Noise("Noise" , 2D) = "white" {}

		_DissolveRadius( "DissolveRadius" , Float ) = 0.1
		_DissolveThreshold("DissolveThreshold" , Range(0,2)) = 0.4  //溶解阈值
		_DissolveDistance("_DissolveDistance" , Float) = 2			//在摄像机的该段距离内会溶解
			_DissolveDistanceFactor("DissolveDistanceFactor", Range(0,3)) = 3
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

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
				float4 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;

				float4 ScreenPosNoW : TEXCOORD1;
				float3 ViewPos : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _Noise;
			float4 _Noise_ST;

			fixed _DissolveRadius;
			fixed _DissolveThreshold;
			fixed _DissolveDistance;
			fixed _DissolveDistanceFactor;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv.zw = TRANSFORM_TEX(v.uv, _Noise);

				//未除以w的屏幕空间坐标，认为宽高为1，除以w之后可以将其限定在(0,1)区间中?不对，除以w还要转换才是在0,1，
				//未除以w应该是在(-1,1)区间中
				o.ScreenPosNoW = ComputeGrabScreenPos(o.vertex);

				//判断在视线空间中的位置到0,0点的距离，既是到摄像机的距离
				o.ViewPos = UnityObjectToViewPos(v.vertex); //ObjSpaceViewDir

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 screenPos = i.ScreenPosNoW.xy / i.ScreenPosNoW.w;
				fixed dis = distance(screenPos, fixed2(0.5,0.5));
				float2 dir = float2(0.5, 0.5) - screenPos;
				float distance = sqrt(dir.x * dir.x + dir.y * dir.y);
				float disolveFactor = (0.5 - dis); //(0.5 - distance);

				fixed4 col = tex2D(_MainTex, i.uv.xy);
				
				//采样一下噪点图用来做溶解
				fixed Dissolve = tex2D(_Noise, i.uv.zw).r;

				//计算一个是否在摄像机到主角的溶解距离中
				//fixed CameraToPosDis = -i.ViewPos.z * _DissolveDistanceFactor; 
				//限定在0-1之中，与r值相对应
				fixed CameraToPosDis = max(0, (_DissolveDistance - (-i.ViewPos.z)) / _DissolveDistance) * _DissolveDistanceFactor;

				//在距离内的才溶解,并且判断是否在摄像机到主角的距离内
				//if (CameraToPosDis < _DissolveDistance && Dissolve < disolveFactor)
				if (Dissolve < (disolveFactor * CameraToPosDis * _DissolveThreshold))
				{
					//if (Dissolve < _DisolveFactor)
						discard;
				}

				return col;
			}
			ENDCG
		}
	}
}
