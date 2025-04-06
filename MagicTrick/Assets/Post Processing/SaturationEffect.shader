Shader "Custom/ColorGradingEffect"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}

        // Color Grading Properties
        _Lift ("Lift (Shadows)", Color) = (1, 1, 1, 1)
        _Gamma ("Gamma (Midtones)", Color) = (1, 1, 1, 1)
        _Gain ("Gain (Highlights)", Color) = (1, 1, 1, 1)

        // Grain Properties
        _GrainIntensity ("Grain Intensity", Range(0, 1)) = 0.2
        _GrainScale ("Grain Scale", Range(1, 10)) = 2.0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _Lift;  // Shadows color adjustment
            float4 _Gamma; // Midtones color adjustment
            float4 _Gain;  // Highlights color adjustment

            float _GrainIntensity; // Grain amount
            float _GrainScale;     // Grain scaling factor

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture
                fixed4 color = tex2D(_MainTex, i.uv);

                // Apply color grading
                color.rgb = (color.rgb - _Lift.rgb) / (_Gamma.rgb) * _Gain.rgb;

                // Add grain
                float2 grainCoord = i.uv * _GrainScale;
                float noise = frac(sin(dot(grainCoord, float2(12.9898, 78.233))) * 43758.5453);
                color.rgb += noise * _GrainIntensity;

                return color;
            }
            ENDCG
        }
    }
}
