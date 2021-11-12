namespace MessierModel
{
    public class Constellation : IdentifiableObject<Constellation>, IHasIdentifier
    {
        public string Name { get; set; }

        public List<MessierTarget> Targets { get; set; } = new List<MessierTarget>();
    }
}
