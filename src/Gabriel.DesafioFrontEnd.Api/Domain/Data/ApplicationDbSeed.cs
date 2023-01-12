using Bogus;
using Gabriel.DesafioFrontEnd.Domain.Entities;

namespace Gabriel.DesafioFrontEnd.Api.Domain.Data
{
    public class ApplicationDbSeed
    {

        public static void SeedAsync(ApplicationDbContext dbContext)
        {

            var _mediaAssetInMemoryStore = new Dictionary<int, MediaAsset>
            {
               { 1, new MediaAsset { ThumbAssetName = "thumb-cam-1.png", VideoAssetName = "cam-1-video.mp4" }},
               { 2, new MediaAsset { ThumbAssetName = "thumb-cam-2.jpg", VideoAssetName = "cam-2-video.mp4" }},
            };

            if (!dbContext.Customer.Any())
            {
                var customers = new List<Customer>();

                for (int i = 1; i <= 20; i++)
                {
                    var customer = new Faker<Customer>()
                        .RuleFor(x => x.Name, x => x.Name.FullName())
                        .RuleFor(x => x.Address, x => x.Address.FullAddress())
                        .RuleFor(x => x.IsActive, x => x.Random.Bool())
                        .RuleFor(x => x.Id, x => x.Random.Uuid())
                        .Generate();

                    dbContext.Customer.Add(customer);

                    for (int x = 1; x < 3; x++)
                    {
                        var mediaAsset = _mediaAssetInMemoryStore[x];
                        var camera = new Faker<Camera>()
                       .RuleFor(x => x.Id, x => x.Random.Number(1, 100000))
                       .RuleFor(x => x.Description, x => x.Name.Suffix())
                       .RuleFor(x => x.Thumbnail, x => mediaAsset.ThumbAssetName)
                       .RuleFor(x => x.Video, x => mediaAsset.VideoAssetName)
                       .RuleFor(x => x.Customer, x => customer)
                       .Generate();
                        dbContext.Camera.Add(camera);

                    }
                }
                dbContext.SaveChanges();
            }


        }

        public ApplicationDbSeed() { }

    }

    public class MediaAsset
    {
        public string VideoAssetName { get; set; }
        public string ThumbAssetName { get; set; }
    }
}
