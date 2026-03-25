using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

using Storm.TechTask.SharedKernel.Authorization;
using Storm.TechTask.SharedKernel.Handlers;
using Storm.TechTask.SharedKernel.Interfaces;

using Microsoft.EntityFrameworkCore; // need this for .Include()

namespace Storm.TechTask.Core.ProjectAggregate.Queries
{
    public static class AllProjects
    {
        public class Query : IQuery<List<Project>>
        {
        }

        public class Authorizer : BaseQueryAuthorizer<Query>
        {
            protected override bool UserHasCorrectRole(IAppUser user) => user.HasRole(AppRole.ProjectAdmin | AppRole.ProjectReader);
        }

        public class Handler : BaseQueryHandler, IRequestHandler<Query, List<Project>>
        {
            public Handler(IRepository repository, IUserSession session) : base(repository) { }


            public async Task<List<Project>> Handle(Query request, CancellationToken cancellationToken)
            {
                var projectsQuery = _repository.Query<Project>()
                                               .Include(p => p.Items);

                return await projectsQuery.ToListAsync(cancellationToken);
            }
            // this calls Query<Project>() from the repository and INCLUDES the items that have been added


            /*public async Task<List<Project>> Handle(Query request, CancellationToken cancellationToken)
            {
                
                return await LoadAllEntities<Project>(cancellationToken);
            }*/
        }
    }
}
