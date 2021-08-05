namespace ids
{
    public interface IContextStore
    {
        string GetContext(string id);
        void AddContext(string id, string context);
    }
}
