﻿using Tes.Net;
using UnityEngine;

namespace Tes.Handlers.Shape3D
{
  /// <summary>
  /// Handles box shapes.
  /// </summary>
  public class BoxHandler : ShapeHandler
  {
    /// <summary>
    /// Create the shape handler.
    /// </summary>
    /// <param name="categoryCheck"></param>
    public BoxHandler(Runtime.CategoryCheckDelegate categoryCheck)
      : base(categoryCheck)
    {
      _solidMesh = Tes.Tessellate.Box.Solid();
      _wireframeMesh = Tes.Tessellate.Box.Wireframe();
      if (Root != null)
      {
        Root.name = Name;
      }
    }

    /// <summary>
    /// Handler name.
    /// </summary>
    public override string Name { get { return "Box"; } }

    /// <summary>
    /// <see cref="ShapeID.Box"/>
    /// </summary>
    public override ushort RoutingID { get { return (ushort)Tes.Net.ShapeID.Box; } }

    /// <summary>
    /// Solid mesh representation.
    /// </summary>
    public override Mesh SolidMesh { get { return _solidMesh; } }
    /// <summary>
    /// Wireframe mesh representation.
    /// </summary>
    public override Mesh WireframeMesh { get { return _wireframeMesh; } }

    /// <summary>
    /// Creates an box shape for serialisation.
    /// </summary>
    /// <param name="shapeComponent">The component to create a shape for.</param>
    /// <returns>A shape instance suitable for configuring to generate serialisation messages.</returns>
    protected override Shapes.Shape CreateSerialisationShape(ShapeComponent shapeComponent)
    {
      Shapes.Shape shape = new Shapes.Box();
      ConfigureShape(shape, shapeComponent);
      return shape;
    }

    private Mesh _solidMesh;
    private Mesh _wireframeMesh;
  }
}
