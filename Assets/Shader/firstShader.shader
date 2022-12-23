Shader "Unlit/firstShader"
{
	Properties
	{
        _ColorA("Color A", Color) = (1,1,1,1)
        _ColorB("Color B", Color) = (1,1,1,1)
        _ColorStart ("Color Start", Range(0,1)) = 1
        _ColorEnd("Color End ", Range(0,1)) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }

		Pass
		{
		    Cull off
		    ZWrite off
		    //ZTest GEqual
		    Blend One One //additive
		    //Blend DstColor Zero //multiply
		    
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			#define TAU 6.28

            float4 _ColorA;
            float4 _ColorB;
            float _ColorStart;
            float _ColorEnd;

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
			
			float InverseLerp(float a, float b, float v)
            {
                return(v - a) / (b - a);
            } 
			
			v2f vert (MeshData v)
			{
				v2f o;
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
			    
			    float xOffset = cos(i.uv.x * 8 * TAU) * 0.02;
			    float t = cos((i.uv.y + xOffset - _Time.y * 0.1 ) * TAU * 5) * 0.5 + 0.5;
			    t *= 1 - i.uv.y ;
			    
			    float topRemove = (abs(i.normal.y) < 1);
			    float waves = t * topRemove;
			    
			    float4 gradient = lerp(_ColorA, _ColorB, i.uv.y); 
			    return gradient * waves;
			    return waves;
			}
			ENDCG
		}
	}
}