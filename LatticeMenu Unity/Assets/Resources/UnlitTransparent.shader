Shader "Custom/UnlitTransparent" {
    Properties{
        _Color("_Color", Color) = (0, 0, 0, 0.5)
    }
    SubShader{
        Lighting Off
        ZWrite Off
        Cull Back
        Blend SrcAlpha OneMinusSrcAlpha
        Tags {"Queue" = "Transparent"}
        Color[_Color]
        Pass {
        }
    }
    FallBack "Unlit/Transparent"
}