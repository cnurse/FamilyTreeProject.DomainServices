using FamilyTreeProject.Core;
using FamilyTreeProject.Core.Data;
using FamilyTreeProject.DomainServices.Common;

namespace FamilyTreeProject.DomainServices
{
    public class CitationService : EntityService<Citation>, ICitationService
    {
        /// <summary>
        /// Constructs a Citation Service that will use the specified
        /// <see cref = "IUnitOfWork"></see> to retrieve data
        /// </summary>
        /// <param name = "unitOfWork">The <see cref = "IUnitOfWork"></see>
        /// to use to retrieve data</param>
        public CitationService(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
