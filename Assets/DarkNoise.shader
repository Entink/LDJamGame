Shader "Custom/DarkNoiseFullscreen"
{
    Properties
    {
        _NoiseIntensity ("Noise Intensity", Range(0, 0.2)) = 0.04
        _NoiseScale ("Noise Scale", Float) = 250
        _DarkThreshold ("Dark Threshold", Range(0, 1)) = 0.85
        _DarkSoftness ("Dark Softness", Range(0.001, 1)) = 0.1
        _TimeScale ("Time Scale", Float) = 2
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" }

        Pass
        {
            Name "DarkNoiseFullscreen"

            ZWrite Off
            Cull Off
            Blend One Zero

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            float _NoiseIntensity;
            float _NoiseScale;
            float _DarkThreshold;
            float _DarkSoftness;
            float _TimeScale;

            float Random(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float2 uv = input.texcoord;
                half4 sceneColor = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv);

                float luminance = dot(sceneColor.rgb, float3(0.299, 0.587, 0.114));
                float darkness = 1.0 - luminance;

                float darkMask = smoothstep(_DarkThreshold - _DarkSoftness, _DarkThreshold, darkness);

                float2 noiseUV = uv * _NoiseScale;
                noiseUV += _Time.y * _TimeScale;

                float noise = Random(noiseUV);
                float noiseAmount = noise * _NoiseIntensity * darkMask;

                sceneColor.rgb += noiseAmount;

                return sceneColor;
            }
            ENDHLSL
        }
    }
}