using System;
using System.Collections.Generic;
using UnityEditor;

namespace Jenga.Runtime.Core
{
    public struct Graph
    {
        public ScriptNode[] ScriptNodes;

        public Dictionary<string, ScriptNode> NodesByGUID;
    }
}