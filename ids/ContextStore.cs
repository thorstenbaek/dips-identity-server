using System.Collections.Concurrent;

namespace ids
{
    public class ContextStore : IContextStore
    {
        private ConcurrentDictionary<string, string> Contexts;

        public ContextStore()
        {
            Contexts = new ConcurrentDictionary<string, string>();
        }

        public void AddContext(string id, string context)
        {
            Contexts[id] = context;
        }

        public string GetContext(string id)
        {
            return Contexts[id];
        }
    }
}
