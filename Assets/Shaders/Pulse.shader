Shader "Basics/Pulse" {
    Properties {
        _BaseColor("Base Color", Color) = (0.8, 0, 0, 1)
        _PulseSpeed("Pulse Speed", Float) = 2.0
        _PulseIntensity("Pulse Intensity", Range(0,1)) = 0.2
    }
    SubShader {
        Tags {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
        }

        Pass {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _PulseSpeed;
                float _PulseIntensity;
            CBUFFER_END

            struct appdata {
                float4 positionOS : POSITION;
            };

            struct v2f {
                float4 positionCS : SV_POSITION;
            };

            v2f vert(appdata v) {
                v2f o = (v2f)0;

                o.positionCS = TransformObjectToHClip(v.positionOS.xyzw);

                return o;
            }

            float4 frag(v2f i) : SV_TARGET{
                float pulse = sin(_Time.y * _PulseSpeed);

                pulse = (pulse * 0.5 + 0.5) * _PulseIntensity;

                float4 finalColor = _BaseColor;
                finalColor.rgb += pulse;

                return finalColor;
            }

            ENDHLSL
        }
    }
}