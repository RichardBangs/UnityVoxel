Shader "Voxel/Default"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" "LightMode"="ForwardBase" }
		LOD 200
		
		Pass
		{
			CGPROGRAM
		    #pragma vertex vert
		    #pragma fragment frag
		    #include "UnityCG.cginc"

			sampler2D _MainTex;

	        struct appdata
	        {
    	        float4 vertex : POSITION;
        	    float4 uv : TEXCOORD0;
        	    
        	    float4 color : COLOR;
        	    float4 normal : NORMAL;
        	};

		    struct v2f
		    {
		        float4 pos : SV_POSITION;
		        float3 color : COLOR0;
		        
		        float2 uv : TEXCOORD0;
		    };

		    v2f vert (appdata v)
		    {
		        v2f o;
		        o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
		        
		        //	Lighting.
		        float lightValue = UNITY_LIGHTMODEL_AMBIENT.x;		//	Ambient.
		        lightValue += max( 0.0f, dot( v.normal, normalize( WorldSpaceLightDir( v.vertex ) ) ) );	//	Diffuse.
		        
		        //	Apply Final Colour (combination of v.color - precalculated Ambient Occulsion lighting, and our calculated diffuse above).
		        o.color = v.color * lightValue;
		        
		        o.uv = v.uv;
		        
		        return o;
		    }

		    half4 frag (v2f i) : COLOR
		    {
		    	float4 tex = tex2D( _MainTex, i.uv );
		    	
		        return half4(i.color * tex, 1);
		    }
			ENDCG
		}
	}
	FallBack "Diffuse"
}