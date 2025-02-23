namespace Jenga.Core
{
    public interface IScriptGraphProvider
    {
        Graph GetGraph();
        public event OnNodeCreatedInGameObject OnNodeCreatedInGameObject;
    }

    public delegate void OnNodeCreatedInGameObject(string gameObjectGuid, string componentGuid);
}