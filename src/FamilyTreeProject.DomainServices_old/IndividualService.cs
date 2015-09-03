//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using Naif.Core.Caching;
using Naif.Core.Collections;
using Naif.Core.Contracts;
using Naif.Data;

namespace FamilyTreeProject.DomainServices
{
    /// <summary>
    ///   The IndividualsService provides a Facade to the Individuals store,
    ///   allowing for additional business logic to be applied.
    /// </summary>
    public class IndividualService : IIndividualService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Individual> _individualRepository;

        /// <summary>
        /// Constructs an Individuals Service to manage Individuals
        /// </summary>
        /// <param name = "unitOfWork">The Unit Of Work to use to retrieve data</param>
        internal IndividualService(IUnitOfWork unitOfWork)
        {
            Requires.NotNull(unitOfWork);

            _unitOfWork = unitOfWork;
            _individualRepository = _unitOfWork.GetRepository<Individual>();
        }

        internal ITreeService TreeService { get; set; }

        /// <summary>
        ///   Adds an individual to the data store and sets the <see cref = "Individual.Id" /> property
        ///   of <paramref name = "individual" /> to the id of the new individual.
        /// </summary>
        /// <param name = "individual">The individual to add to the data store.</param>
        public void AddIndividual(Individual individual)
        {
            //Contract
            Requires.NotNull(individual);

            _individualRepository.Add(individual);
            _unitOfWork.Commit();
        }

        /// <summary>
        ///   Deletes an individual from the data store
        /// </summary>
        /// <remarks>
        ///   The delete operation takes effect immediately
        /// </remarks>
        /// <param name = "individual">The individual to delete</param>
        public void DeleteIndividual(Individual individual)
        {
            //Contract
            Requires.NotNull(individual);

            _individualRepository.Delete(individual);
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Retrieves a single Individual
        /// </summary>
        /// <param name = "id">The Id of the Indiidual to retrieve</param>
        /// <param name="treeId">The Id of the tree</param>
        /// <returns>An <see cref = "Individual" /></returns>
        public Individual GetIndividual(int id, int treeId)
        {
            //Contract
            Requires.NotNegative("id", id);
            Requires.NotNegative("treeId", treeId);

            return GetIndividuals(treeId).SingleOrDefault(i => i.Id == id);
        }

        /// <summary>
        /// Retrieves all the individuals for a particular tree
        /// </summary>
        /// <param name="treeId">The Id of the tree</param>
        /// <returns>A collection of <see cref = "Individual" />objects</returns>
        public IEnumerable<Individual> GetIndividuals(int treeId)
        {
            Requires.NotNegative("treeId", treeId);

            return TreeService.GetTree(treeId).Individuals;
        }

        /// <summary>
        /// Gets a page of individuals based on a Search Term
        /// </summary>
        /// <param name="treeId">The portalId</param>
        /// <param name="predicate">The search term to use</param>
        /// <param name="pageIndex">The page index to return</param>
        /// <param name="pageSize">The page size</param>
        /// <returns>content type collection.</returns>
        public IPagedList<Individual> GetIndividuals(int treeId, Func<Individual, bool> predicate, int pageIndex, int pageSize)
        {
            return new PagedList<Individual>(GetIndividuals(treeId).Where(predicate), pageIndex, pageSize);
        }

        /// <summary>
        ///   Updates an individual in the data store.
        /// </summary>
        /// <param name = "individual">The individual to update in the data store.</param>
        public void UpdateIndividual(Individual individual)
        {
            //Contract
            Requires.NotNull(individual);

            _individualRepository.Update(individual);
            _unitOfWork.Commit();
        }
    }
}
