Shader "UV_Sample"{
    Properties{ [MainTexture] _MainTex ("Texture", 2D) = "white" {} } // Texture that can be passed in to shader material or the texture the material is applied to  
    
    SubShader{ // Renderer chooses first SubShader compatible with GPU and target device
        Cull Off ZWrite Off ZTest Always
        // Cull - Back, Front, Off => Removes objects that are on the back side of objects, front removes front side, off disables
        // ZWrite - On, Off => Defines if a depth buffer is used
        // ZTest - Less, Greater, LEqual, GEqual, Equal, NotEqual, Always => Defines the type of depth comparison to use

        Pass{
            HLSLPROGRAM // Start high level shader language

            #pragma vertex vert // Vertex shader - maps object's polygon vertices and textures to homogenous 3D => (-1 < x,y < 1) 
            #pragma fragment frag // Fragment (pixel) shader 
            #pragma fragmentoption ARB_precision_hint_fastest // Optimized when fine precisions aren't required

            #if SXR_USE_URP
            // Must include Unity's URP ShaderLibrary 
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #else
            #include "UnityCG.cginc"
            #endif
            
            struct appdata{ // define the appdata struct to include worldspace POSITION and the texture TEXCOORD0coordinates
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;};

            struct v2f { // defines struct for transformed world space => screen space positions
                float4 vertex : SV_POSITION; // System-Value (screen space) positions
                float2 uv : TEXCOORD0; };

            v2f vert (appdata v){ // Takes in world space coordinates, outputs homogenous coordinates 
                v2f o;
                
                #if SXR_USE_URP
                o.vertex = TransformObjectToHClip(v.vertex); // Clips objects based on ZTest property, converts to homogenous coordinates
                #else
                o.vertex = UnityObjectToClipPos(v.vertex); 
                #endif
                
                o.uv = v.uv; // Uses the unmodified texture coordinates, use TRANSFORM_TEX(v.uv, _MainTex) if using TEXTURE2D/SAMPLER
                return o; }

            sampler2D _MainTex; // Define _MainTex to be a sampler2D (samples from TEXCOORD0)

            /* If using tiling/offset for applying texture to objects:
              
            TEXTURE2D(_MainTex); // Define _MainTex as TEXTURE2D
            SAMPLER(sampler_MainTex); // Creates a sampler for _MainTex

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;      // required if using tiling/offset
            CBUFFER_END
            */
            
            float4 _MainTex_TexelSize; // Returns texel size if needing to access other uv positions in fragment shader (not needed here but useful to know)

            float4 frag (v2f i) : SV_Target { // Takes v2f-formatted output pixel-by-pixel from the 'vert' function as input, outputs to screenspace target (targetTexture for a camera)
                float4 unmodified_current_pixel = tex2D(_MainTex, i.uv); // samples _MainTex at the currently processed pixel, use SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv) for TEXTURE2D/SAMPLER
                float4 one_pixel_above = tex2D(_MainTex, float2(i.uv.x, i.uv.y+_MainTex_TexelSize.y)); // unused but this is how to get nearby pixels
                return float4(i.uv.x, i.uv.y, unmodified_current_pixel.b, 1.0); } // outputs red and green values based on screen position, keeping the original textures blue value
            
            ENDHLSL
        }
    }
}
