using Core;

namespace Persistence.Messages.Events
{
    public class ResourceSaved : IEvent
    {
        public string Id { get; set; }
    }
}
