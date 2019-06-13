Shader "Decal/CameraPorjector"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" }
		LOD 100

		Pass
		{
			ZWrite Off
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			Offset -1, -1

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 decalUV : TEXCOORD0;
				float4 pos : SV_POSITION;
			};

			sampler2D _MainTex;
			//外部计算出的由世界矩阵到摄像机投影矩阵的矩阵
			float4x4 worldToCameraProjectionMatrix;
			
			v2f vert (float4 vertex : POSITION)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(vertex);
				//创建一个由模型坐标系到摄像机投影坐标系的矩阵，从右往左计算效果！！！
				float4x4 objToCameraProjection = mul(worldToCameraProjectionMatrix, unity_ObjectToWorld);

				float4 decalProjectSpacePos = mul(objToCameraProjection, vertex);

				o.decalUV = ComputeScreenPos(decalProjectSpacePos);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2Dproj(_MainTex, UNITY_PROJ_COORD(i.decalUV));
				return col;
			}
			ENDCG
		}
	}
}
