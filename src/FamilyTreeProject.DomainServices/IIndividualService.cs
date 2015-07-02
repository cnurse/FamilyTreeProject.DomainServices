//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System.Collections.Generic;

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
        ///   Retrieves all the children of an Individual
        /// </summary>
        /// <param name = "parentId">The Id of the Parent</param>
        /// <returns>An <see cref = "IList{T}" />.</returns>
        IList<Individual> GetChildren(int parentId);

        /// <summary>
        ///   Retrieves a single Individual
        /// </summary>
        /// <param name = "id">The Id of the Indiidual to retrieve</param>
        /// <param name = "includeChildren">A flag that indicates whether to get the children of the Individual</param>
        /// <returns>An <see cref = "Individual" /></returns>
        Individual GetIndividual(int id, bool includeChildren);

        IEnumerable<Individual> GetIndividuals(int treeId);

        /// <summary>
        ///   Retrieves all the spouses of an Individual
        /// </summary>
        /// <param name = "individualId">The Id of the Individual</param>
        /// <returns>An <see cref = "IList{Individual}" />.</returns>
        IList<Individual> GetSpouses(int individualId);

        /// <summary>
        ///   Updates an individual in the data store.
        /// </summary>
        /// <param name = "individual">The individual to update in the data store.</param>
        void UpdateIndividual(Individual individual);
    }
}
