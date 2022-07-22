using FamilyTreeProject.Common.Data;
using FamilyTreeProject.Common.Models;
using FamilyTreeProject.DomainServices.Common;

namespace FamilyTreeProject.DomainServices
{
    public class NoteService : EntityService<Note>, INoteService
    {
        /// <summary>
        /// Constructs a Fact Service that will use the specified
        /// <see cref = "IUnitOfWork"></see> to retrieve data
        /// </summary>
        /// <param name = "unitOfWork">The <see cref = "IUnitOfWork"></see>
        /// to use to retrieve data</param>
        public NoteService(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
