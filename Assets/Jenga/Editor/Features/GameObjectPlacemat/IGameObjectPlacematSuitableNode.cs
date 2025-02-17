using UnityEngine.GraphToolsFoundation.Overdrive;

namespace Jenga.Editor.Features.GameObjectGraphPresentation
{
    public interface IGameObjectPlacematSuitableNode
    {
        SerializableGUID ContainingPlacematGuid { get; set; }
    }
}