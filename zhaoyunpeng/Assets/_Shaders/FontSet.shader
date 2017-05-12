Shader "myUGUI/myOutLine" {  
    Properties {  
        _MainTex ("Font Texture", 2D) = "white" {}  
        _Color ("Text Color", Color) = (1,1,1,1)  
        _OutLineWidth ("_OutLineWidth", float) = 0.01  
        _BlurPower("_BlurPower", float) = 5  
        _BlurColorMix("_BlurColorMix", float) = 5  
        _FontColorPower("_FontColorPower", float) = 1  
        _StepX ("_StepX", float) = 1  
        _StepY ("_StepY", float) = 1  
    }  
  
    SubShader {  
  
        Tags {  
            "Queue"="Transparent"  
            "IgnoreProjector"="True"  
            "RenderType"="Transparent"  
            "PreviewType"="Plane"  
        }  
        Lighting Off Cull Off ZTest Always ZWrite Off  
        Blend SrcAlpha OneMinusSrcAlpha  
  
        Pass {  
            CGPROGRAM  
            #pragma vertex vert  
            #pragma fragment frag  
  
            #include "UnityCG.cginc"  
  
            struct appdata_t {  
                float4 vertex : POSITION;  
                fixed4 color : COLOR;  
                float2 texcoord : TEXCOORD0;  
                float3 normal : NORMAL;  
  
            };  
  
            struct v2f {  
                float4 vertex : SV_POSITION;  
                fixed4 color : COLOR;  
                float2 texcoord : TEXCOORD0;  
            };  
  
            sampler2D _MainTex;  
            uniform float4 _MainTex_ST;  
            uniform float4 _Color;  
            half _OutLineWidth;  
            half _BlurPower;  
            half _BlurColorMix;  
            half _FontColorPower;  
  
            half _StepX;  
            half _StepY;  
            v2f vert (appdata_t v)  
            {  
                v2f o;                
  
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);  
  
                o.color = v.color ;  
                o.texcoord = TRANSFORM_TEX(v.texcoord ,_MainTex);  
                return o;  
            }  
  
            fixed4 frag (v2f i) : SV_Target  
            {  
                fixed4 col = i.color ;  
                col.a *= tex2D(_MainTex , i.texcoord ).a;  
  
  
                half xLength = _StepX * _OutLineWidth;  
                half yLength = _StepY * _OutLineWidth;  
  
                half weight = 0.1;  
  
                half alpha = tex2D(_MainTex, i.texcoord.xy + float2(xLength , 0)).a * weight;  
                alpha += tex2D(_MainTex, i.texcoord.xy + float2(-xLength , 0)).a * weight;  
                alpha += tex2D(_MainTex, i.texcoord.xy + float2( 0, yLength)).a * weight;  
                alpha += tex2D(_MainTex, i.texcoord.xy + float2( 0, -yLength)).a * weight;  
                alpha += tex2D(_MainTex, i.texcoord.xy + float2(xLength , yLength)).a * weight;  
                alpha += tex2D(_MainTex, i.texcoord.xy + float2(xLength , -yLength)).a * weight;  
                alpha += tex2D(_MainTex, i.texcoord.xy + float2(-xLength , yLength)).a * weight;  
                alpha += tex2D(_MainTex, i.texcoord.xy + float2(-xLength , -yLength)).a * weight;  
  
  
                half4 outCol = _Color ;  
  
                outCol.a = max(0,(alpha - col.a));//外侵蚀   
                half inColBright = alpha * col.a;//内侵蚀 alpha * col.a  
                outCol.a += lerp(0.5 , 0, inColBright) * col.a ;//0.5防止内侵蚀过于严重  
                outCol.a *= _BlurPower;  
                  
  
                fixed4 finalCol;  
  
                finalCol.rgb = lerp(outCol.rgb * 0.1 , outCol.rgb, outCol.a);  
                finalCol.a = outCol.a + lerp( outCol.a, col.a, _BlurColorMix);  
                finalCol.rgb +=col.rgb * (col.a-outCol.a * col.a) *_FontColorPower;  
                  
                return finalCol;  
            }  
            ENDCG  
        }  
  
          
  
    }  
}