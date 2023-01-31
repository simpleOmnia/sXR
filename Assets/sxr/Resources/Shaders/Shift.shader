Shader "Shift"{
    Properties{ _MainTex ("Texture", 2D) = "white" {} }
    
    SubShader{
        Pass{
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag#pragma fragmentoption ARB_precision_hint_fastest
            
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
            
            float gazeY;
            float gazeX;
            float aspectRatio; 
            
            float4 frag (v2f i) : SV_Target
            {
                sampler2D tex = _MainTex; 
                float _texelw = _MainTex_TexelSize.x;
                float2 uv = i.uv;
                uint sampleDistance = 15;

                int pixelsX = round((.5-gazeX)/_texelw);
                int pixelsY = round((.5-gazeY)/_texelw); 
                
                float shiftY = pixelsY * _texelw;
                float shiftX = pixelsX * _texelw;
                
                float2 newUV;
                newUV.x = uv.x + shiftX;
                newUV.y = uv.y + shiftY; 
                
                return tex2D(tex, newUV); 
            }
            ENDHLSL
        }
    }
}