Shader "Custom/SPscale"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _SPscale("SPscale", Range(0.0, 1.0)) = 0.0
    }
    SubShader 
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float _SPscale;

			fixed4 frag(v2f_img i) : COLOR
			{
				fixed4 currentText = tex2D(_MainTex, i.uv);

				
				float sepiaR = currentText.r * 0.393 + currentText.g * 0.769 + currentText.b * 0.189;
                float sepiaG = currentText.r * 0.349 + currentText.g * 0.686 + currentText.b * 0.168;
                float sepiaB = currentText.r * 0.272 + currentText.g * 0.534 + currentText.b * 0.131;

				// simple SPscale
				//float SPscale = (sepiaR + sepiaG + sepiaB) / 3;

				//YUV 
				//float SPscale = 0.299 * currentText.r + 0.587 * currentText.g + 0.114 * currentText.b;

				fixed4 SPscale = fixed4(sepiaR, sepiaG, sepiaB, _SPscale);
				fixed4 color = lerp(currentText, SPscale, _SPscale);

				currentText.rgb = color;
				
				return currentText;
			}
		
		ENDCG
		}
	}
	FallBack off
}