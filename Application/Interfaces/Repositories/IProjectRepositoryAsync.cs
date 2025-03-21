using Application.Features.Projects.Queries;
using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IProjectRepositoryAsync : IGenericRepositoryAsync<Project>
    {
        Task<PagedResponse<List<Project>>> GetAllProjects(GetAllProjectsParameter filter);
    }
}
