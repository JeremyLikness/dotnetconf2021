namespace WebApplication1.Model
{
    public class Step : BaseModel
    {
        public int Order { get; set; } = 0;

        public TimeSpan Duration { get; set; } = TimeSpan.Zero;

        public string Instructions { get; set; } = string.Empty;

        public override string ToString() => $"Step #{Order}";
    }
}
