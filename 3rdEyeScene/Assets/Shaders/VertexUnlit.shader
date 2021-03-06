﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "VertexColour/VertexUnlit"
{
  Properties
  {
    // Tint is for UI highlight. Colour is the primary colour.
    _Color("Main Colour", Color) = (1, 1, 1, 1)
    _Tint("Tint", Color) = (1, 1, 1, 1)
  }

  SubShader
  {
    Tags { "RenderType" = "Opaque" }
    LOD 100

    Pass
    {
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"

      uniform float4 _Color;
      uniform float4 _Tint;

      // Note: we do lighting in the vertex shader because it is uniform across the point quad.
      struct VertexInput
      {
        float4 vertex : POSITION;
        float4 colour : COLOR;
      };

      struct FragmentInput
      {
        float4 vertex : SV_POSITION;
        float4 colour : COLOR;
      };

      FragmentInput vert(VertexInput v)
      {
        FragmentInput o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.colour = _Color * _Tint * v.colour;
        return o;
      }

      float4 frag(FragmentInput i) : COLOR
      {
        return i.colour;
      }
      ENDCG
    }
  }
}
