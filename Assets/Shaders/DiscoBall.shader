Shader "Custom/DiscoBall" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_EmissiveTex("Emisssive", 2D) = "black" {}   
		_EmisssiveMult("Intensity",Range(0,10)) = 0.0
		_ScrollSpeed("Scroll Speed",Range(0,5)) = 1      
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _EmissiveTex;

		struct Input 
		{
			float2 uv_EmissiveTex;
		};
        
        half _Emisssive;
		half _Glossiness;
		half _Metallic;
		half _EmisssiveMult;
		half _ScrollSpeed;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
		
		    fixed2 scrolledUV = IN.uv_EmissiveTex;
		    scrolledUV += frac(_Time.y * fixed2(_ScrollSpeed,0));
		     
			// Albedo comes from a texture tinted by color
			fixed4 c = _Color +  tex2D(_EmissiveTex, scrolledUV) * _EmisssiveMult;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
