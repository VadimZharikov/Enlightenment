﻿using EnlightenmentApp.DAL.DataContext;
using EnlightenmentApp.DAL.Entities;
using EnlightenmentApp.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EnlightenmentApp.DAL.Tests.Repositories.GenericRepository
{
    public class GenericRepositoryIntegrationTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private GenericRepository<SectionEntity>? _repository;

        public GenericRepositoryIntegrationTests()
        {
            this._options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "EnlightenmentApp" + DateTime.Now.ToFileTimeUtc())
                .Options;
        }

        [Theory, AutoRepositoryData]
        public async Task GetEntities_DatabasePopulated_ReturnsEntityCollection(SectionEntity section)
        {
            await using DatabaseContext context = new(_options);
            this._repository = new (context);

            await context.Sections.AddAsync(section);
            await context.Sections.AddAsync(section);
            await context.SaveChangesAsync();

            var result = await _repository.GetEntities(default);

            result.ShouldNotBeNull();
        }

        [Theory, AutoRepositoryData]
        public async Task GetById_ValidId_ReturnsExpectedEntity(SectionEntity section)
        {
            await using DatabaseContext context = new(_options);
            _repository = new(context);

            var entity = await context.Sections.AddAsync(section);
            await context.SaveChangesAsync();

            SectionEntity? result = await _repository.GetById(entity.Entity.Id, default);

            result.ShouldNotBeNull();
            result.ShouldBeEquivalentTo(entity.Entity);
        }

        [Theory, AutoRepositoryData]
        public async Task Add_ValidEntity_ReturnsAddedEntity(SectionEntity section)
        {
            await using DatabaseContext context = new(_options);
            _repository = new(context);

            SectionEntity result = await _repository.Add(section, default);

            result.ShouldNotBeNull();
        }

        [Theory, AutoRepositoryData]
        public async Task Update_ValidEntity_ReturnsUpdatedEntity(SectionEntity section)
        {
            await using DatabaseContext context = new(_options);
            _repository = new(context);

            var entity = await context.Sections.AddAsync(section);
            await context.SaveChangesAsync();

            section.CheatSheet = "<p></p>";

            SectionEntity result = await _repository.Update(section, default);

            result.ShouldNotBeNull();
            result.ShouldBeEquivalentTo(entity.Entity);
        }

        [Theory, AutoRepositoryData]
        public async Task Delete_ValidId_EntityDeleted(SectionEntity section)
        {
            await using DatabaseContext context = new(_options);
            _repository = new(context);

            var entity = await context.Sections.AddAsync(section);
            await context.SaveChangesAsync();

            SectionEntity? result = await _repository.Delete(entity.Entity.Id, default);

            result.ShouldNotBeNull();
            result.ShouldBeEquivalentTo(entity.Entity);
        }

        [Theory, AutoRepositoryData]
        public async Task EntityExists_ValidId_ReturnsTrue(SectionEntity section)
        {
            await using DatabaseContext context = new(_options);
            _repository = new(context);

            var entity = await context.Sections.AddAsync(section);
            await context.SaveChangesAsync();

            bool result = await _repository.EntityExists(entity.Entity.Id, default);

            result.ShouldBeTrue();
        }

        [Theory, AutoRepositoryData]
        public async Task EntityExists_ValidEntity_ReturnsTrue(SectionEntity section)
        {
            await using DatabaseContext context = new(_options);
            _repository = new(context);

            var entity = await context.Sections.AddAsync(section);
            await context.SaveChangesAsync();

            bool result = await _repository.EntityExists(entity.Entity, default);

            result.ShouldBeTrue();
        }

        [Fact]
        public async Task GetEntities_EmptyDatabase_ReturnsEntityCollection()
        {
            await using DatabaseContext context = new(_options);
            this._repository = new(context);

            IEnumerable<SectionEntity> result = await _repository.GetEntities(default);

            result.ShouldBeEmpty();
        }

        [Theory, AutoRepositoryData]
        public async Task GetById_InValidId_ReturnsNull(SectionEntity section)
        {
            await using DatabaseContext context = new(_options);
            _repository = new(context);

            var entity = await context.Sections.AddAsync(section);
            await context.SaveChangesAsync();

            SectionEntity? result = await _repository.GetById(entity.Entity.Id + 1, default);

            result.ShouldBeNull();
        }

        [Fact]
        public async Task Add_Null_Throws()
        {
            await using DatabaseContext context = new(_options);
            _repository = new(context);

            await _repository.Add(null!, default)
                .ShouldThrowAsync<Exception>();
        }

        [Theory, AutoRepositoryData]
        public async Task Update_InValidEntity_Throws(SectionEntity section)
        {
            await using DatabaseContext context = new(_options);
            _repository = new(context);

            var entity = await context.Sections.AddAsync(section);
            await context.SaveChangesAsync();
            section.Id = entity.Entity.Id + 1;

            await _repository.Update(section, default)
                .ShouldThrowAsync<Exception>();
        }

        [Theory, AutoRepositoryData]
        public async Task Delete_InValidId_Throws(SectionEntity section)
        {
            await using DatabaseContext context = new(_options);
            _repository = new(context);

            var entity = await context.Sections.AddAsync(section);
            await context.SaveChangesAsync();

            await _repository.Delete(entity.Entity.Id + 1, default)
                .ShouldThrowAsync<Exception>();
        }

        [Theory, AutoRepositoryData]
        public async Task EntityExists_InValidId_ReturnsFalse(SectionEntity section)
        {
            await using DatabaseContext context = new(_options);
            _repository = new(context);

            var entity = await context.Sections.AddAsync(section);
            await context.SaveChangesAsync();

            bool result = await _repository.EntityExists(entity.Entity.Id + 1, default);

            result.ShouldBeFalse();
        }

        [Theory, AutoRepositoryData]
        public async Task EntityExists_InValidEntity_ReturnsFalse(SectionEntity section)
        {
            await using DatabaseContext context = new(_options);
            _repository = new(context);

            var entity = await context.Sections.AddAsync(section);
            await context.SaveChangesAsync();
            section.Id = entity.Entity.Id + 1;

            bool result = await _repository.EntityExists(entity.Entity, default);

            result.ShouldBeFalse();
        }
    }
}
