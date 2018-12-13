namespace ATag.Tests
{
    using System.Linq;
    using System.Threading.Tasks;
    using ATag.Core;
    using ATag.EntityFrameworkCore;
    using Xunit;

    [Collection(nameof(DatabaseCollectionFixture))]
    public class DbTestAsync
    {
        public DbTestAsync(DatabaseFixture dbFixture)
        {
            this.tagRepository = new TagRepository(dbFixture.CreateDataContext());
        }

        private readonly ITagRepository tagRepository;

        [Fact]
        public async Task CanCreateTag()
        {
            var tagId = await this.tagRepository.AddTagAsync(new Tag("Test tag", 1, "10", 1));

            Assert.NotEqual(0, tagId);
            Assert.NotNull(await this.tagRepository.LoadTagAsync(tagId));
        }

        [Fact]
        public async Task CanOwnMultipleTag()
        {
            var tagId = await this.tagRepository.AddTagAsync(new Tag("Procure computer A", 2, "Chief Manager", 1));

            Assert.NotEqual(0, tagId);
            Assert.NotNull(await this.tagRepository.LoadTagAsync(tagId));

            var teamFilters = new[]
            {
                new TagOwnerFilter(2, "Chief Manager"),
                new TagOwnerFilter(2, "Chief Supervisor"),
                new TagOwnerFilter(1, "Chief John")
            };

            var tags = await this.tagRepository.LoadTagsAsync(teamFilters);

            Assert.NotEmpty(tags);
        }

        [Fact]
        public async Task CreateTaggedEntity()
        {
            var tagId = await this.tagRepository.AddTagAsync(new Tag("TestABC #2", 1, "10", 1));

            Assert.NotEqual(0, tagId);
            Assert.NotNull(await this.tagRepository.LoadTagAsync(tagId));

            var entityKey = "1";
            var entityType = "Circle A";
            await this.tagRepository.TagEntityAsync(new[] { tagId }, entityType, entityKey, "Test ABC #2", 1);

            var data = await this.tagRepository.LoadTaggedEntitiesAsync(tagId, 1, 10);
            Assert.NotEmpty(data.Results.Select(a => a.EntityKey.Equals(entityKey) && a.EntityType.Equals(entityType)));
        }

        [Fact]
        public async Task LoadTagNoteWithFilters()
        {
            var testAbc = "Test ABC #1";
            var tagId = await this.tagRepository.AddTagAsync(new Tag(testAbc, 1, "10", 1));

            Assert.NotEqual(0, tagId);
            Assert.NotNull(await this.tagRepository.LoadTagAsync(tagId));

            var entityKey = "1";
            var entityType = "Watch A";

            await this.tagRepository.TagEntityAsync(new[] { tagId }, entityType, entityKey, "Test Note #1", 1);

            var teamFilter = new TagOwnerFilter(2, "Team A");
            var personalFilter = new TagOwnerFilter(1, "10");

            var tags = await this.tagRepository.LoadTagsAsync(teamFilter, personalFilter);
            var tag = tags.First(a => a.Name.Equals(testAbc));
            var tagNote = await this.tagRepository.LoadTagNoteAsync(tag.Id, entityType, entityKey);
            var note = tagNote;

            Assert.NotEmpty(note);
        }

        [Fact]
        public async Task TagNotExists()
        {
            var teamFilter = new TagOwnerFilter(2, "Team ABC");
            var personalFilter = new TagOwnerFilter(1, "20");

            var tags = await this.tagRepository.LoadTagsAsync(teamFilter, personalFilter);

            Assert.Empty(tags);
        }
    }
}