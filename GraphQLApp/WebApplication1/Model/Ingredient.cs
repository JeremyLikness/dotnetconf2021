namespace WebApplication1.Model
{
    public class Ingredient : BaseModel
    {
        public string Name { get; set; } = string.Empty;

        public int WholeQty { get; set; } = 1;

        public int Numerator { get; set; } = 0;

        public int Denominator { get; set; } = 0;

        public string Measurement { get; set; } = string.Empty;

        public override string ToString() => Numerator == 0 ?
            $"{WholeQty} {Measurement} {Name}" :
            (WholeQty > 0 ?
                $"{WholeQty} {Numerator}/{Denominator} {Measurement} {Name}" :
                $"{Numerator}/{Denominator} {Measurement} {Name}");
    }
}
