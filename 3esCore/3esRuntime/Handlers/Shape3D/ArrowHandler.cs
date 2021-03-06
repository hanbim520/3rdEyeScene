﻿using Tes.Net;
using UnityEngine;

namespace Tes.Handlers.Shape3D
{
  /// <summary>
  /// Handles Arrow shapes.
  /// </summary>
  public class ArrowHandler : ShapeHandler
  {
    /// <summary>
    /// Create the shape handler.
    /// </summary>
    /// <param name="categoryCheck"></param>
    public ArrowHandler(Runtime.CategoryCheckDelegate categoryCheck)
      : base(categoryCheck)
    {
      _solidMesh = Tes.Tessellate.Arrow.Solid();
      _wireframeMesh = Tes.Tessellate.Arrow.Wireframe();
      if (Root != null)
      {
        Root.name = Name;
      }
    }

    /// <summary>
    /// Handler name.
    /// </summary>
    public override string Name { get { return "Arrow"; } }

    /// <summary>
    /// <see cref="ShapeID.Arrow"/>
    /// </summary>
    public override ushort RoutingID { get { return (ushort)Tes.Net.ShapeID.Arrow; } }

    /// <summary>
    /// Solid mesh representation.
    /// </summary>
    public override Mesh SolidMesh { get { return _solidMesh; } }
    /// <summary>
    /// Wireframe mesh representation.
    /// </summary>
    public override Mesh WireframeMesh { get { return _wireframeMesh; } }

    /// <summary>
    /// Override to decode ScaleX as radius and ScaleZ as length.
    /// </summary>
    protected override void DecodeTransform(ObjectAttributes attributes, Transform transform, ushort flags)
    {
      float length = attributes.ScaleZ;
      float radius = attributes.ScaleX;
      if ((flags & (ushort)UpdateFlag.UpdateMode) == 0 || (flags & (ushort)UpdateFlag.Position) != 0)
      {
        transform.localPosition = new Vector3(attributes.X, attributes.Y, attributes.Z);
      }
      if ((flags & (ushort)UpdateFlag.UpdateMode) == 0 || (flags & (ushort)UpdateFlag.Rotation) != 0)
      {
        transform.localRotation = new Quaternion(attributes.RotationX, attributes.RotationY, attributes.RotationZ, attributes.RotationW);
      }
      if ((flags & (ushort)UpdateFlag.UpdateMode) == 0 || (flags & (ushort)UpdateFlag.Scale) != 0)
      {
        transform.localScale = new Vector3(radius, radius, length);
      }
    }

    /// <summary>
    /// Overridden to handle component pieces.
    /// </summary>
    protected override void EncodeAttributes(ref ObjectAttributes attr, GameObject obj, ShapeComponent comp)
    {
      Transform transform = obj.transform;
      attr.Colour = ShapeComponent.ConvertColour(comp.Colour);
      attr.X = transform.localPosition.x;
      attr.Y = transform.localPosition.y;
      attr.Z = transform.localPosition.z;
      attr.RotationX = transform.localRotation.x;
      attr.RotationY = transform.localRotation.y;
      attr.RotationZ = transform.localRotation.z;
      attr.RotationW = transform.localRotation.w;
      attr.ScaleX = transform.localScale.x;
      attr.ScaleY = attr.ScaleZ = transform.localScale.z;
    }

    /// <summary>
    /// Creates an arrow shape for serialisation.
    /// </summary>
    /// <param name="shapeComponent">The component to create a shape for.</param>
    /// <returns>A shape instance suitable for configuring to generate serialisation messages.</returns>
    protected override Shapes.Shape CreateSerialisationShape(ShapeComponent shapeComponent)
    {
      Shapes.Shape shape = new Shapes.Arrow();
      ConfigureShape(shape, shapeComponent);
      return shape;
    }

    private Mesh _solidMesh;
    private Mesh _wireframeMesh;
  }
}
