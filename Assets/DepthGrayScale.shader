// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/DepthGrayscale" {
	SubShader{
		Tags{ "RenderType" = "Opaque" }

		Pass{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma enable_d3d11_debug_symbols;
#include "UnityCG.cginc"

	sampler2D _CameraDepthTexture;

	struct v2f {
		float4 pos : SV_POSITION;
		//float3 wpos:  SV_POSITION;
		float4 scrPos:TEXCOORD1;
	};

	uniform int _Points_Length = 65536;
	uniform int _bla = 1;
	uniform int y = 1;
	float3 _Points[65536];
	uniform float2 _Properties[20];

	int _Point[9];

	//Vertex Shader
	v2f vert(appdata_base v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		//o.wpos = mul(unity_ObjectToWorld, v.vertex).xyz;
		o.scrPos = ComputeScreenPos(o.pos);
		//for some reason, the y position of the depth texture comes out inverted
		//o.scrPos.y = 1 - o.scrPos.y;
		return o;
	}

	//Fragment Shader
	half4 frag(v2f i) : COLOR{
		float depthValue = Linear01Depth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r);
	half4 depth;

	depth.r = depthValue;
	depth.g = depthValue;
	depth.b = depthValue;

	_bla = 1;// i.scrPos.y + 256 * i.scrPos.x;

	_Points[0] = float4(0, 1, 2, 1);
	_Point[1] = 3;


	depth.a = 1;
	return depth;



	}
		ENDCG
	}
	}
		FallBack "Diffuse"
}