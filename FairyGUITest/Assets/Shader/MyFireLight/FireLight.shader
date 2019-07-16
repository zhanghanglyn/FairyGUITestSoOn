Shader "FireLight/FireLight"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "black" {}
		_BumpTex("BumpTex", 2D) = "white" {}
		_CircleRadius("CircleRadius" , Range(0,0.9)) = 0.3
		_BumpVal("BumpVal" , Range(0,1)) = 0.1	//调节BUMP范围
		_ReverseSpeedX("ReverseSpeedX" , Float) = 10
		_ReverseSpeedY("ReverseSpeedY" , Float) = 10
		_WaveX("WaveX" , Float) = 5
		_WaveY("WaveY" , Float) = 5
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType"="Transparent"}
		LOD 100

		Pass
		{
			//Blend SrcAlpha OneMinusSrcAlpha

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
				float4 screenPosNoW : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BumpTex;
			float4 _BumpTex_ST;

			fixed _BumpVal;
			fixed _CircleRadius;
			fixed _ReverseSpeedX;
			fixed _ReverseSpeedY;
			fixed _WaveX;
			fixed _WaveY;

			//需要以之为中点的坐标
			float3 FireScreenPos;

			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
				fixed2 BumpST_XY = fixed2(_BumpTex_ST.x - _WaveX, _BumpTex_ST.y - _WaveY);
				o.uv.zw = v.uv.xy * BumpST_XY + _BumpTex_ST.zw;
				//未除以W的屏幕坐标
				o.screenPosNoW = ComputeScreenPos(o.vertex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//判断屏幕坐标位置
				float2 screenPos = i.screenPosNoW.xy / i.screenPosNoW.w;
				//计算一个圆形区域
				float x = (screenPos.x - FireScreenPos.x);
				float y = (screenPos.y - FireScreenPos.y);
				float dis = sqrt(x * x + y * y);

				//获取一个bump颜色
				fixed2 bumpUV;
				bumpUV.x = i.uv.z + _Time.y / _ReverseSpeedX;
				bumpUV.y = i.uv.w + _Time.y/ _ReverseSpeedY;
				fixed bumpr = tex2D(_BumpTex , bumpUV).r;

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv.xy);
				col.a = 1;

				//if (distance(FireScreenPos.xy , screenPos) <= _CircleRadius)
				if (bumpr * _BumpVal > (distance(FireScreenPos.xy , screenPos) - _CircleRadius) )
				{
					col.rgb = fixed3(0.3,0.2,0.4);
					//col.a = min(distance(FireScreenPos.xy, screenPos),1 );
					
					discard;
				}

				return col;
			}
			ENDCG
		}
	}
}
