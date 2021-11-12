namespace WebApplication1.Model
{
    public abstract class BaseModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public override int GetHashCode() => Id.GetHashCode();

        public override bool Equals(object? obj) =>
            obj is BaseModel model &&
            model.GetType() == GetType() &&
            model.Id == Id;
    }
}
