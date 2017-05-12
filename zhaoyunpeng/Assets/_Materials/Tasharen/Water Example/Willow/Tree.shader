Shader "Game/Tree"
{
	Properties
	{
		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_Secondary ("Secondary Color", Color) = (1, 1, 1, 1)
		_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
		_Cutoff ("Base Alpha cutoff", Range (0,.9)) = .5
	}

 	SubShader
	{
		LOD 600
		Cull Off
		ZWrite On

		Tags
		{
			"Queue"="AlphaTest"

			// Soft edges, but this shader won't leave anything in the depth texture
			//"IgnoreProjector"="True"
			//"RenderType"="Transparent"
			
			// Rough edges but works properly with the depth texture
			"IgnoreProjector"="True"
			"RenderType"="TransparentCutout"
		}

		Pass
		{
			// We actually get softer edges if we do write to RGB
			//ColorMask A

			AlphaTest GEqual [_Cutoff]
			Blend SrcAlpha OneMinusSrcAlpha

			SetTexture [_MainTex]
			{
				constantColor [_Color]
				Combine texture * constant, texture * constant 
			}
		}
		
		CGPROGRAM
		#pragma surface surf Lambert alpha
		sampler2D _MainTex;
		half4 _Color;
		half4 _Secondary;

		struct Input
		{
			half4 color : COLOR;
			half2 uv_MainTex : TEXCOORD0;
		};

		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = lerp(tex.rgb, tex.rgb * _Color.rgb, IN.color.g + IN.color.r);
			o.Albedo = lerp(o.Albedo, tex.rgb * _Secondary.rgb, IN.color.r);
			o.Alpha = tex.a * _Color.a;
		}
		ENDCG
	}

	SubShader
	{
		LOD 200
		ZWrite On

		Tags
		{
			"Queue"="AlphaTest"
			"IgnoreProjector"="True"
			"RenderType"="TransparentCutout"
		}

		CGPROGRAM
		#pragma surface surf Lambert alphatest:_Cutoff
		sampler2D _MainTex;
		half4 _Color;
		half4 _Secondary;

		struct Input
		{
			half4 color : COLOR;
			half2 uv_MainTex : TEXCOORD0;
		};

		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = lerp(tex.rgb, tex.rgb * _Color.rgb, IN.color.g + IN.color.r);
			o.Albedo = lerp(o.Albedo, tex.rgb * _Secondary.rgb, IN.color.r);
			o.Alpha = tex.a * _Color.a;
		}
		ENDCG
	}
	Fallback "Transparent/Cutout/VertexLit"
}
