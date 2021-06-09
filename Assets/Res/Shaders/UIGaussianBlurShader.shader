Shader "Custom/Unlit/UIGaussianBlur" {
    Properties {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _BlurSize ("Blue Size", Range(0, 8)) = 1
    }

    SubShader {
        Tags {
            "Queue"="Transparent"
            "RenderType"="Opaque"
        }

        CGINCLUDE
        #include "UnityCG.cginc"

        struct a2v
        {
            float4 localPos : POSITION;
        };

        struct v2f
        {
            float4 clipPos : SV_POSITION;
            float2 grabScreenPos0 : TEXCOORD0;
            float2 grabScreenPos1 : TEXCOORD1;
            float2 grabScreenPos2 : TEXCOORD2;
            float2 grabScreenPos3 : TEXCOORD3;
            float2 grabScreenPos4 : TEXCOORD4;
            float2 grabScreenPos5 : TEXCOORD5;
            float2 grabScreenPos6 : TEXCOORD6;
            float2 grabScreenPos7 : TEXCOORD7;
            float2 grabScreenPos8 : TEXCOORD8;
        };

        sampler2D _MainTex;
        fixed4 _Color;
        sampler2D _GrabTexture;
        float4 _GrabTexture_TexelSize;
        float _BlurSize;

        v2f vertVertical(a2v v)
        {
            v2f o;
            o.clipPos = UnityObjectToClipPos(v.localPos);

            float4 grabScreenPos = ComputeGrabScreenPos(o.clipPos);
            grabScreenPos = grabScreenPos / grabScreenPos.w;
            o.grabScreenPos0 = grabScreenPos - float2(0, _GrabTexture_TexelSize.y * 4) * _BlurSize;
            o.grabScreenPos1 = grabScreenPos - float2(0, _GrabTexture_TexelSize.y * 3) * _BlurSize;
            o.grabScreenPos2 = grabScreenPos - float2(0, _GrabTexture_TexelSize.y * 2) * _BlurSize;
            o.grabScreenPos3 = grabScreenPos - float2(0, _GrabTexture_TexelSize.y * 1) * _BlurSize;
            o.grabScreenPos4 = grabScreenPos;
            o.grabScreenPos5 = grabScreenPos + float2(0, _GrabTexture_TexelSize.y * 1) * _BlurSize;
            o.grabScreenPos6 = grabScreenPos + float2(0, _GrabTexture_TexelSize.y * 2) * _BlurSize;
            o.grabScreenPos7 = grabScreenPos + float2(0, _GrabTexture_TexelSize.y * 3) * _BlurSize;
            o.grabScreenPos8 = grabScreenPos + float2(0, _GrabTexture_TexelSize.y * 4) * _BlurSize;

            return o;
        }

        v2f vertHorizontal(a2v v)
        {
            v2f o;
            o.clipPos = UnityObjectToClipPos(v.localPos);

            float4 grabScreenPos = ComputeGrabScreenPos(o.clipPos);
            grabScreenPos = grabScreenPos / grabScreenPos.w;
            o.grabScreenPos0 = grabScreenPos - float2(_GrabTexture_TexelSize.x * 4, 0) * _BlurSize;
            o.grabScreenPos1 = grabScreenPos - float2(_GrabTexture_TexelSize.x * 3, 0) * _BlurSize;
            o.grabScreenPos2 = grabScreenPos - float2(_GrabTexture_TexelSize.x * 2, 0) * _BlurSize;
            o.grabScreenPos3 = grabScreenPos - float2(_GrabTexture_TexelSize.x * 1, 0) * _BlurSize;
            o.grabScreenPos4 = grabScreenPos;
            o.grabScreenPos5 = grabScreenPos + float2(_GrabTexture_TexelSize.x * 1, 0) * _BlurSize;
            o.grabScreenPos6 = grabScreenPos + float2(_GrabTexture_TexelSize.x * 2, 0) * _BlurSize;
            o.grabScreenPos7 = grabScreenPos + float2(_GrabTexture_TexelSize.x * 3, 0) * _BlurSize;
            o.grabScreenPos8 = grabScreenPos + float2(_GrabTexture_TexelSize.x * 4, 0) * _BlurSize;

            return o;
        }

        fixed4 frag(v2f i): SV_Target
        {
            fixed4 sum = fixed4(0, 0, 0, 0);
            sum += 0.05 * tex2D(_GrabTexture, i.grabScreenPos0);
            sum += 0.09 * tex2D(_GrabTexture, i.grabScreenPos1);
            sum += 0.15 * tex2D(_GrabTexture, i.grabScreenPos2);
            sum += 0.12 * tex2D(_GrabTexture, i.grabScreenPos3);
            sum += 0.18 * tex2D(_GrabTexture, i.grabScreenPos4);
            sum += 0.15 * tex2D(_GrabTexture, i.grabScreenPos5);
            sum += 0.12 * tex2D(_GrabTexture, i.grabScreenPos6);
            sum += 0.09 * tex2D(_GrabTexture, i.grabScreenPos7);
            sum += 0.05 * tex2D(_GrabTexture, i.grabScreenPos8);
            return sum;
        }
        ENDCG

        GrabPass {
        }

        Pass {
            CGPROGRAM
            #pragma vertex vertVertical
            #pragma fragment frag
            ENDCG
        }

        GrabPass {
        }

        Pass {
            CGPROGRAM
            #pragma vertex vertHorizontal
            #pragma fragment frag
            ENDCG
        }

    }
    Fallback Off
}