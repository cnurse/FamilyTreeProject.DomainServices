﻿//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System.Collections.Generic;
using System.Linq;
using Naif.Core.Contracts;
using Naif.Data;

namespace FamilyTreeProject.DomainServices
{
    public class FamilyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILinqRepository<Family> _repository;

        /// <summary>
        ///   Constructs a Family Service that will use the specified
        ///   <see cref = "IUnitOfWork"></see>
        ///   to retrieve data
        /// </summary>
        /// <param name = "unitOfWork">The <see cref = "IUnitOfWork"></see>
        ///   to use to retrieve data</param>
        public FamilyService(IUnitOfWork unitOfWork)
        {
            //Contract
            Requires.NotNull(unitOfWork);

            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetLinqRepository<Family>();
        }

        /// <summary>
        ///   Adds a family to the data store and sets the <see cref = "Family.Id" /> property
        ///   of <paramref name = "family" /> to the id of the new family.
        /// </summary>
        /// <param name = "family">The family to add to the data store.</param>
        public void AddFamily(Family family)
        {
            //Contract
            Requires.NotNull(family);

            _repository.Add(family);
            _unitOfWork.Commit();
        }

        /// <summary>
        ///   Deletes a family from the data store
        /// </summary>
        /// <remarks>
        ///   The delete operation takes effect immediately
        /// </remarks>
        /// <param name = "family">The family to delete</param>
        public void DeleteFamily(Family family)
        {
            //Contract
            Requires.NotNull(family);

            _repository.Delete(family);
            _unitOfWork.Commit();
        }

        /// <summary>
        ///   Retrieves a single Family
        /// </summary>
        /// <param name = "id">The Id of the Family to retrieve</param>
        /// <param name = "includeChildren">A flag that indicates whether to get the children of the Family</param>
        /// <returns>An <see cref = "Family" /></returns>
        public Family GetFamily(int id, bool includeChildren)
        {
            //Contract
            Requires.NotNegative("id", id);

            var family = _repository.GetAll().SingleOrDefault(f => f.Id == id);

            return family;
        }

        public IEnumerable<Family> GetFamilies(int treeId)
        {
            //Contract
            Requires.NotNegative("treeId", treeId);

            return _repository.Find(ind => ind.TreeId == treeId);
        }

        /// <summary>
        ///   Updates a family in the data store.
        /// </summary>
        /// <param name = "family">The family to update in the data store.</param>
        public void UpdateFamily(Family family)
        {
            //Contract
            Requires.NotNull(family);

            _repository.Update(family);
            _unitOfWork.Commit();
        }
    }
}