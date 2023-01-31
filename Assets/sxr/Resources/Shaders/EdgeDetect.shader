Shader "EdgeDetect"
{
    Properties
    {
       [MainTexture] _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest

            #if SXR_USE_URP
            // Must include Unity's URP ShaderLibrary 
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #else
            #include "UnityCG.cginc"
            #endif

			struct appdata{ 
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;};

            struct v2f { 
                float4 vertex : SV_POSITION; 
                float2 uv : TEXCOORD0; };

            v2f vert (appdata v){ 
                v2f o;

            	#if SXR_USE_URP
                o.vertex = TransformObjectToHClip(v.vertex); // Clips objects based on ZTest property, converts to homogenous coordinates
                #else
                o.vertex = UnityObjectToClipPos(v.vertex); 
                #endif
            	
                o.uv = v.uv; 
                return o; }

            sampler2D _MainTex;
            
            float4 _MainTex_TexelSize;
            int invert; 

            float lum(float3 color) {
                return color.r*.3 + color.g*.59 + color.b*.11;
            }

            float sobel(sampler2D tex,  float2 uv){
                float _texelw = _MainTex_TexelSize.x;
                
                float3 Gx = tex2D(tex, float2(uv.x-_texelw, uv.y-_texelw)).rgb
                    + 2*tex2D(tex, float2(uv.x-_texelw, uv.y)).rgb
                    + tex2D(tex, float2(uv.x-_texelw, uv.y+_texelw)).rgb
                    + (-1)*tex2D(tex, float2(uv.x+_texelw, uv.y-_texelw)).rgb
                    + (-2)*tex2D(tex, float2(uv.x+_texelw, uv.y)).rgb
                    + (-1)*tex2D(tex, float2(uv.x+_texelw, uv.y+_texelw)).rgb;

                float3 Gy = tex2D(tex, float2(uv.x-_texelw, uv.y-_texelw) ).rgb
                    + 2*tex2D(tex, float2(uv.x, uv.y-_texelw)).rgb
                    + tex2D(tex, float2(uv.x+_texelw, uv.y-_texelw)).rgb
                    + (-1)*tex2D(tex, float2(uv.x-_texelw, uv.y+_texelw)).rgb
                    + (-2)*tex2D(tex, float2(uv.x, uv.y+_texelw)).rgb
                    + (-1)*tex2D(tex, float2(uv.x+_texelw, uv.y+_texelw)).rgb;

                float Gvx = max(max(max(Gx.r, Gx.g), Gx.b), lum(Gx));
                float Gvy = max(max(max(Gy.r, Gy.g), Gy.b), lum(Gy));
                float val = sqrt(Gvx*Gvx + Gvy*Gvy);

                if (invert==1)
                    return .75f-val;
                return val;
            }
            
            float4 frag (v2f i) : SV_Target
            {
                float lumCol = sobel(_MainTex, i.uv);
                return float4(lumCol, lumCol, lumCol, lumCol); ;
            }
            ENDHLSL
        }
        
    }
}
