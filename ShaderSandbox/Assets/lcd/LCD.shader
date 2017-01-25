
//"Fade between two textures at X distance" 


// Alan Zucconi: http://www.alanzucconi.com/?p=4707
Shader "Alan Zucconi/LCD" {
	Properties {
		//_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		_LCDTex("LCD (RGB)", 2D) = "white" {}
		_Pixels ("Pixels", Vector) = (1,1,0,0)
		_LCDPixels("LCD pixels", Vector) = (3,3,0,0)

		_DistanceOne("Distance of full effect", Float) = 1 // In metres
		_DistanceZero ("Distance of zero effect", Float) = 2 // In metres
		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert
		#pragma multi_compile DUMMY PIXELSNAP_ON

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		//fixed4 _Color;

		sampler2D _LCDTex;
		int2 _Pixels;
		int2 _LCDPixels;

		float _DistanceOne;
		float _DistanceZero;


		
		
		void vert(inout appdata_full v, out Input o)
		{
			//#if defined(PIXELSNAP_ON) && !defined(SHADER_API_FLASH)
			v.vertex = UnityPixelSnap(v.vertex);
			//#endif

			UNITY_INITIALIZE_OUTPUT(Input, o);
			
			// float dist = distance(_WorldSpaceCameraPos, mul(Object2World, v.vertex));
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			
			// Pixelated effect 
			//float2 uv = round((IN.uv_MainTex + (1. / _Pixels.xy) / 2) * _Pixels.xy) / _Pixels.xy
			float2 uv = round(IN.uv_MainTex * _Pixels.xy + 0.5) / _Pixels.xy;
			fixed4 a = tex2D(_MainTex, uv);

			// LCD cells
			float2 uv_lcd = IN.uv_MainTex * _Pixels.xy / _LCDPixels;
			fixed4 d = tex2D(_LCDTex, uv_lcd);

			

			float dist = distance(_WorldSpaceCameraPos, IN.worldPos); // In metres
			float alpha = saturate
				(
					(dist - _DistanceOne) / (_DistanceZero-_DistanceOne)
				);	// [_DistanceOne, _DistanceZero] > [0, 1]

			// Mixing
			o.Albedo = lerp(a * d, a, alpha);
			
			
			// Non pixelated version
			//fixed4 e = tex2D(_MainTex, IN.uv_MainTex);
			//o.Albedo = lerp(a * d, e, alpha);
	

			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			//o.Alpha = c.a;
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
