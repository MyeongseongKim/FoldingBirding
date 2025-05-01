Shader "Custom/ClippingShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }

    SubShader{
        Tags{ "RenderType"="Opaque"}
        LOD 200
        Cull Off

        CGPROGRAM

        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        fixed4 _Color;
        sampler2D _MainTex;
        half _Glossiness;
        half _Metallic;

        float4 _Plane;


        struct Input 
        {
            float2 uv_MainTex;
            float3 worldPos;
            float facing : VFACE;
        };

        void surf (Input IN, inout SurfaceOutputStandard o) 
        {
            float distance = dot(IN.worldPos, _Plane.xyz);
            distance = distance + _Plane.w;
            clip(-distance);

            float facing = IN.facing * 0.5 + 0.5;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
            o.Metallic = _Metallic * facing;
            o.Smoothness = _Glossiness * facing;
        }
        ENDCG
    }
    FallBack "Standard"
}