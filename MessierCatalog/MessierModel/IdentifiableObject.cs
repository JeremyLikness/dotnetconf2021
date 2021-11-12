namespace MessierModel
{
    public abstract class IdentifiableObject<THasIdentifier>
        where THasIdentifier : IHasIdentifier, new()
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public override bool Equals(object? obj) =>
            obj is THasIdentifier otherObj && otherObj.Id == Id;

        public override int GetHashCode() => Id.GetHashCode();
    }
}
