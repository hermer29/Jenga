using System;
using System.Collections.Generic;
using Jenga.Core.Utilities.Reactivity;
using Jenga.Editor.Features.GameObjectGraphPresentation;
using Jenga.Runtime;
using UnityEditor.GraphToolsFoundation.Overdrive;
using UnityEditor.GraphToolsFoundation.Overdrive.BasicModel;
using UnityEngine;
using UnityEngine.GraphToolsFoundation.Overdrive;

namespace Jenga.Editor.Base
{
    [Serializable]
    public class ScriptGraphModel : GraphModel
    {
        public ScriptGraphModel()
        {
            StencilType = null;
        }

        public override Type DefaultStencilType => typeof(ScriptGraphStencil);
        [SerializeField] public ReactiveProperty<JengaRuntime> CurrentRuntime = new ReactiveProperty<JengaRuntime>(null);

        public IPlacematModel CreateGameObjectPlacemat(Rect position, SerializableGUID gameObjectGuid, string name,
            SpawnFlags spawnFlags = SpawnFlags.Default)
        {
            var placematModel = InstantiateGameObjectPlacemat(position, gameObjectGuid, name);
            if (!spawnFlags.IsOrphan())
            {
                placematModel.ZOrder = this.GetPlacematMaxZOrder() + 1;

                AddPlacemat(placematModel);
            }

            return placematModel;
        }
        
        protected virtual IPlacematModel InstantiateGameObjectPlacemat(Rect position,
            SerializableGUID gameObjectGuid, string name)
        {
            var placematModelType = GetGameObjectPlacematType();
            var placematModel = Instantiate<IPlacematModel>(placematModelType) as GameObjectPlacematModel;
            placematModel.GameObjectGuid = gameObjectGuid;
            placematModel.PositionAndSize = position;
            placematModel.AssetModel = AssetModel;
            placematModel.ZOrder = 0;
            placematModel.Title = name;
            return placematModel;
        }

        private Type GetGameObjectPlacematType() => typeof(GameObjectPlacematModel);
    }
}
