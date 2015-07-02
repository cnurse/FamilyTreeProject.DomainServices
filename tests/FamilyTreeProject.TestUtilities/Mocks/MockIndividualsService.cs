using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FamilyTreeProject.Collections;
using FamilyTreeProject.Data;

namespace FamilyTreeProject.Tests.Utilities.Mocks
{
    public class MockIndividualsService : IIndividualsService
    {

        #region Private Members

        private List<Individual> individuals;

        #endregion

        #region Constructors

        public MockIndividualsService()
        {
            individuals = new List<Individual>();

            for (int i = TestConstants.PAGE_TotalCount -1; i >= 0; i--)
            {
                individuals.Add(new Individual()
                                    {
                                        Id = TestConstants.PAGE_TotalCount - 1 - i,
                                        FirstName = String.Format(TestConstants.IND_FirstName, i),
                                        LastName = (i < 5) ? TestConstants.IND_LastName : TestConstants.IND_AltLastName,
                                        TreeId = TestConstants.TREE_Id,
                                        FatherId = (TestConstants.PAGE_TotalCount - 1 - i < 5 && TestConstants.PAGE_TotalCount - 1 - i > 2) ? TestConstants.ID_FatherId : -1,
                                        MotherId = (TestConstants.PAGE_TotalCount - 1 - i < 5 && TestConstants.PAGE_TotalCount - 1 - i > 2) ? TestConstants.ID_MotherId : -1
                                    });
            }
        }

        #endregion
        #region IIndividualsService Members

        /// <summary>
        /// Adds an individual to the data store and sets the <see cref="Individual.Id"/> property
        /// of <paramref name="newIndividual"/> to the id of the new individual.
        /// </summary>
        /// <param name="newIndividual">The individual to add to the data store.</param>
        public void AddIndividual(Individual newIndividual)
        {
            newIndividual.Id = TestConstants.ID_New;
        }

        /// <summary>
        /// Deletes an individual from the data store
        /// </summary>
        /// <remarks>
        /// The delete operation takes effect immediately
        /// </remarks>
        /// <param name="individual">The individual to delete</param>
        public void DeleteIndividual(Individual individual)
        {
            //Do nothing;
        }

        /// <summary>
        /// Retrieves all the children of an Individual
        /// </summary>
        /// <param name="parentId">The Id of the Parent</param>
        /// <returns>An <see cref="IList{Individual}"/>.</returns>
        public IList<Individual> GetChildren(int parentId)
        {
            return individuals.AsQueryable()
                                .Where(ind => ind.FatherId == parentId || ind.MotherId == parentId)
                                .ToList();
        }

        /// <summary>
        /// Retrieves all the children of a Couple
        /// </summary>
        /// <param name="fatherId">The Id of the Father</param>
        /// <param name="motherId">The Id of the Mother</param>
        /// <returns>An <see cref="IList{Individual}"/>.</returns>
        public IList<Individual> GetChildren(int fatherId, int motherId)
        {
            return individuals.AsQueryable()
                                .Where(ind => ind.FatherId == fatherId && ind.MotherId == motherId)
                                .ToList();
        }

        /// <summary>
        /// Retrieves a single individual
        /// </summary>
        /// <param name="id">The Id of the Indiidual to retrieve</param>
        /// <param name="includeChildren">A flag that indicates whether to get the children of the Individual</param>
        /// <returns>An <see cref="Individual"/></returns>
        public Individual GetIndividual(int id, bool includeChildren)
        {
            switch (id)
            {
                case TestConstants.ID_Exists:
                    return new Individual() { Id = id, FirstName = String.Format(TestConstants.IND_FirstName, id), LastName = TestConstants.IND_LastName };
                default:
                    return null;
            }
        }

        /// <summary>
        /// Retrieves all the Individuals in a Tree
        /// </summary>
        /// <param name="treeId">The Id of the Tree</param>
        /// <returns>An <see cref="IList{Individual}"/> of <see cref="Individual"/> objects.</returns>
        public IList<Individual> GetIndividuals(int treeId)
        {
            return individuals;
        }

        /// <summary>
        /// Retrieves a single page of Individuals in a Tree
        /// </summary>
        /// <param name="treeId">The Id of the Tree</param>
        /// <param name="pageNumber">The page number (0-indexed)</param>
        /// <param name="pageSize">The size of the page</param>
        /// <returns>A <see cref="IPagedList{T}"/> of  <see cref="Individual"/> objects.</returns>
        public IPagedList<Individual> GetIndividuals(int treeId, int pageNumber, int pageSize)
        {
            return individuals.AsQueryable().InPagesOf(pageSize).GetPage(pageNumber);
        }

        /// <summary>
        /// Updates an individual in the data store.
        /// </summary>
        /// <param name="individual">The individual to update in the data store.</param>
        public void UpdateIndividual(Individual individual)
        {
            //Do nothing;
        }

        #endregion
    }
}
