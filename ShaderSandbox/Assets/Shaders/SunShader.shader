Shader "Custom/SunShader" {
Properties {
    _MainTex ("Texture", 2D) = "white" { }
    _BumpMap ("BumpMap", 2D) = "white" { }
    _RimLightColor ("Rimlight Color", Color) = (1.0, 1.0, 1.0, 1.0)
    _MainColor ("Main Color", Color) = (0.95, 0.95, 0.0, 1.0)
}
SubShader {
    Pass {
 
    CGPROGRAM
    // Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members tangentSpaceLightDir)
    #pragma exclude_renderers d3d11 xbox360
    #pragma vertex vert
    #pragma fragment frag
     
    #include "UnityCG.cginc"
     
    sampler2D _MainTex;
    sampler2D _BumpMap;
    float4 _RimLightColor;
    float4 _MainColor;
   
    struct v2f {
        float4  pos : SV_POSITION;
        float2  uv : TEXCOORD0;
        float3  tangentSpaceLightDir;
    };
     
    float4 _MainTex_ST;
     
    v2f vert (appdata_tan v)
    {
        v2f o;
        o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
        o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
       
        float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
                     float3 binormal = cross( normalize(v.normal), normalize(v.tangent.xyz) );
                   float3x3 rotation = float3x3( v.tangent.xyz, binormal, v.normal );
                   
                   o.tangentSpaceLightDir = normalize(mul(rotation, viewDir));
           
       
        return o;
    }
 
 
    half4 frag (v2f i) : COLOR
    {
       half3 tangentSpaceNormal = normalize((tex2D(_BumpMap, i.uv).rgb * 2.0) - 1.0);
       
       half4 result = float4(0, 0, 0, 1);
 
       float rimFactor = dot(tangentSpaceNormal, i.tangentSpaceLightDir);
 
       result.rgb = (1-rimFactor) * _RimLightColor.rgb;
       result.rgb += tex2D(_MainTex, i.uv).rgb * _MainColor.rgb * rimFactor * 2   ;
 
       return result;
    }
 
 
ENDCG
 
    }
}
Fallback "Unlit"
}