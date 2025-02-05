using System.Collections.Generic;

namespace Jenga.Core
{
    public struct Graph
    {
        public ScriptInstance[] ScriptNodes;

        public Dictionary<string, ScriptInstance> NodesByGUID;
    }
}