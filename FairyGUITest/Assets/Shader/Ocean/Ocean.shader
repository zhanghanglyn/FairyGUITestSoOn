Shader "Ocean/Ocean"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_WaveTex("WaveTex", 2D) = "white" {}
		_DepthThresholdValue("_DepthThresholdValue" , Range(0,1)) = 0.1
		_InvFade("_InvFade",Range(0.01,3.0)) = 0.1
		_Wave2Tex("Wave2Tex" , 2D) = "white" {}
		_Speed( "Speed", Float ) = 0.1
		_WaveBump("WaveBump" , 2D) = "white" {}		//给波浪增加扭曲UV的效果BUMP图
		_WaveDistinct("WaveDistinct" , Range(0.5,2)) = 1.2  //波浪靠近岸边的清晰程度
		_WaveWarp("_WaveWarp" ,Range(1,5)) = 1.2 //海浪的扭曲程度，越大越小
		_AlphaLight("_AlphaLight" , Range(1,2)) = 1.5
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 100

		Blend SrcAlpha OneMinusSrcAlpha  //效果会完全不同
		//Blend One OneMinusSrcColor

		//Blend DstColor Zero

		ZWrite Off
		Cull Off
		ColorMask RGB
		Lighting Off

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
				float4 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float4 vertex : SV_POSITION;

				float4 projectionPos:TEXCOORD2;
			};

			sampler2D _CameraDepthTexture;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _WaveTex;
			float4 _WaveTex_ST;

			sampler2D _Wave2Tex;
			float4 _Wave2Tex_ST;

			sampler2D _WaveBump;
			float4 _WaveBump_ST;

			fixed _DepthThresholdValue;
			fixed _InvFade;

			fixed _Speed;
			fixed _WaveDistinct;
			fixed _WaveWarp;
			fixed _AlphaLight;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv.zw = v.uv * _Wave2Tex_ST.xy + _Wave2Tex_ST.zw;

				o.uv2.xy = TRANSFORM_TEX(v.uv , _WaveBump);

				o.projectionPos = ComputeScreenPos(o.vertex);
				//这个函数相当于把v.vertex的点去转换到view空间，并且将-z值返回给传入的参数
				COMPUTE_EYEDEPTH(o.projectionPos.z);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//计算投影深度，处理水面颜色使在离岸进的地方颜色浅透明度高，反之
				float depth = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture , UNITY_PROJ_COORD(i.projectionPos));
				depth = LinearEyeDepth(depth);

				float diffDepth = saturate(_InvFade * (depth - i.projectionPos.z));

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv.xy);
				//col.rgb *= diffDepth;   //这个不乘，比较有卡通的海水效果
				col.a *= diffDepth;

				//计算冲刷的波浪相关  用sin或者cos函数来模拟海浪冲刷上去又返回海里的情况
				//因为diffDepth是在0-1之间，所以可以用来限制海浪的方向
				fixed uvDepth = diffDepth + sin(_Time.y * _Speed);
				fixed uvDepth2 = diffDepth + sin(_Time.x * 6 * _Speed);
				//再用一张bump贴图来设置uv
				fixed4 bump = tex2D(_WaveBump, i.uv2.xy);

				//叠加一个海浪波纹
				fixed4 col2 = tex2D(_WaveTex, float2(i.uv.y + uvDepth + bump.r / _WaveWarp, i.uv.x + uvDepth + bump.r / _WaveWarp));
				//col2.rgb *= diffDepth;
				col2.a *= diffDepth;
				//col.rgb = lerp( col2.rgb , col.rgb , diffDepth);
				//col.a = lerp(col2.a, col.a, diffDepth);
				//尝试新海浪波纹叠加
				col.rgb += (col.rgb * col2.a + col2.rgb * (1 - diffDepth)) * (1 - diffDepth);

				//将海浪波纹叠加
				fixed4 textRgb = tex2D(_Wave2Tex, float2(i.uv.z + uvDepth + bump.x / _WaveWarp, i.uv.w + uvDepth + bump.y/ _WaveWarp));
				col.rgb += (textRgb.rgb * textRgb.a / 2 * ((1 - diffDepth)*_WaveDistinct) );
				col.a = col.a * _AlphaLight;
				//再叠加一个海浪
				fixed4 textRgb2 = tex2D(_Wave2Tex, float2( (i.uv.z + uvDepth2 + bump.y / _WaveWarp) * 2,
					(i.uv.w + uvDepth2 + bump.r / _WaveWarp))*2 );
				col.rgb += (textRgb2.rgb * textRgb2.a / 2 * ((1 - diffDepth)*_WaveDistinct)) / 2;


				return col;
			}
			ENDCG
		}
	}
}
