// unlit, vertex colour, alpha blended
// cull off

Shader "Inhuman/Background"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_ForeColor("Foreground Color", Color) = (1, 1, 0, 1)
		_BackColor("Background Color", Color) = (0, 1, 1, 1)
		_CloudColor("Cloud Color", Color) = (0.5, 0.5, 0.5, 1)
		_CloudRimColor("Cloud Rim Color", Color) = (1, 1, 1, 1)
		_CloudAmount("Cloud Amount", Float) = 1
		_CloudShift("Cloud Shift", Float) = 0

		_ParallaxSpeed("Parallax Speed", Float) = 2
		_ParallaxAmount("Parallax Amount", Float) = 0.2
	}

	SubShader
	{
		Tags{ "Queue" = "Background" "RenderType" = "Opaque" }
		ZWrite Off 
		Lighting Off 
		Fog{ Mode Off } 
		LOD 110

		Pass
		{
			CGPROGRAM
			#pragma vertex VShader
			#pragma fragment PShader 
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _ForeColor;
			float4 _BackColor;
			float4 _CloudColor;
			float4 _CloudRimColor;
			float _CloudAmount;
			float _CloudShift;

			float _ParallaxSpeed;
			float _ParallaxAmount;

			//=========================================================================================//
			struct VertexInput
			{
				float4 Pos : POSITION;
				float4 Color : COLOR;
				float2 Uv : TEXCOORD0;
			};

			struct VertexOutput
			{
				float4 Pos : POSITION;
				fixed4 Color : COLOR;
				float2 Uv : TEXCOORD0;
			};

			//=========================================================================================//
			VertexOutput VShader(VertexInput IN)
			{
				VertexOutput OUT;
				OUT.Pos = mul(UNITY_MATRIX_MVP, IN.Pos);
				OUT.Color = IN.Color;
				OUT.Uv = IN.Uv;

				return OUT;
			}

			//=========================================================================================//
			fixed4 PShader(VertexOutput IN) : COLOR
			{
				fixed4 OUT = tex2D(_MainTex, IN.Uv) * IN.Color;

				float2 uv_offset = float2(0, _ParallaxSpeed * _Time.a);
				float2 parallax_offset = float2(0, _ParallaxAmount * _Time.a);

				fixed4 alpha = tex2D(_MainTex, IN.Uv + uv_offset + parallax_offset * 2);
				fixed4 top = tex2D(_MainTex, IN.Uv + uv_offset + parallax_offset * 1);
				fixed4 middle = tex2D(_MainTex, IN.Uv + uv_offset + parallax_offset * 0.66);
				fixed4 bottom = tex2D(_MainTex, IN.Uv + uv_offset + parallax_offset * 0.33);
				


				float bottom_blend = lerp(0, 0.5, bottom.b); 
				float3 buffer = lerp(_BackColor, lerp(_BackColor, _ForeColor, bottom_blend), saturate(bottom.b * 100));

				float middle_blend = lerp(0.25, 0.75, middle.g); 
				buffer = lerp(buffer, lerp(_BackColor, _ForeColor, middle_blend), saturate(middle.g * 100));

				float top_blend = lerp(0.5, 1, top.r);
				buffer = lerp(buffer, lerp(_BackColor, _ForeColor, top_blend), saturate(top.r * 100));
				
				buffer += _CloudColor * (alpha.a + _CloudShift) * _CloudAmount;

				//float cloud_blend = saturate(alpha.a * _CloudAmount + _CloudShift);
				//buffer = lerp(buffer, lerp(_CloudRimColor, _CloudColor, cloud_blend), cloud_blend);

				OUT.rgb = buffer;

				return OUT;
			}

			ENDCG
		}
	}

	SubShader
	{
		Tags{ "Queue" = "Background" "RenderType" = "Opaque" }
		ZWrite Off
		LOD 100

		BindChannels
		{
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
			Bind "Color", color
		}

		Pass
		{
			Lighting Off
			SetTexture[_MainTex] { combine texture * primary }
		}
	}
}
