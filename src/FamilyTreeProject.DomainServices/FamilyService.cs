//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using FamilyTreeProject.Data;
using FamilyTreeProject.DomainServices.Common;

namespace FamilyTreeProject.DomainServices
{
    /// <summary>
    /// The FamilyService provides a Facade to the Families store,
    /// allowing for additional business logic to be applied.
    /// </summary>
    public class FamilyService : EntityService<Family>, IFamilyService
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
