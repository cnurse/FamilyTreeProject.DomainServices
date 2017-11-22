using FamilyTreeProject.Core;
using FamilyTreeProject.Core.Data;
using FamilyTreeProject.DomainServices.Common;

namespace FamilyTreeProject.DomainServices
{
    public class FactService : EntityService<Fact>, IFactService
    {
        /// <summary>
        /// Constructs a Fact Service that will use the specified
        /// <see cref = "IUnitOfWork"></see> to retrieve data
        /// </summary>
        /// <param name = "unitOfWork">The <see cref = "IUnitOfWork"></see>
        /// to use to retrieve data</param>
        public FactService(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
