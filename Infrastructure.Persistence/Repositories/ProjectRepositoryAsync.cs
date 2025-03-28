using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Wrappers;
using System.Linq;
using Application.Features.Projects.Queries;

namespace Infrastructure.Persistence.Repositories
{
    public class ProjectRepositoryAsync : GenericRepositoryAsync<Project>, IProjectRepositoryAsync
    {
        private readonly DbSet<Project> _projects;
        private readonly ApplicationDbContext _dbContext;

        public ProjectRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _projects = dbContext.Set<Project>();
            _dbContext = dbContext;
        }

        public override async Task UpdateAsync(Project project)
        {
            var existingProjectImage = await _dbContext.ProjectImages.Where(p => p.ProjectId == project.Id).ToListAsync();
            _dbContext.ProjectImages.RemoveRange(existingProjectImage);

            foreach (var image in project.Images)
            {
                _dbContext.ProjectImages.Add(image);
            }

            _dbContext.Entry(project).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public override async Task<Project> GetByIdAsync(int id)
        {
            return await _projects
                .Include(p => p.Images.OrderBy(img => img.Order))
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PagedResponse<List<Project>>> GetAllProjects(GetAllProjectsParameter filter)
        {
            var query = _projects
                .Include(p => p.Images.OrderBy(img => img.Order))
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(u => u.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(u => u.IsActive == filter.IsActive.Value);
            }

            var totalCount = await query.CountAsync();

            var project = await query
                .OrderByDescending(p => p.LastModified)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .AsNoTracking()
                .ToListAsync();

            return new PagedResponse<List<Project>>(project, filter.PageNumber, filter.PageSize, totalCount);
        }
    }
}
