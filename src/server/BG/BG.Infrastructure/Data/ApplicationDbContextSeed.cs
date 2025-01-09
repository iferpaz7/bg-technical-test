using System.Reflection;
using Newtonsoft.Json;

namespace BG.Infrastructure.Data;

public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (!await context.IdentificationTypes.AnyAsync())
        {
            var jsonData = await File.ReadAllTextAsync(path + @"/Data/SeedData/IdentificationTypes.json");
            var deserializedData = JsonConvert.DeserializeObject<List<IdentificationType>>(jsonData);
            context.IdentificationTypes.AddRange(deserializedData);
        }

        if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
    }
}