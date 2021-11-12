namespace MessierModel
{
    public class ObjectType : IdentifiableObject<ObjectType>, IHasIdentifier
    {
        public string Type { get; set; }
        public List<MessierTarget> Targets { get; set; } = new List<MessierTarget>();
    }
}