Shader "Color Effect" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	SubShader 
	{
		Pass 
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
				
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest 
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			float3 _Color;
			float _Multiplier;
			float _Amount;
			float _ColorAmount; 

			fixed4 frag (v2f_img i) : COLOR
			{
				float4 output = 0;
				float4 original = tex2D(_MainTex, i.uv);

				float3 colored = original.rgb * _Color * _Multiplier;
				output.rgb = lerp(original.rgb, colored, _Amount);
				output.rgb = lerp(output.rgb, _Color, _ColorAmount);
				output.a = original.a;

				return output;
			}

			ENDCG
		}
	}

	Fallback off
}