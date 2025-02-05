using System;
using System.Collections.Generic;
using UnityEditor.GraphToolsFoundation.Overdrive.Samples.Contexts;
using UnityEngine.UIElements;

namespace UnityEditor.GraphToolsFoundation.Overdrive
{
    /// <summary>
    /// Base class for UI parts that display a list of <see cref="BaseModelPropertyField"/>.
    /// </summary>
    public class ScriptEditorInspector : BaseModelUIPart
    {
        public static readonly string ussClassName = "ge-inspector-fields";

        VisualElement m_Root;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldsInspector"/> class.
        /// </summary>
        public ScriptEditorInspector(string name, ScriptNodeModel model, IModelUI ownerElement, string parentClassName)
            : base(name, model, ownerElement, parentClassName) { }

        /// <inheritdoc />
        public override VisualElement Root => m_Root;

        /// <inheritdoc />
        protected override void BuildPartUI(VisualElement parent)
        {
            m_Root = new VisualElement { name = PartName };
            m_Root.AddToClassList(ussClassName);
            m_Root.AddToClassList(m_ParentClassName.WithUssElement(PartName));

            var scriptNodeModel = (m_Model as ScriptNodeModel);
            
            if (scriptNodeModel == null)
                return;

            var editor = Editor.CreateEditor(scriptNodeModel.ScriptSerializedInstance);
            
            var imguiWrapper = new IMGUIContainer(() =>
            {
                if (editor == null)
                    return;
                editor.OnInspectorGUI();
            });
            
            m_Root.Add(imguiWrapper);

            parent.Add(m_Root);
        }

        protected override void UpdatePartFromModel() { }
    }
}
