//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System;
using System.Collections.Generic;
using Naif.Core.Collections;

namespace FamilyTreeProject.DomainServices
{
    /// <summary>
    ///   An interface that represents the Individuals Service
    /// </summary>
    public interface IIndividualService
    {
        /// <summary>
        ///   Adds an individual to the data store and sets the <see cref = "Individual.Id" /> property
        ///   of <paramref name = "newIndividual" /> to the id of the new individual.
        /// </summary>
        /// <param name = "newIndividual">The individual to add to the data store.</param>
        void AddIndividual(Individual newIndividual);

        /// <summary>
        ///   Deletes an individual from the data store
        /// </summary>
        /// <remarks>
        ///   The delete operation takes effect immediately
        /// </remarks>
        /// <param name = "individual">The individual to delete</param>
        void DeleteIndividual(Individual individual);

        /// <summary>
        ///   Retrieves a single Individual
        /// </summary>
        /// <param name = "id">The Id of the Individual to retrieve</param>
        /// <param name="treeId">The Id of the tree</param>
        /// <returns>An <see cref = "Individual" /></returns>
        Individual GetIndividual(int id, int treeId);

        /// <summary>
        /// Retrieves all the individuals for a particular tree
        /// </summary>
        /// <param name="treeId">The Id of the tree</param>
        /// <returns>A collection of <see cref = "Individual" objects /></returns>
        IEnumerable<Individual> GetIndividuals(int treeId);

        /// <summary>
        /// Gets a page of individuals based on a Search Term
        /// </summary>
        /// <param name="treeId">The portalId</param>
        /// <param name="predicate">The search term to use</param>
        /// <param name="pageIndex">The page index to return</param>
        /// <param name="pageSize">The page size</param>
        /// <returns>content type collection.</returns>
        IPagedList<Individual> GetIndividuals(int treeId, Func<Individual, bool> predicate, int pageIndex, int pageSize);

        /// <summary>
        ///   Updates an individual in the data store.
        /// </summary>
        /// <param name = "individual">The individual to update in the data store.</param>
        void UpdateIndividual(Individual individual);
    }
}
