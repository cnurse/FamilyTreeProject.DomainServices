using FamilyTreeProject.Common.Data;
using FamilyTreeProject.Core;
using FamilyTreeProject.DomainServices.Common;

namespace FamilyTreeProject.DomainServices
{
    /// <summary>
    ///   The IndividualService provides a Facade to the Individuals store,
    ///   allowing for additional business logic to be applied.
    /// </summary>
    public class IndividualService : AncestorEntityService<Individual> , IIndividualService
    {
        /// <summary>
        /// Constructs an Individuals Service to manage Individuals
        /// </summary>
        /// <param name = "unitOfWork">The Unit Of Work to use to retrieve data</param>
        public IndividualService(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
