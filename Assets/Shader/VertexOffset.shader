Shader "Unlit/vertexdisplace"
{
	Properties
	{
        _ColorA("Color A", Color) = (1,1,1,1)
        _ColorB("Color B", Color) = (1,1,1,1)
        _ColorStart ("Color Start", Range(0,1)) = 1
        _ColorEnd("Color End ", Range(0,1)) = 0
		_WaveAmp("Wave Amplitude", Range(0,1)) = 0.1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Geometry" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			#define TAU 6.28

            float4 _ColorA;
            float4 _ColorB;
            float _ColorStart;
            float _ColorEnd;
			float _WaveAmp;

			struct MeshData
			{
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
				float3 normals : NORMAL; 
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 normal : TEXCOORD0;
				float2 uv : TEXCOORD1;
			};

			float GetWave(float2 uv)
			{
				float2 uvCenter = uv * 2 - 1;
				float radialDistance = length(uvCenter);
				float wave = cos((radialDistance - _Time.y * 0.1) * TAU * 5) * 0.5;
				return wave;
			}
			
			float InverseLerp(float a, float b, float v)
            {
                return(v - a) / (b - a);
            } 
			
			v2f vert (MeshData v)
			{
				v2f o;

				//float wave = cos((v.uv0.y - _Time.y * 0.1 ) * TAU * 5);
				//v.vertex.y = wave * _WaveAmp;

				v.vertex.y = GetWave(v.uv0) * _WaveAmp;
				
				o.uv = v.uv0;  //(v.uv0 + _Offset) * _Scale;
				o.normal = UnityObjectToWorldNormal( v.normals) ; 
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o; 
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
			    //float t = InverseLerp(_ColorStart, _ColorEnd, i.uv.x); 
			    //t = frac(t);
			    //float t = abs(frac(i.uv.x * 5) * 2 - 1);
				
				
			    //float wave = cos((i.uv.y - _Time.y * 0.1 ) * TAU * 5) * 0.5 + 0.5;
				return GetWave(i.uv);
			}
			ENDCG
		}
	}
}