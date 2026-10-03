Shader "Basics/BloodParticle" {
    Properties {
        _BloodColor("Blood Color", Color) = (0.8, 0, 0, 1)
        _MainTex("Blood Texture", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // declare texture and sampler
            Texture2D _MainTex;
            SamplerState sampler_MainTex;

            CBUFFER_START(UnityPerMaterial)
                float4 _BloodColor;
                float4 _MainTex_ST;
            CBUFFER_END

            struct appdata {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
                float4 color      : COLOR;
            };

            struct v2f {
                float4 positionCS : SV_POSITION;
                float2 uv         : TEXCOORD0;
                float4 color      : COLOR;
            };

            v2f vert(appdata v) {
                v2f o = (v2f)0;

                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); // Pass and scale
                o.color = v.color;
                return o;
            }

            float4 frag(v2f i) : SV_TARGET {
                float4 texColor = _MainTex.Sample(sampler_MainTex, i.uv);

                float4 finalColor = texColor * _BloodColor * i.color;

                return finalColor;
            }


            ENDHLSL
        }
    }
}