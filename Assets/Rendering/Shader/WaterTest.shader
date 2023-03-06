// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/WaterTest"
{
    Properties
    {
        _DepthGradientShallow("Shallow Color", Color) = (0,0,0.5,1)
        _DepthGradientDeep("Deep Color", Color) = (0,0,1,1)
        _DepthMaxDist("Depth Max Distance", Float) = 1
        _FoamDistance("Foam Distance", Float) = 0.4
        _FoamColor("Foam Distance", Color) = (1,1,1,1)

//        _SurfaceTextureA("Surface TextureA", 2D) = "white" {}
//        _SurfaceTextureB("Surface TextureB", 2D) = "white" {}
        
        _SurfaceNoise("Surface Noise", 2D) = "white" {}
        _SurfaceDiffuse("Surface Noise Diffuse", 2D) = "white" {}
        _SurfaceDiffuseAmount("Surface Diffuse Amount", Range(0,1)) = 0.5
        _SurfaceSpeed("Surface Speed", Vector) = (0.3, 0.3, 0, 0)
        _SurfaceCutoff("Surface Cutoff", Range(0,1)) = 0.5
        
        //surface displace
//        _WaveTexture("Waves", 2D) = "white"{}
        
        _Smoothness("Smoothness", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Transparent"}

        Pass
        {
            //Blend One One
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            // make fog work
            //#pragma multi_compile_fog
            
            #include "UnityCG.cginc"
            
            float4 _DepthGradientShallow;
            float4 _DepthGradientDeep;
            float _DepthMaxDist;
            sampler2D _CameraDepthTexture;

            sampler2D _SurfaceTextureA;
            float4 _SurfaceTextureA_ST;

            //surface Color
            sampler2D _SurfaceNoise;
            float4 _SurfaceNoise_ST;

            //surface texture
            sampler2D _SurfaceDiffuse;
            float4 _SurfaceDiffuse_ST;
            float _SurfaceDiffuseAmount;

            float _FoamDistance;
            
            float _SurfaceCutoff;
            float2 _SurfaceSpeed;

            //surface color
            float4 _FoamColor;

            //surface displace
            // sampler2D _WaveTexture;
            // float4 _WaveTexture_ST;

            float _Smoothness;

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 screenPosition : TEXCOORD1;
                float2 noiseUV : TEXCOORD2;
                float2 distortUV : TEXCOORD3;
                half3 WorldRefl : TEXCOORD4;
                float3 normal: NORMAL;
                
                UNITY_FOG_COORDS(1)
            };
            

            v2f vert (MeshData v)
            {
                v2f o;
                
                //Depth
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPosition = ComputeScreenPos(o.vertex);
                o.normal = v.normal;
                

                o.noiseUV = TRANSFORM_TEX(v.uv, _SurfaceNoise);
                o.distortUV = TRANSFORM_TEX(v.uv, _SurfaceDiffuse);

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
                float3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
                float3 worldNormal = UnityObjectToWorldNormal(o.normal);
                o.WorldRefl = reflect(-worldViewDir, worldNormal);
                
                
                // float wavesMap = TRANSFORM_TEX(v.uv, _WaveTexture);
                // o.vertex.y += sin(_Time + wavesMap) * 0.8;
                // o.vertex.x += sin(_Time + wavesMap) * 0.8;

                 //v.vertex.y = o.waves + _Time.y;
                
                
                
                //UNITY_TRANSFER_FOG(o,o.vertex);
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

                //Surface Texture
                //float surfaceTexture = tex2D(_SurfaceTextureA, i.uv).r;
                //return waterColor + surfaceTexture;

                //Surface Noise Distort
                float2 noiseUVDistort = (tex2D(_SurfaceDiffuse, i.distortUV).xy * 2 - 1) * _SurfaceDiffuseAmount;
                //calculate surface noise
                float2 noiseUVMotion = float2(i.noiseUV.x + (_Time.y * _SurfaceSpeed.x) + noiseUVDistort.x, i.noiseUV.y + (_Time.y * _SurfaceSpeed.y) + noiseUVDistort.y);
                //assign surface noise
                float surfaceNoiseSample = tex2D(_SurfaceNoise, noiseUVMotion).x;

                //foam
                float foamDepthDifference = saturate(depthDifference / _FoamDistance);
                
                //cutoff
                float noiseCutoff = _SurfaceCutoff * foamDepthDifference;
                //float surfaceNoise = surfaceNoiseSample > noiseCutoff ? 2 : 0;
                float surfaceNoise = smoothstep(noiseCutoff - 0.00001, noiseCutoff + 0.00001, surfaceNoiseSample);

                //noise color
                float surfaceNoiseColor = surfaceNoise * _FoamColor;
                
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);

                float4 skyData = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, i.WorldRefl);
                // decode cubemap data into actual color
                float3 skyColor = DecodeHDR (skyData, unity_SpecCube0_HDR);
                // output it!
                fixed4 c = 0;
                c.rgb = skyColor * _Smoothness * surfaceNoiseSample;
                
                
                return waterColor + surfaceNoiseColor + c;
            }
            ENDCG
        }
    }
}
