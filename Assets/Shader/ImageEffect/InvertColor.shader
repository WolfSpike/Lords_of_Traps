﻿Shader "Custom/InvertColor"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("_Color", color) = (1,1,1,1)
		_Revert ("Revert", Range(0,1)) = 1
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float4 _Color;
			float _Revert;


			float4 frag (v2f i) : SV_Target
			{
				float4 col;

				col = tex2D(_MainTex, i.uv);
				col *= _Color;
				col.rgb = abs(_Revert - col.rgb);
			    return col;
			}
			ENDCG
		}
	}
}
