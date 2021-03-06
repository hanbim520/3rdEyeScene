using UnityEngine;
using Tes.Maths;
using Tes.Net;

namespace Tes.Handlers
{
  /// <summary>
  /// A minimalist <see cref="MonoBehaviour"/> used to track information
  /// about objects generated by Tes shape handlers. For example, tracks wireframe/solid state.
  /// </summary>
  public class ShapeComponent : MonoBehaviour
  {
    /// <summary>
    /// Object ID, unique in its shape handler. Zero for transient objects.
    /// </summary>
    public uint ObjectID { get { return _objectID; } set { _objectID = value; } }
    [SerializeField]
    private uint _objectID;
    /// <summary>
    /// Object's filtering category.
    /// </summary>
    public ushort Category { get { return _category; } set { _category = value; } }
    [SerializeField]
    private ushort _category;
    /// <summary>
    /// <see cref="ObjectFlag"/>
    /// </summary>
    public ushort ObjectFlags { get { return _objectFlags; } set { _objectFlags = value; } }
    [SerializeField]
    private ushort _objectFlags;
    /// <summary>
    /// Object colour.
    /// </summary>
    public Color32 Colour { get { return _colour; } set { _colour = value; } }
    [SerializeField]
    private Color32 _colour;
    /// <summary>
    /// An extension value which can be used contextually by various shape types.
    /// </summary>
    public int ExtendedValue { get { return _extendedValue; } set { _extendedValue = value; } }
    [SerializeField]
    private int _extendedValue;

    /// <summary>
    /// May be used to flag a dirty status. Specific to the owning shape handler.
    /// </summary>
    public bool Dirty { get; set; }

    /// <summary>
    /// Translates a 3es colour to a native, Unity colour.
    /// </summary>
    /// <param name="colour">The 3es colour to translate.</param>
    /// <returns>The unity <see cref="Color32"/> value for <paramref name="colour"/>.</returns>
    public static Color32 ConvertColour(uint colour)
    {
      return new Tes.Maths.Colour(colour).ToUnity32();
    }

    /// <summary>
    /// Translates a Unity colour to a 3es colour value.
    /// </summary>
    /// <param name="colour">The Unity colour to translate.</param>
    /// <returns>The 3es colour value for <paramref name="colour"/>.</returns>
    public static uint ConvertColour(Color32 colour)
    {
      return Tes.Maths.ColourExt.FromUnity(colour).Value;
    }

    /// <summary>
    /// Is this a transient object (zero <see cref="ObjectID"/>).
    /// </summary>
    public bool IsTransient { get { return ObjectID == 0; } }

    /// <summary>
    /// Is <see cref="ObjectFlag.Wireframe"/> set?
    /// </summary>
    public bool Wireframe
    {
      get { return TestFlag(ObjectFlag.Wireframe); }
      set { SetFlag(ObjectFlag.Wireframe, value); }
    }

    /// <summary>
    /// Is <see cref="ObjectFlag.Transparent"/> set?
    /// </summary>
    public bool Transparent
    {
      get { return TestFlag(ObjectFlag.Transparent); }
      set { SetFlag(ObjectFlag.Transparent, value); }
    }

    /// <summary>
    /// Is <see cref="ObjectFlag.TwoSided"/> set?
    /// </summary>
    public bool TwoSided
    {
      get { return TestFlag(ObjectFlag.TwoSided); }
      set { SetFlag(ObjectFlag.TwoSided, value); }
    }

    /// <summary>
    /// Test <paramref name="flag"/> in <see cref="ObjectFlags"/>.
    /// </summary>
    /// <param name="flag">The flag to test.</param>
    /// <returns>True if the flag is set.</returns>
    public bool TestFlag(ObjectFlag flag)
    {
      return (ObjectFlags & (ushort)flag) != 0;
    }

    /// <summary>
    /// Set <paramref name="flag"/> in <see cref="ObjectFlags"/>.
    /// </summary>
    /// <param name="flag">The flag to set.</param>
    public void AddFlag(ObjectFlag flag)
    {
      ObjectFlags |= (ushort)flag;
    }

    /// <summary>
    /// Clear <paramref name="flag"/> in <see cref="ObjectFlags"/>.
    /// </summary>
    /// <param name="flag">The flag to clear.</param>
    public void ClearFlag(ObjectFlag flag)
    {
      ObjectFlags &= (ushort)~flag;
    }

    /// <summary>
    /// Set or clear <paramref name="flag"/> in <see cref="ObjectFlags"/>.
    /// </summary>
    /// <param name="flag">The flag to set or clear.</param>
    /// <param name="set">True to set, false to clear.</param>
    public void SetFlag(ObjectFlag flag, bool set)
    {
      if (set)
      {
        AddFlag(flag);
      }
      else
      {
        ClearFlag(flag);
      }
    }
  }
}
