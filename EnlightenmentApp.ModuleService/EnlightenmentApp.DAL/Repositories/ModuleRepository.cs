﻿using EnlightenmentApp.DAL.DataContext;
using EnlightenmentApp.DAL.Entities;
using EnlightenmentApp.DAL.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EnlightenmentApp.DAL.Repositories
{
    public class ModuleRepository : GenericRepository<ModuleEntity>, IModuleRepository
    {
        public ModuleRepository(DatabaseContext dbContext) : base(dbContext)
        {

        }

        public override async Task<IEnumerable<ModuleEntity>> GetEntities(CancellationToken ct)
        {
            var modules = await _context.Modules.Include(m => m.Tags)
                .AsNoTracking().ToListAsync(ct);
            return modules;
        }

        public override async Task<ModuleEntity?> GetById(int id, CancellationToken ct)
        {
            var module = await _context.Modules
                .Include(m => m.Sections)
                .FirstOrDefaultAsync(m => m.Id == id, ct);
            return module;
        }

        public override async Task<ModuleEntity> Add(ModuleEntity moduleEntity, CancellationToken ct)
        {
            if (!moduleEntity.Tags.IsNullOrEmpty())
            {
                _context.Tags.AttachRange(moduleEntity.Tags);
            }

            await _context.Modules.AddAsync(moduleEntity, ct);
            await _context.SaveChangesAsync(ct);
            return moduleEntity;
        }

        public override async Task<ModuleEntity> Update(ModuleEntity moduleEntity, CancellationToken ct)
        {
            var dbModuleEntity = _context.Modules
            .Include(p => p.Tags)
            .First(p => p.Id == moduleEntity.Id);
            SetTagsDiff(moduleEntity, dbModuleEntity);

            dbModuleEntity.Tags.ToList().AddRange(moduleEntity.Tags);
            await _context.SaveChangesAsync(ct);
            return moduleEntity;
        }

        private static void SetTagsDiff(ModuleEntity moduleEntity, ModuleEntity dbModuleEntity)
        {
            //remove unused tags
            dbModuleEntity.Tags.ToList()
                .RemoveAll(m => !moduleEntity.Tags.ToList()
                    .Exists(x => x.Id == m.Id));
            //store new tags
            moduleEntity.Tags.ToList().RemoveAll(m => dbModuleEntity.Tags.ToList()
                            .Exists(x => x.Id == m.Id));
        }
    }
}
