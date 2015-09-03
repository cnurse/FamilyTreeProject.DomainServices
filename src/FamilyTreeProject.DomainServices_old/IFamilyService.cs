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
    public interface IFamilyService
    {
        /// <summary>
        ///   Adds a family to the data store and sets the <see cref = "Family.Id" /> property
        ///   of <paramref name = "newFamily" /> to the id of the new family.
        /// </summary>
        /// <param name = "newFamily">The family to add to the data store.</param>
        void AddFamily(Family newFamily);

        /// <summary>
        ///   Deletes a family from the data store
        /// </summary>
        /// <remarks>
        ///   The delete operation takes effect immediately
        /// </remarks>
        /// <param name = "family">The family to delete</param>
        void DeleteFamily(Family family);

        /// <summary>
        /// Retrieves a single Family
        /// </summary>
        /// <param name = "id">The Id of the Family to retrieve</param>
        /// <param name = "treeId">The Id of the Tree</param>
        /// <returns>An <see cref = "Family" /></returns>
        Family GetFamily(int id, int treeId);

        /// <summary>
        /// Retrieves all the Families
        /// </summary>
        /// <param name = "treeId">The Id of the Tree</param>
        /// <returns>A collection ofn <see cref = "Family" /> objects</returns>
        IEnumerable<Family> GetFamilies(int treeId);

        /// <summary>
        ///   Updates a family in the data store.
        /// </summary>
        /// <param name = "family">The family to update in the data store.</param>
        void UpdateFamily(Family family);
    }
}
