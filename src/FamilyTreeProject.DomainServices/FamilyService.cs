using FamilyTreeProject.Core;
using FamilyTreeProject.Core.Data;
using FamilyTreeProject.DomainServices.Common;

namespace FamilyTreeProject.DomainServices
{
    /// <summary>
    /// The FamilyService provides a Facade to the Families store,
    /// allowing for additional business logic to be applied.
    /// </summary>
    public class FamilyService : AncestorEntityService<Family>, IFamilyService
    {
        /// <summary>
        /// Constructs a Family Service that will use the specified
        /// <see cref = "IUnitOfWork"></see> to retrieve data
        /// </summary>
        /// <param name = "unitOfWork">The <see cref = "IUnitOfWork"></see>
        /// to use to retrieve data</param>
        public FamilyService(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
