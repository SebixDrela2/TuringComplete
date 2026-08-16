namespace Turing.Core.Components;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class ComponentAttribute() : Attribute
{
    public bool Primitive { get; set; }
}