namespace ATag.EntityFrameworkCore
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ATag.Core;
    using ATag.EntityFrameworkCore.DataAccess;
    using Microsoft.EntityFrameworkCore;

    public class TagRepository : ITagRepository
    {
        internal readonly TagsDbContext DbContext;

        /// <summary>
        /// Initialize new instance of repository
        /// </summary>
        /// <param name="context"></param>
        public TagRepository(DataContext context)
        {
            this.DbContext = context.DbContext;
        }

        /// <inheritdoc />
        public int AddTag(Tag tag)
        {
            this.DbContext.Tags.Add(tag);
            this.DbContext.SaveChanges();
            return tag.Id;
        }

        /// <inheritdoc />
        public async Task<int> AddTagAsync(Tag tag)
        {
            await this.DbContext.Tags.AddAsync(tag);
            await this.DbContext.SaveChangesAsync();

            return await Task.FromResult(tag.Id);
        }

        /// <inheritdoc />
		public void AddTaggedEntity(ICollection<int> tagIds, string entityType, string entityKey, string note, int userId)
        {
            var existTagIds = this.DbContext.TaggedEntities
                .Where(t => t.EntityType == entityType && t.EntityKey == entityKey && tagIds.Contains(t.TagId))
                .Select(t => t.TagId)
                .ToList();

            var missingTagIds = tagIds.Except(existTagIds).ToList();

            var tags = this.DbContext.Tags
                .Where(a => missingTagIds.Contains(a.Id))
                .ToArray();

            foreach (var tag in tags)
            {
                tag.TagEntity(entityKey, entityType, note, userId);
            }

            this.DbContext.SaveChanges();
        }

        /// <inheritdoc />
        public async Task AddTaggedEntityAsync(ICollection<int> tagIds, string entityType, string entityKey, string note, int userId)
        {
            var existTagIds = await this.DbContext.TaggedEntities
                .Where(t => t.EntityType == entityType && t.EntityKey == entityKey && tagIds.Contains(t.TagId))
                .Select(t => t.TagId)
                .ToListAsync();

            var missingTagIds = tagIds.Except(existTagIds).ToList();

            var tags = await this.DbContext.Tags
                .Where(a => missingTagIds.Contains(a.Id))
                .ToArrayAsync();

            foreach (var tag in tags)
            {
                tag.TagEntity(entityKey, entityType, note, userId);
            }

            await this.DbContext.SaveChangesAsync();
            await Task.CompletedTask;
        }

        /// <inheritdoc />
		public void DeleteTag(int tagId, int userId)
        {
            var entity = this.DbContext.Tags.Find(tagId);

            if (entity == null)
            {
                return;
            }

            entity.Delete(userId);
            this.DbContext.SaveChanges();
        }

        /// <inheritdoc />
        public async Task DeleteTagAsync(int tagId, int userId)
        {
            var entity = await this.DbContext.Tags.FindAsync(tagId);

            if (entity == null)
            {
                return;
            }

            entity.Delete(userId);
            await this.DbContext.SaveChangesAsync();
            await Task.CompletedTask;
        }

        /// <inheritdoc />
		public void DeleteTaggedEntity(int tagId, string entityType, string entityKey)
        {
            var taggedEntities = this.DbContext.TaggedEntities
                .Where(a => a.EntityType == entityType && a.EntityKey == entityKey && a.TagId == tagId)
                .ToList();

            if (taggedEntities.Count == 0)
            {
                return;
            }

            this.DbContext.TaggedEntities.RemoveRange(taggedEntities);
            this.DbContext.SaveChanges();
        }

        /// <inheritdoc />
        public async Task DeleteTaggedEntityAsync(int tagId, string entityType, string entityKey)
        {
            var taggedEntities = await this.DbContext.TaggedEntities
                .Where(a => a.EntityType == entityType && a.EntityKey == entityKey && a.TagId == tagId)
                .ToListAsync();

            if (taggedEntities.Count == 0)
            {
                return;
            }

            this.DbContext.TaggedEntities.RemoveRange(taggedEntities);
            await this.DbContext.SaveChangesAsync();

            await Task.CompletedTask;
        }

        /// <inheritdoc />
		public void EditTag(string name, string ownerId, int ownerType, int tagId, int userId)
        {
            var tag = this.DbContext.Tags.Find(tagId);

            if (tag == null)
            {
                return;
            }

            var tagWithSameNameExists = this.DbContext.Tags.Any(t =>
                t.OwnerType == tag.OwnerType &&
                t.OwnerId == tag.OwnerId &&
                t.Name == name &&
                t.Id != tag.Id &&
                t.IsDeleted == false);

            if (tagWithSameNameExists)
            {
                throw new TagException("Tag with the same name already exists.");
            }

            tag.Edit(name, ownerType, ownerId, userId);

            this.DbContext.SaveChanges();
        }

        /// <inheritdoc />
        public async Task EditTagAsync(string name, string ownerId, int ownerType, int tagId, int userId)
        {
            var tag = await this.DbContext.Tags.FindAsync(tagId);

            if (tag == null)
            {
                return;
            }

            var tagWithSameNameExists = await this.DbContext.Tags.AnyAsync(t =>
                 t.OwnerType == tag.OwnerType &&
                 t.OwnerId == tag.OwnerId &&
                 t.Name == name &&
                 t.Id != tag.Id &&
                 t.IsDeleted == false);

            if (tagWithSameNameExists)
            {
                throw new TagException("Tag with the same name already exists.");
            }

            tag.Edit(name, ownerType, ownerId, userId);

            await this.DbContext.SaveChangesAsync();

            await Task.CompletedTask;
        }

        /// <inheritdoc />
		public void EditTagNote(int taggedEntityId, string note, int userId)
        {
            var taggedEntityData = this.DbContext.TaggedEntities
                .Include(a => a.TagNote)
                .Single(a => a.Id == taggedEntityId);

            if (taggedEntityData == null)
            {
                return;
            }

            taggedEntityData.SetNote(note, userId);

            this.DbContext.SaveChanges();
        }

        /// <inheritdoc />
        public async Task EditTagNoteAsync(int taggedEntityId, string note, int userId)
        {
            var taggedEntityData = await this.DbContext.TaggedEntities
                .Include(a => a.TagNote)
                .SingleAsync(a => a.Id == taggedEntityId);

            if (taggedEntityData == null)
            {
                return;
            }

            taggedEntityData.SetNote(note, userId);

            await this.DbContext.SaveChangesAsync();

            await Task.CompletedTask;
        }

        /// <inheritdoc />
		public void EditTagNote(int tagId, string entityType, string entityKey, string note, int userId)
        {
            var taggedEntityData = this.DbContext.TaggedEntities
                .Include(a => a.TagNote)
                .Single(a => a.TagId == tagId && a.EntityType.Equals(entityType) && a.EntityKey.Equals(entityKey));

            if (taggedEntityData == null)
            {
                return;
            }

            taggedEntityData.SetNote(note, userId);

            this.DbContext.SaveChanges();
        }

        /// <inheritdoc />
        public async Task EditTagNoteAsync(int tagId, string entityType, string entityKey, string note, int userId)
        {
            var taggedEntityData = await this.DbContext.TaggedEntities
                .Include(a => a.TagNote)
                .SingleAsync(a => a.TagId == tagId && a.EntityType.Equals(entityType) && a.EntityKey.Equals(entityKey));

            if (taggedEntityData == null)
            {
                return;
            }

            taggedEntityData.SetNote(note, userId);

            await this.DbContext.SaveChangesAsync();

            await Task.CompletedTask;
        }

        /// <inheritdoc />
		public IEnumerable<Tag> LoadEntityTags(string entityType, string entityKey, params TagOwnerFilter[] filters)
        {
            return this.DbContext.Tags
                .BelongingTo(filters.ToArray())
                .Where(a => !a.IsDeleted && a.TaggedEntities.Any(t => t.EntityKey == entityKey && t.EntityType == entityType))
                .ToArray();
        }

        /// <inheritdoc />
        public Task<Tag[]> LoadEntityTagsAsync(string entityType, string entityKey, params TagOwnerFilter[] filters)
        {
            return this.DbContext.Tags
                .BelongingTo(filters.ToArray())
                .Where(a => !a.IsDeleted && a.TaggedEntities.Any(t => t.EntityKey == entityKey && t.EntityType == entityType))
                .ToArrayAsync();
        }

        /// <inheritdoc />
		public Tag LoadTag(int tagId)
        {
            return this.DbContext.Tags.FirstOrDefault(a => a.Id == tagId && !a.IsDeleted);
        }

        /// <inheritdoc />
        public Task<Tag> LoadTagAsync(int tagId)
        {
            return this.DbContext.Tags.FirstOrDefaultAsync(a => a.Id == tagId && !a.IsDeleted);
        }

        /// <inheritdoc />
        public Task<PagedEntity<TaggedEntity>> LoadTaggedEntitiesAsync(int tagId, int pageIndex, int pageSize)
        {
            return this.DbContext.TaggedEntities
                .Include(a => a.TagNote)
                .Where(t => t.TagId == tagId)
                .OrderByDescending(t => t.Id)
                .WithPagingAsync(pageIndex, pageSize);
        }

        /// <inheritdoc />
		public IEnumerable<TaggedEntity> LoadTaggedEntities(int tagId)
        {
            return this.DbContext.TaggedEntities
                .Include(a => a.TagNote)
                .Where(t => t.TagId == tagId)
                .OrderByDescending(t => t.Id)
                .ToArray();
        }

        /// <inheritdoc />
        public Task<TaggedEntity[]> LoadTaggedEntitiesAsync(int tagId)
        {
            return this.DbContext.TaggedEntities
                .Include(a => a.TagNote)
                .Where(t => t.TagId == tagId)
                .OrderByDescending(t => t.Id)
                .ToArrayAsync();
        }

        /// <inheritdoc />
		public string LoadTagNote(int taggedEntityId)
        {
            return this.DbContext.TaggedEntities.Include(a => a.TagNote).SingleOrDefault(a => a.Id == taggedEntityId)?.TagNote?.Note;
        }

        /// <inheritdoc />
        public async Task<string> LoadTagNoteAsync(int taggedEntityId)
        {
            return await Task.FromResult(
                (await this.DbContext.TaggedEntities.Include(a => a.TagNote).SingleOrDefaultAsync(a => a.Id == taggedEntityId))?.TagNote?.Note);
        }

        /// <inheritdoc />
		public string LoadTagNote(int tagId, string entityType, string entityKey)
        {
            return this.DbContext.TaggedEntities
                .Include(a => a.TagNote)
                .SingleOrDefault(a => a.TagId == tagId && a.EntityType.Equals(entityType) && a.EntityKey.Equals(entityKey))?.TagNote?.Note;
        }

        /// <inheritdoc />
        public async Task<string> LoadTagNoteAsync(int tagId, string entityType, string entityKey)
        {
            return await Task.FromResult(
                (await this.DbContext.TaggedEntities
                    .Include(a => a.TagNote)
                    .SingleOrDefaultAsync(a => a.TagId == tagId && a.EntityType.Equals(entityType) && a.EntityKey.Equals(entityKey)))?.TagNote?.Note);
        }

        /// <inheritdoc />
		public PagedEntity<TaggedEntity> LoadTaggedEntities(int tagId, int pageIndex, int pageSize)
        {
            return this.DbContext.TaggedEntities
                .Include(a => a.TagNote)
                .Where(t => t.TagId == tagId)
                .OrderByDescending(t => t.Id)
                .WithPaging(pageIndex, pageSize);
        }

        /// <inheritdoc />
        public PagedEntity<Tag> LoadTags(int pageIndex, int pageSize, params TagOwnerFilter[] filters)
        {
            return this.DbContext.Tags
                .BelongingTo(filters.ToArray())
                .Where(a => !a.IsDeleted)
                .OrderBy(a => a.Id)
                .WithPaging(pageIndex, pageSize);
        }

        /// <inheritdoc />
        public Task<PagedEntity<Tag>> LoadTagsAsync(int pageIndex, int pageSize, params TagOwnerFilter[] filters)
        {
            return this.DbContext.Tags
                .BelongingTo(filters.ToArray())
                .Where(a => !a.IsDeleted)
                .OrderBy(a => a.Id)
                .WithPagingAsync(pageIndex, pageSize);
        }

        /// <inheritdoc />
		public IEnumerable<Tag> LoadTags(params TagOwnerFilter[] filters)
        {
            return this.DbContext.Tags
                .BelongingTo(filters.ToArray())
                .Where(a => !a.IsDeleted)
                .OrderBy(a => a.Id)
                .ToArray();
        }

        /// <inheritdoc />
        public Task<Tag[]> LoadTagsAsync(params TagOwnerFilter[] filters)
        {
            return this.DbContext.Tags
                .BelongingTo(filters.ToArray())
                .Where(a => !a.IsDeleted)
                .OrderBy(a => a.Id)
                .ToArrayAsync();
        }

        /// <inheritdoc />
		public void TagEntity(ICollection<int> tagIds, string entityType, string entityKey, string note, int userId)
        {
            var existingTagIds = this.DbContext.TaggedEntities
                .Where(t => t.EntityType == entityType && t.EntityKey == entityKey && tagIds.Contains(t.TagId))
                .Select(t => t.TagId)
                .ToList();

            var missingTagIds = tagIds.Except(existingTagIds).ToList();

            var tags = this.DbContext.Tags
                .Where(a => missingTagIds.Contains(a.Id))
                .ToArray();

            foreach (var tag in tags)
            {
                tag.TagEntity(entityKey, entityType, note, userId);
            }

            this.DbContext.SaveChanges();
        }

        /// <inheritdoc />
        public async Task TagEntityAsync(ICollection<int> tagIds, string entityType, string entityKey, string note, int userId)
        {
            var existingTagIds = await this.DbContext.TaggedEntities
                .Where(t => t.EntityType == entityType && t.EntityKey == entityKey && tagIds.Contains(t.TagId))
                .Select(t => t.TagId)
                .ToListAsync();

            var missingTagIds = tagIds.Except(existingTagIds).ToList();

            var tags = await this.DbContext.Tags
                .Where(a => missingTagIds.Contains(a.Id))
                .ToArrayAsync();

            foreach (var tag in tags)
            {
                tag.TagEntity(entityKey, entityType, note, userId);
            }

            await this.DbContext.SaveChangesAsync();

            await Task.CompletedTask;
        }
    }
}