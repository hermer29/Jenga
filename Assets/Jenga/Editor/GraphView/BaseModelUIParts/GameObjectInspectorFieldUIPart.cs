using System.Collections.Generic;
using UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts.ModelCommands;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts.UI.BaseModelUIParts
{
    public class GameObjectInspectorFieldUIPart : BaseModelUIPart
    {
        public static readonly string ussClassName = "game-object-inspector-field";

        public GameObjectInspectorFieldUIPart(string name, IGraphElementModel model, IModelUI ownerElement, string parentClassName) : base(name, model, ownerElement, parentClassName)
        {
        }

        private VisualElement UiContainer { get; set; }
        private List<ObjectField> GameObjectLabels { get; set; } = new List<ObjectField>();

        public override VisualElement Root => UiContainer;

        protected override void BuildPartUI(VisualElement parent)
        {
            if (!(m_Model is ScriptNodeModel model))
                return;

            UiContainer = new VisualElement() { name = PartName };
            UiContainer.AddToClassList(ussClassName);
            UiContainer.AddToClassList(m_ParentClassName.WithUssElement(PartName));
            
            foreach (var gameObjectField in model.GetGameObjectFields())
            {
                var label = new ObjectField() { name = gameObjectField.Name };

                void OnChangeGameObject(ChangeEvent<GameObject[]> evt)
                {
                    if (!(m_Model is ScriptNodeModel bakeNodeModel))
                        return;

                    m_OwnerElement.CommandDispatcher.Dispatch(
                        new SetInspectorGameObjectFieldCommand(new[] { bakeNodeModel }, evt.newValue));
                        
                }
                
                label.RegisterCallback<ChangeEvent<GameObject[]>>(OnChangeGameObject);
                UiContainer.Add(label);
                GameObjectLabels.Add(label);
            }
            
            parent.Add(UiContainer);
        }

        protected override void PostBuildPartUI()
        {
            base.PostBuildPartUI();
            
            var stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                "Assets/Jenga/Editor/USS/InspectorFieldParts.uss");

            if (stylesheet != null)
            {
                UiContainer.styleSheets.Add(stylesheet);
            }
        }

        protected override void UpdatePartFromModel()
        {
            if (!(m_Model is ScriptNodeModel scriptNodeModel))
                return;

            for (var i = 0; i < GameObjectLabels.Count; i++)
            {
                var label = GameObjectLabels[i];
                label.SetValueWithoutNotify(scriptNodeModel.GameObjects[i]);
            }
        }
    }
}