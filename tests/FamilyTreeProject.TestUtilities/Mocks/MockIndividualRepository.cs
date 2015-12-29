using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FamilyTreeProject.Collections;
using FamilyTreeProject.Data;

namespace FamilyTreeProject.Tests.Utilities.Mocks
{
    public class MockIndividualRepository : IRepository<Individual>
    {

        #region Private Members

        private List<Individual> individuals;

        #endregion

        #region Constructors

        public MockIndividualRepository()
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

        #region IRepository<Individual> Members

        public IQueryable<Individual> GetAll()
        {
            return individuals.AsQueryable();
        }

        public PagedList<Individual> GetPaged(int pageIndex, int pageSize)
        {
            return new PagedList<Individual>(GetAll(), pageIndex, pageSize);
        }

        public IQueryable<Individual> Find(Expression<System.Func<Individual, bool>> expression)
        {
            return GetAll().Where(expression);
        }

        public void Add(Individual item)
        {
            item.Id = TestConstants.ID_New;
            individuals.Add(item);
        }

        public void Delete(Individual item)
        {
            individuals.Remove(item);
        }

        public void Delete(Expression<System.Func<Individual, bool>> expression)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Individual item)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
