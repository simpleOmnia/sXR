Shader "HorizontalBlur"{
    Properties{_MainTex ("Texture", 2D) = "white" {}}
    
    SubShader{
	    Pass{
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
            float2 _MainTex_TexelSize;
	    	int	kernelSize;

			float4 frag(v2f i) : SV_Target{
				if(!kernelSize) kernelSize = 21;
				
				float3 col;

				const int max = ((kernelSize - 1) / 2);
				const int min = -max;

				for (int x = min; x <= max; ++x)
					col += tex2D(_MainTex, i.uv + float2(_MainTex_TexelSize.x * x, 0.0));

				return float4(col/kernelSize, 1.0);}
	    	
	    	ENDHLSL
		}
    }
}

