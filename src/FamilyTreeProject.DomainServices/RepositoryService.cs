using FamilyTreeProject.Common.Data;
using FamilyTreeProject.Core;
using FamilyTreeProject.DomainServices.Common;

namespace FamilyTreeProject.DomainServices
{
    /// <summary>
    ///   The RepositoryService provides a Facade to the Repositories(GEDCOM) store,
    ///   allowing for additional business logic to be applied.
    /// </summary>
    public class RepositoryService : EntityService<Repository>, IRepositoryService
    {
        /// <summary>
        ///   Constructs a Repository Service that will use the specified
        ///   <see cref = "IUnitOfWork"></see>
        ///   to retrieve data
        /// </summary>
        /// <param name = "unitOfWork">The <see cref = "IUnitOfWork"></see>
        ///   to use to retrieve data</param>
        public RepositoryService(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
