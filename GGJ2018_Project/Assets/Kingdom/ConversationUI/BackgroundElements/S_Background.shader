// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Kingdom/BackgroundShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}

		_GradientMap("GradientMap", 2D) = "white" {}

		_Color("Tint", Color) = (1,1,1,1)
			_GradientTime("GradientTime", Range(0, 1)) = 0

		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		_LineColor("LineColor", Color) = (0, 0, 0, 1)
			_LightColor("LightColor", Color) = (1, 1, 1, 1)
			_LightIntensity("LightIntensity", Float) = 1

			_NoiseMap("NoiseMap", 2D) = "white" {}
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog{ Mode Off }
		Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile DUMMY PIXELSNAP_ON
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				half2 texcoord  : TEXCOORD0;
			};

			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif

				return OUT;
			}

		sampler2D _MainTex;
		sampler2D _GradientMap;
		sampler2D _NoiseMap;
		float4 _LineColor;
		float4 _LightColor;
		float _LightIntensity;
		float _GradientTime;

		fixed4 frag(v2f IN) : SV_Target
		{
			fixed4 c = tex2D(_MainTex, IN.texcoord);
			
			fixed4 backgroundColor = tex2D(_GradientMap, half2(_GradientTime, c.r));

			c.rgb = lerp(backgroundColor.rgb * c.a, (c.g * _LineColor.rgb), c.g) 
				
				+ c.b *_LightColor * _LightIntensity * pow(tex2D(_NoiseMap, half2(sin(_Time.x), cos(_Time.x)*3)* IN.texcoord), 1.4);
		
			c.rgb = lerp(c.rgb, _LineColor, 1- c.a);
			
			c.rgb *= IN.color;

			c.rgb *= c.a;

			return c;
			}
			ENDCG
		}
	}
}