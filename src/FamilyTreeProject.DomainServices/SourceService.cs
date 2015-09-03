//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using FamilyTreeProject.DomainServices.Common;
using Naif.Data;

namespace FamilyTreeProject.DomainServices
{
    /// <summary>
    ///   The SourceService provides a Facade to the Sources store,
    ///   allowing for additional business logic to be applied.
    /// </summary>
    public class SourceService : EntityService<Source>, ISourceService
    {
        /// <summary>
        ///   Constructs a Source Service that will use the specified
        ///   <see cref = "IUnitOfWork"></see>
        ///   to retrieve data
        /// </summary>
        /// <param name = "unitOfWork">The <see cref = "IUnitOfWork"></see>
        ///   to use to retrieve data</param>
        public SourceService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    }
}
