using DataAccess;
using MessierModel;
using Microsoft.EntityFrameworkCore;

const string rootPath = @"<path to messier folder with images>";

Console.WriteLine("Messier catalog loader.");

var catalogCsv = Path.Combine(rootPath, "mcatalog.tsv");

Console.WriteLine("Parsing the catalog...");
var entries = File.ReadAllLines(catalogCsv);

var constellations = new List<Constellation>();
var types = new List<ObjectType>();
var targets = new List<MessierTarget>();

Func<string, ObjectType> FindOrCreateType = type =>
{
    var candidate = types.FirstOrDefault(t => t.Type == type);
    if (candidate == null)
    {
        candidate = new ObjectType
        {
            Type = type,
        };
        types.Add(candidate);
    }
    return candidate;
};

Func<string, Constellation> FindOrCreateConstellation = constellation =>
{
    var candidate = constellations.FirstOrDefault(c => c.Name == constellation);
    if (candidate == null)
    {
        candidate = new Constellation
        {
            Name = constellation,
        };
        constellations.Add(candidate);
    }
    return candidate;
};

static T StringToEnum<T>(string value) 
    where T: Enum 
    => (T)Enum.Parse(typeof(T), value.Trim().Replace(" ", string.Empty), true);

var parsed = 0;
var passedHeaders = false;
var notFound = string.Empty;

foreach (var entry in entries)
{
    if (!string.IsNullOrWhiteSpace(entry))
    {
        if (passedHeaders)
        {
            var parts = entry.Split('\t');
            parts[8] = parts[8]
                .Trim()
                .Replace(",", string.Empty);

            var target = new MessierTarget
            {
                Index = int.Parse(parts[0][1..]),
                NGCDesignation = parts[1].Trim(),
                Type = FindOrCreateType(parts[2].Trim())
            };
            target.Type.Targets.Add(target);
            target.Constellation = FindOrCreateConstellation(parts[3].Trim());
            target.Constellation.Targets.Add(target);
            var raParts = parts[4].Split('h');
            var hours = double.Parse(raParts[0].Trim());
            var minutes = double.Parse(raParts[1].Trim().Replace("m", string.Empty));
            target.RightAscension = TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes);
            var decParts = parts[5].Split('°');
            var arcMinutes = decimal.Parse(decParts[1].Trim());
            target.DeclinationDegrees = decimal.Parse(decParts[0].Trim());
            if (target.DeclinationDegrees < 0)
            {
                target.DeclinationDegrees -= arcMinutes / (decimal)60.0;
            }
            else
            {
                target.DeclinationDegrees += arcMinutes / (decimal)60.0;
            }
            target.Magnitude = decimal.Parse(parts[6].Trim());
            target.DistanceLightyears = int.Parse(parts[8].Trim().Replace(",", string.Empty));
            target.ViewingSeason = StringToEnum<Season>(parts[9]);
            target.ViewingDifficulty = StringToEnum<Difficulty>(parts[10]);

            var fileName = Path.Combine(rootPath, $"m{target.Index}.jpg");
            var setNotFound = false;
            if (!File.Exists(fileName))
            {
                if (!string.IsNullOrEmpty(notFound))
                {
                    target.Thumbnail = notFound;
                    fileName = string.Empty;
                }
                else
                {
                    fileName = Path.Combine(rootPath, "m0.jpg");
                    setNotFound = true;
                }
            }

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var bytes = File.ReadAllBytes(fileName);
                var b64String = Convert.ToBase64String(bytes);
                target.Thumbnail = "data:image/jpg;base64," + b64String;
                if (setNotFound)
                {
                    notFound = target.Thumbnail;
                }
            }

            targets.Add(target);
            Console.Write($"M{target.Index}\t{target.NGCDesignation}\t{target.Type.Type}\t");
            Console.Write($"{target.Constellation.Name}\t{target.RightAscension}\t{target.DeclinationDegrees}°\t");
            Console.WriteLine($"{target.DistanceLightyears}ly\t{target.ViewingSeason}\t{target.ViewingDifficulty}");

            parsed++;
        }
        else
        {
            passedHeaders = true;
        }
    }
}

Console.WriteLine($"Parsed {targets.Count} targets in {constellations.Count} constellations with {types.Count} types.");

Console.WriteLine("Saving to the database...");

var targetDb = Path.Combine(rootPath, "messier.db");

var options = new DbContextOptionsBuilder<MessierContext>()
    .UseSqlite($"Data Source={targetDb}");

using var context = new MessierContext(options.Options);

context.Database.EnsureDeleted();
context.Database.EnsureCreated();

context.Targets.AddRange(targets);
context.ObjectTypes.AddRange(types);
context.Constellations.AddRange(constellations);

await context.SaveChangesAsync();

Console.WriteLine("The database is ready");
