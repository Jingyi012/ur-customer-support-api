using Application.DTOs.User;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Users.Queries
{
    public class GetUserSelectionQuery: IRequest<Response<List<UserSelectionDto>>>
    {
    }

    public class GetUserSelectionQueryHandler : IRequestHandler<GetUserSelectionQuery, Response<List<UserSelectionDto>>>
    {
        private readonly IUserService _userService;
        public GetUserSelectionQueryHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<Response<List<UserSelectionDto>>> Handle(GetUserSelectionQuery request, CancellationToken cancellationToken)
        {
            var result = await _userService.GetAllUserIdAndNamesAsync();
            return new Response<List<UserSelectionDto>>(result);
        }
    }
}
