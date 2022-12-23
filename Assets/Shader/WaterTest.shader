Shader "Unlit/WaterTest"
{
    Properties
    {
        _DepthGradientShallow("Shallow Color", Color) = (0,0,0.5,1)
        _DepthGradientDeep("Deep Color", Color) = (0,0,1,1)
        _DepthMaxDist("Depth Max Distance", Float) = 1
        
        _SurfaceTextureA("Surface TextureA", 2D) = "white" {}
        _SurfaceTextureB("Surface TextureB", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Transparent"}

        Pass
        {
            //Blend One One
            Blend SrcAlpha OneMinusSrcAlpha
            
            
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members UNITY_FOG_COORDS)
#pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag
            
            // make fog work
            #pragma multi_compile_fog
            
            #include "UnityCG.cginc"
            
            float4 _DepthGradientShallow;
            float4 _DepthGradientDeep;
            float _DepthMaxDist;
            sampler2D _CameraDepthTexture;

            sampler2D _SurfaceTextureA;
            float4 _SurfaceTextureA_ST;

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPosition : TEXCOORD1;
                
                UNITY_FOG_COORDS(1)
            };

            v2f vert (MeshData v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.uv = v.uv;
                o.screenPosition = ComputeScreenPos(o.vertex);

                o.uv = TRANSFORM_TEX(v.uv, _SurfaceTextureA);
                
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //calc depth
                float currDepth = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPosition)).r;
                float existingDepthLinear = LinearEyeDepth(currDepth);
                float depthDifference = existingDepthLinear - i.screenPosition.w;

                //calc color
                float4 waterDepthDifference = saturate(depthDifference / _DepthMaxDist);
                float4 waterColor = lerp(_DepthGradientShallow, _DepthGradientDeep, waterDepthDifference);

                float surfaceTexture = tex2D(_SurfaceTextureA, i.uv).r;
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return waterColor + surfaceTexture;
            }
            ENDCG
        }
    }
}
