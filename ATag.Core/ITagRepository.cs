namespace ATag.Core
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITagRepository
    {
        /// <summary>
        /// Create new tag.
        /// </summary>
        /// <param name="tag">Tag object</param>
        /// <returns>Tag Id</returns>
        int AddTag(Tag tag);

        /// <summary>
        /// Create new tag asynchronously.
        /// </summary>
        /// <param name="tag">Tag object</param>
        /// <returns>Tag Id</returns>
        Task<int> AddTagAsync(Tag tag);

        /// <summary>
        /// Add tagged entity for Tag.
        /// </summary>
        /// <param name="tagIds">Tag Id(s)</param>
        /// <param name="entityType">Entity type</param>
        /// <param name="entityKey">Entity key</param>
        /// <param name="note">Note</param>
        /// <param name="userId">User id who tag entity</param>
        void AddTaggedEntity(ICollection<int> tagIds, string entityType, string entityKey, string note, int userId);

        /// <summary>
        /// Add tagged entity for Tag asynchronously.
        /// </summary>
        /// <param name="tagIds">Tag Id(s)</param>
        /// <param name="entityType">Entity type</param>
        /// <param name="entityKey">Entity key</param>
        /// <param name="note">Note</param>
        /// <param name="userId">User id who tag entity</param>
        Task AddTaggedEntityAsync(ICollection<int> tagIds, string entityType, string entityKey, string note, int userId);

        /// <summary>
        /// Delete tag.
        /// </summary>
        /// <param name="tagId">Tag id</param>
        /// <param name="userId">User id who deleted tag</param>
        void DeleteTag(int tagId, int userId);

        /// <summary>
        /// Delete tag asynchronously.
        /// </summary>
        /// <param name="tagId">Tag id</param>
        /// <param name="userId">User id who deleted tag</param>
        Task DeleteTagAsync(int tagId, int userId);

        /// <summary>
        /// Delete tagged entity.
        /// </summary>
        /// <param name="tagId">Tag id</param>
        /// <param name="entityType">Entity type</param>
        /// <param name="entityKey">Entity key</param>
        void DeleteTaggedEntity(int tagId, string entityType, string entityKey);

        /// <summary>
        /// Delete tagged entity asynchronously.
        /// </summary>
        /// <param name="tagId">Tag id</param>
        /// <param name="entityType">Entity type</param>
        /// <param name="entityKey">Entity key</param>
        Task DeleteTaggedEntityAsync(int tagId, string entityType, string entityKey);

        /// <summary>
        /// Edit tag.
        /// </summary>
        /// <param name="name">Tag name</param>
        /// <param name="ownerId">Owner id</param>
        /// <param name="ownerType">Owner type</param>
        /// <param name="tagId">Tag id</param>
        /// <param name="userId">User Id who edit tag</param>
        void EditTag(string name, string ownerId, int ownerType, int tagId, int userId);

        /// <summary>
        /// Edit tag asynchronously.
        /// </summary>
        /// <param name="name">Tag name</param>
        /// <param name="ownerId">Owner id</param>
        /// <param name="ownerType">Owner type</param>
        /// <param name="tagId">Tag id</param>
        /// <param name="userId">User Id who edit tag</param>
        Task EditTagAsync(string name, string ownerId, int ownerType, int tagId, int userId);

        /// <summary>
        /// Edit tag's note.
        /// </summary>
        /// <param name="taggedEntityId">TaggedEntity Id</param>
        /// <param name="note">Comment</param>
        /// <param name="userId">User Id</param>
        void EditTagNote(int taggedEntityId, string note, int userId);


        /// <summary>
        /// Edit tag's note asynchronously.
        /// </summary>
        /// <param name="taggedEntityId">TaggedEntity Id</param>
        /// <param name="note">Comment</param>
        /// <param name="userId">User Id</param>
        Task EditTagNoteAsync(int taggedEntityId, string note, int userId);

        /// <summary>
        /// Edit tag's note.
        /// </summary>
        /// <param name="tagId">Tag Id</param>
        /// <param name="entityType">Entity type</param>
        /// <param name="entityKey">Entity key</param>
        /// <param name="note">Comment</param>
        /// <param name="userId">User Id</param>
        void EditTagNote(int tagId, string entityType, string entityKey, string note, int userId);

        /// <summary>
        /// Edit tag's note asynchronously.
        /// </summary>
        /// <param name="tagId">Tag Id</param>
        /// <param name="entityType">Entity type</param>
        /// <param name="entityKey">Entity key</param>
        /// <param name="note">Comment</param>
        /// <param name="userId">User Id</param>
        Task EditTagNoteAsync(int tagId, string entityType, string entityKey, string note, int userId);

        /// <summary>
        /// Get tags for the given entity.
        /// </summary>
        /// <param name="entityType">Entity type</param>
        /// <param name="entityKey">Entity key</param>
        /// <param name="filters">Owners' filters</param>
        /// <returns>List of tags</returns>
        IEnumerable<Tag> LoadEntityTags(string entityType, string entityKey, params TagOwnerFilter[] filters);

        /// <summary>
        /// Get tags for the given entity asynchronously.
        /// </summary>
        /// <param name="entityType">Entity type</param>
        /// <param name="entityKey">Entity key</param>
        /// <param name="filters">Owners' filters</param>
        /// <returns>List of tags</returns>
        Task<Tag[]> LoadEntityTagsAsync(string entityType, string entityKey, params TagOwnerFilter[] filters);

        /// <summary>
        /// Load tag details.
        /// </summary>
        /// <param name="tagId">Tag id</param>
        /// <returns>Returns tag details.</returns>
        Tag LoadTag(int tagId);

        /// <summary>
        /// Load tag details asynchronously.
        /// </summary>
        /// <param name="tagId">Tag id</param>
        /// <returns>Returns tag details.</returns>
        Task<Tag> LoadTagAsync(int tagId);

        /// <summary>
        /// Load tag entities.
        /// </summary>
        /// <param name="tagId">Tag id</param>
        /// <param name="pageIndex">current page number</param>
        /// <param name="pageSize">number of rows in page</param>
        /// <returns>Returns paginated list of tags for given filter(s).</returns>
        PagedEntity<TaggedEntity> LoadTaggedEntities(int tagId, int pageIndex, int pageSize);

        /// <summary>
        /// Load tag entities asynchronously.
        /// </summary>
        /// <param name="tagId">Tag id</param>
        /// <param name="pageIndex">current page number</param>
        /// <param name="pageSize">number of rows in page</param>
        /// <returns>Returns paginated list of tags for given filter(s).</returns>
        Task<PagedEntity<TaggedEntity>> LoadTaggedEntitiesAsync(int tagId, int pageIndex, int pageSize);

        /// <summary>
        /// Load tag entities.
        /// </summary>
        /// <param name="tagId">Tag id</param>
        /// <returns>Returns paginated list of tags for given filter(s).</returns>
        IEnumerable<TaggedEntity> LoadTaggedEntities(int tagId);

        /// <summary>
        /// Load tag entities asynchronously.
        /// </summary>
        /// <param name="tagId">Tag id</param>
        /// <returns>Returns paginated list of tags for given filter(s).</returns>
        Task<TaggedEntity[]> LoadTaggedEntitiesAsync(int tagId);

        /// <summary>
        /// Load tag details.
        /// </summary>
        /// <param name="taggedEntityId">TaggedEntity id</param>
        /// <returns>Returns tag's note.</returns>
        string LoadTagNote(int taggedEntityId);

        /// <summary>
        /// Load tag details asynchronously.
        /// </summary>
        /// <param name="taggedEntityId">TaggedEntity id</param>
        /// <returns>Returns tag's note.</returns>
        Task<string> LoadTagNoteAsync(int taggedEntityId);

        /// <summary>
        /// Load tag details.
        /// </summary>
        /// <param name="tagId">Tag Id</param>
        /// <param name="entityType">Entity type</param>
        /// <param name="entityKey">Entity key</param>
        /// <returns>Returns tag's note.</returns>
        string LoadTagNote(int tagId, string entityType, string entityKey);

        /// <summary>
        /// Load tag details asynchronously.
        /// </summary>
        /// <param name="tagId">Tag Id</param>
        /// <param name="entityType">Entity type</param>
        /// <param name="entityKey">Entity key</param>
        /// <returns>Returns tag's note.</returns>
        Task<string> LoadTagNoteAsync(int tagId, string entityType, string entityKey);

        /// <summary>
        /// Get all tags for filters.
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="filters">Owners' filters</param>
        /// <returns>Returns paginated list of tags.</returns>
        PagedEntity<Tag> LoadTags(int pageIndex, int pageSize, params TagOwnerFilter[] filters);

        /// <summary>
        /// Get all tags for filters asynchronously.
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="filters">Owners' filters</param>
        /// <returns>Returns paginated list of tags.</returns>
        Task<PagedEntity<Tag>> LoadTagsAsync(int pageIndex, int pageSize, params TagOwnerFilter[] filters);

        /// <summary>
        /// Get all tags for filters.
        /// </summary>
        /// <param name="filters">Owners' filters</param>
        /// <returns>Returns paginated list of tags.</returns>
        IEnumerable<Tag> LoadTags(params TagOwnerFilter[] filters);

        /// <summary>
        /// Get all tags for filters asynchronously.
        /// </summary>
        /// <param name="filters">Owners' filters</param>
        /// <returns>Returns paginated list of tags.</returns>
        Task<Tag[]> LoadTagsAsync(params TagOwnerFilter[] filters);

        /// <summary>
        /// Tag an entity.
        /// </summary>
        /// <param name="tagIds">Tag name</param>
        /// <param name="entityType">Entity type</param>
        /// <param name="entityKey">Entity key</param>
        /// <param name="note">Note</param>
        /// <param name="userId">User Id who edit tag</param>
        void TagEntity(ICollection<int> tagIds, string entityType, string entityKey, string note, int userId);

        /// <summary>
        /// Tag an entity asynchronously.
        /// </summary>
        /// <param name="tagIds">Tag name</param>
        /// <param name="entityType">Entity type</param>
        /// <param name="entityKey">Entity key</param>
        /// <param name="note">Note</param>
        /// <param name="userId">User Id who edit tag</param>
        Task TagEntityAsync(ICollection<int> tagIds, string entityType, string entityKey, string note, int userId);
    }
}