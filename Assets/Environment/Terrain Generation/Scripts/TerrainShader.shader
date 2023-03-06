Shader "Custom/TerrainShader" {
	Properties {
		_LayerTexture ("Layer Texture", 2D) = "white" {}
		_LayerMaxHeight ("Grass Max Height", Range(0,1)) = .5
		_LayerSlopeThreshold ("Grass Slope Threshold", Range(0,1)) = .5
        _LayerBlendAmount ("Grass Blend Amount", Range(0,1)) = .5
		_LayerHeightFadePower ("Grass Height Fade", Range(0,6)) = 1
		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
        CGPROGRAM
        #pragma surface frag Standard fullforwardshadows
        
        #pragma target 3.0
        
        half _GrassMaxHeight;
        half _MaxHeight;

        struct Input {
            float3 worldPos;
            float3 worldNormal;
            float3 viewDir;
        };

        void frag(Input IN, inout SurfaceOutputStandard o)
        {
	        float height = IN.worldPos.y;
        	float normalizedHeight = height/_MaxHeight;
            float grassGrowHeightPercent = smoothstep(0,_GrassMaxHeight,normalizedHeight);
        	
        }
        
		ENDCG
	}
	FallBack "Diffuse"
}