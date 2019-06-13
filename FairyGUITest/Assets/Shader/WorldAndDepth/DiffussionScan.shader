Shader "DepthTest/DiffussionScan"
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
				float3 uv : TEXCOORD0;   //z分量用来存随时间增加的距离
				float4 vertex : SV_POSITION;
				float4 ray : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _CameraDepthTexture;
			float4x4 _RayMatrix;
			float4 _ScanCenter;
			float _ScanSpeed;
			float _ScanWidth;
			fixed4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);

				//根据v.uv判断位置
				if (v.uv.x < 0.5 && v.uv.y > 0.5)
					o.ray = _RayMatrix[0];
				else if (v.uv.x < 0.5 && v.uv.y < 0.5)
					o.ray = _RayMatrix[1];
				else if (v.uv.x > 0.5 && v.uv.y > 0.5)
					o.ray = _RayMatrix[2];
				else if (v.uv.x > 0.5 && v.uv.y < 0.5)
					o.ray = _RayMatrix[3];

				//计算下随时间增加的距离
				o.uv.z = _Time.y * _ScanSpeed;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//重建世界坐标  pos = cameraPos + depth * ray;
				float depth = LinearEyeDepth( SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture , i.uv.xy));
				float3 worldPos = _WorldSpaceCameraPos.xyz + depth * i.ray.xyz;

				fixed4 col = tex2D(_MainTex, i.uv.xy);

				//判断当前点是否处于扫描圈内,distance判断圆环
				fixed disc = distance(worldPos.xyz, _ScanCenter.xyz);
				if (disc < i.uv.z && disc > i.uv.z - _ScanWidth)
				{
					col = _Color;
				}

				return col;
			}
			ENDCG
		}
	}
}
