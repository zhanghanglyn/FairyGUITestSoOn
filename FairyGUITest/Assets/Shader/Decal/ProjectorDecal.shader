/*
渲染投影仪时，Unity会设置两个额外的矩阵，_Projector和_ProjectorClip。
_Projector将投影仪剪辑空间的x和y轴映射到u和v坐标，这些坐标通常用于采样径向衰减纹理。
_ProjectorClip将投影仪视图空间的z轴映射到au坐标（可能在v中复制），该坐标可用于采样斜坡纹理，该纹理定义投影仪随距离的衰减。投影机近平面的u值为0，投影机远平面的值为1。

编辑：这些变换将堆叠在模型和投影仪视图矩阵的顶部，因此您只需将它们乘以对象空间顶点位置即可获得正确的结果。
*/

Shader "Decal/ProjectorDecal"
{
	Properties
	{
		_Cookie ("_CookieTexture", 2D) = "white" {}
		_FallOff("_FallOff" , 2D) = "white" {}
	}
	SubShader
	{
		Tags { "Queue"="Transparent" }
		LOD 100

		Pass
		{
			ZWrite Off
			ColorMask RGB
			//Blend DstColor Zero  //阴影等不透明用
			Blend SrcAlpha OneMinusSrcAlpha
			Offset -1, -1

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 cookieUV : TEXCOORD0;
				float4 falloffUV : TEXCOORD1;
				float4 pos : SV_POSITION;
			};

			sampler2D _Cookie;
			float4 _Cookie_ST;
			sampler2D _FallOff;
			float4 _FallOff_ST;
			float4x4 unity_Projector;
			float4x4 unity_ProjectorClip;
			
			v2f vert (float4 vertex : POSITION)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(vertex);
				o.cookieUV = mul(unity_Projector, vertex);	//用于采样镜像衰减纹理
				o.falloffUV = mul(unity_ProjectorClip, vertex);	//该纹理定义投影仪随距离的衰减

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				
				fixed4 decal_color = tex2Dproj(_Cookie , UNITY_PROJ_COORD(i.cookieUV));
				//decal_color.a = 1.0 - decal_color.a;

				fixed4 fallOff = tex2Dproj(_FallOff, UNITY_PROJ_COORD(i.falloffUV));

				//目的是为了让默认颜色变为白色！！
				fixed4 outcolor = lerp(fixed4(1, 1, 1, 0), decal_color, fallOff.a);

				return outcolor;
			}
			ENDCG
		}
	}
}
