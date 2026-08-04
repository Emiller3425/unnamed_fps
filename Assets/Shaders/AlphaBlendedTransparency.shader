Shader "Basics/AlphaBlendedTransparency" {
    Properties {
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Source Blend Mode", Integer) = 5
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Destination Blend Mode", Integer) = 10
    }
    SubShader {
        Tags {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Pass {
            Blend [_SrcBlend] [_DstBlend]
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            float4 _BaseColor;

            struct appdata {
                float4 positionOS : POSITION;
            };

            struct v2f {
                float4 positionCS : SV_POSITION;
            };

            v2f vert(appdata v) {
                v2f o = (v2f)0;

                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);

                return o;
            }

            float4 frag(v2f i) : SV_TARGET{
                return _BaseColor;
            }
            ENDHLSL
        }
    }
}