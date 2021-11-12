using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessierModel
{
    public class MessierTarget : IdentifiableObject<MessierTarget>, IHasIdentifier
    {
        public int Index { get; set; }
        public string Thumbnail { get; set; }
        public string NGCDesignation { get; set; }
        public ObjectType Type { get; set; }
        public Constellation Constellation { get; set; }
        public TimeSpan RightAscension { get; set; }
        public decimal DeclinationDegrees { get; set; }
        public decimal Magnitude { get; set; }
        public int DistanceLightyears { get; set; }
        public Season ViewingSeason { get; set; }
        public Difficulty ViewingDifficulty { get; set; }
    }
}
