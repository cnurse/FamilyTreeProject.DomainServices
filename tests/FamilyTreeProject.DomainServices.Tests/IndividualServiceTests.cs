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
using System.Linq.Expressions;
using FamilyTreeProject.TestUtilities;
using Moq;
using Naif.Data;
using NUnit.Framework;

namespace FamilyTreeProject.DomainServices.Tests
{
    [TestFixture]
    public class IndividualServiceTests
    {
        private IndividualService service;

        #region Constructor Tests

        [Test]
        public void IndividualService_Constructor_Throws_If_UnitOfWork_Argument_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new IndividualService(null));
        }

        #endregion

        #region Tests

        #region AddIndividual Tests

        [Test]
        public void IndividualService_AddIndividual_Throws_On_Null_Individual()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            service = new IndividualService(mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => service.AddIndividual(null));
        }

        [Test]
        public void IndividualService_AddIndividual_Calls_Repsoitory_Add_Method_With_The_Same_Individual_Object_It_Recieved()
        {
            // Create test data
            var newIndividual = new Individual
            {
                FirstName = "Foo",
                LastName = "Bar"
            };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Individual>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Individual>()).Returns(mockRepository.Object);

            //Arrange
            service = new IndividualService(mockUnitOfWork.Object);

            //Act
            service.AddIndividual(newIndividual);

            //Assert
            mockRepository.Verify(r => r.Add(newIndividual));
        }

        [Test]
        public void IndividualService_AddIndividual_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newIndividual = new Individual
            {
                FirstName = "Foo",
                LastName = "Bar"
            };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Individual>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Individual>()).Returns(mockRepository.Object);

            //Arrange
            service = new IndividualService(mockUnitOfWork.Object);

            //Act
            service.AddIndividual(newIndividual);

            //Assert
            mockUnitOfWork.Verify(db => db.Commit());
        }

        #endregion

        #region DeleteIndividual Tests

        [Test]
        public void IndividualService_DeleteIndividual_Throws_On_Null_Individual()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            service = new IndividualService(mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => service.DeleteIndividual(null));
        }

        [Test]
        public void IndividualService_DeleteIndividual_Calls_Repsoitory_Delete_Method_With_The_Same_Individual_Object_It_Recieved()
        {
            // Create test data
            var newIndividual = new Individual
            {
                FirstName = "Foo",
                LastName = "Bar"
            };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Individual>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Individual>()).Returns(mockRepository.Object);

            //Arrange
            service = new IndividualService(mockUnitOfWork.Object);

            //Act
            service.DeleteIndividual(newIndividual);

            //Assert
            mockRepository.Verify(r => r.Delete(newIndividual));
        }

        [Test]
        public void IndividualService_DeleteIndividual_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newIndividual = new Individual
            {
                FirstName = "Foo",
                LastName = "Bar"
            };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Individual>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Individual>()).Returns(mockRepository.Object);

            //Arrange
            service = new IndividualService(mockUnitOfWork.Object);

            //Act
            service.DeleteIndividual(newIndividual);

            //Assert
            mockUnitOfWork.Verify(db => db.Commit());
        }

        #endregion

        #region GetChildren Tests

        [Test]
        public void IndividualService_GetChildren_Throws_On_Negative_ParentId()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            service = new IndividualService(mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => service.GetChildren(-1));
        }

        [Test]
        public void IndividualService_GetChildren_Calls_Repository_Find()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Individual>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Individual>()).Returns(mockRepository.Object);
            service = new IndividualService(mockUnitOfWork.Object);

            //Act
            service.GetChildren(TestConstants.ID_ParentId);

            //Assert
            mockRepository.Verify(r => r.Find(It.IsAny<Expression<Func<Individual, bool>>>()));
        }

        [Test]
        public void IndividualService_GetChildren_Returns_Children_On_Valid_ParentId()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Individual>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Individual>()).Returns(mockRepository.Object);
            mockRepository.Setup(r => r.Find(It.IsAny<Expression<Func<Individual, bool>>>()))
                            .Returns(GetIndividuals());
            service = new IndividualService(mockUnitOfWork.Object);

            //Act
            var children = service.GetChildren(TestConstants.ID_ParentId);

            //Assert
            Assert.IsTrue(children.Count > 0);
        }

        [Test]
        public void IndividualService_GetChildren_Returns_EmptyList_On_InValid_ParentId()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Individual>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Individual>()).Returns(mockRepository.Object);
            mockRepository.Setup(r => r.GetAll())
                            .Returns(GetIndividuals());
            service = new IndividualService(mockUnitOfWork.Object);

            //Act
            var children = service.GetChildren(TestConstants.ID_InvalidParentId);

            //Assert
            Assert.IsTrue(children.Count == 0);
        }

        #endregion

        #region GetIndividual Tests

        [Test]
        public void IndividualService_GetIndividual_Throws_On_Negative_Id()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            service = new IndividualService(mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => service.GetIndividual(-1, false));
        }

        [Test]
        public void IndividualService_GetIndividual_Calls_Repsoitory_GetAll()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Individual>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Individual>()).Returns(mockRepository.Object);
            service = new IndividualService(mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            service.GetIndividual(id, false);

            //Assert
            mockRepository.Verify(r => r.GetAll());
        }

        [Test]
        public void IndividualService_GetIndividual_Returns_Individual_On_Valid_Id()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Individual>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Individual>()).Returns(mockRepository.Object);
            mockRepository.Setup(r => r.GetAll())
                            .Returns(GetIndividuals());
            service = new IndividualService(mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            var individual = service.GetIndividual(id, false);

            //Assert
            Assert.IsInstanceOf<Individual>(individual);
        }

        [Test]
        public void IndividualService_GetIndividual_Returns_Null_On_InValid_Id()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Individual>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Individual>()).Returns(mockRepository.Object);
            mockRepository.Setup(r => r.GetAll())
                            .Returns(GetIndividuals());
            service = new IndividualService(mockUnitOfWork.Object);
            const int id = TestConstants.ID_NotFound;

            //Act
            var individual = service.GetIndividual(id, false);

            //Assert
            Assert.IsNull(individual);
        }

        [Test]
        [TestCase(TestConstants.ID_ParentId, true, 2)]
        [TestCase(TestConstants.ID_ParentId, false, 0)]
        public void IndividualService_GetIndividual_Returns_Children(int parentID, bool includeChildren, int childrenCount)
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Individual>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Individual>()).Returns(mockRepository.Object);
            mockRepository.Setup(r => r.Find(It.IsAny<Expression<Func<Individual, bool>>>()))
                            .Returns(GetIndividuals().Where(ind => ind.FatherId == parentID || ind.MotherId == parentID));
            mockRepository.Setup(r => r.GetAll())
                            .Returns(GetIndividuals());
            service = new IndividualService(mockUnitOfWork.Object);

            //Act
            var individual = service.GetIndividual(parentID, includeChildren);

            //Assert
            Assert.AreEqual(childrenCount, individual.Children.Count);
        }

        #endregion

        #region GetIndividuals Tests

        [Test]
        public void IndividualService_GetIndividuals_Throws_On_Negative_TreeId()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            service = new IndividualService(mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => service.GetIndividuals(-1));
        }

        [Test]
        public void IndividualService_GetIndividuals_Calls_Repsoitory_Find()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Individual>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Individual>()).Returns(mockRepository.Object);
            service = new IndividualService(mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            service.GetIndividuals(treeId);

            //Assert
            mockRepository.Verify(r => r.Find(It.IsAny<Expression<Func<Individual, bool>>>()));
        }

        #endregion

        #region GetChildren Tests

        [Test]
        public void IndividualService_GetSpouses_Throws_On_Negative_Id()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            service = new IndividualService(mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => service.GetSpouses(-1));
        }

        [Test]
        public void IndividualService_GetSpouses_Calls_FamilyRepoitory_GetAll()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Family>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Family>()).Returns(mockRepository.Object);
            service = new IndividualService(mockUnitOfWork.Object);

            //Act
            service.GetSpouses(TestConstants.ID_Exists);

            //Assert
            mockRepository.Verify(r => r.Find(It.IsAny<Expression<Func<Family, bool>>>()));
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 1)]
        [TestCase(6, 0)]
        public void IndividualService_GetSpouses_Returns_Correct_No_Of_Spouses(int individualId, int expectedCount)
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockIndividualRepository = new Mock<ILinqRepository<Individual>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Individual>()).Returns(mockIndividualRepository.Object);
            mockIndividualRepository.Setup(r => r.GetAll())
                                        .Returns(GetSpouses());

            var mockFamilyRepository = new Mock<ILinqRepository<Family>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Family>()).Returns(mockFamilyRepository.Object);
            mockFamilyRepository.Setup(r => r.Find(It.IsAny<Expression<Func<Family, bool>>>()))
                                        .Returns(GetFamilies().Where(f => f.HusbandId == individualId || f.WifeId == individualId));

            service = new IndividualService(mockUnitOfWork.Object);

            //Act
            var spouses = service.GetSpouses(individualId);

            //Assert
            Assert.AreEqual(expectedCount, spouses.Count);
        }

        [Test]
        [TestCase(1, 0, 3)]
        [TestCase(2, 0, 4)]
        [TestCase(3, 0, 1)]
        [TestCase(4, 0, 2)]
        [TestCase(2, 1, 5)]
        public void IndividualService_GetSpouses_Returns_Correct_Spouses(int individualId, int index, int expectedId)
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockIndividualRepository = new Mock<ILinqRepository<Individual>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Individual>()).Returns(mockIndividualRepository.Object);
            mockIndividualRepository.Setup(r => r.GetAll())
                                        .Returns(GetSpouses());

            var mockFamilyRepository = new Mock<ILinqRepository<Family>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Family>()).Returns(mockFamilyRepository.Object);
            mockFamilyRepository.Setup(r => r.Find(It.IsAny<Expression<Func<Family, bool>>>()))
                                        .Returns(GetFamilies().Where(f => f.HusbandId == individualId || f.WifeId == individualId));

            service = new IndividualService(mockUnitOfWork.Object);

            //Act
            var spouses = service.GetSpouses(individualId);

            //Assert
            Assert.AreEqual(expectedId, spouses[index].Id);
        }

        #endregion

        #region UpdateIndividual Tests

        [Test]
        public void IndividualService_UpdateIndividual_Throws_On_Null_Individual()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            service = new IndividualService(mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => service.AddIndividual(null));
        }

        [Test]
        public void IndividualService_UpdateIndividual_Calls_Repository_Update_Method_With_The_Same_Individual_Object_It_Recieved()
        {
            // Create test data
            var individual = new Individual { Id = TestConstants.ID_Exists, FirstName = "Foo", LastName = "Bar" };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Individual>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Individual>()).Returns(mockRepository.Object);

            //Arrange
            service = new IndividualService(mockUnitOfWork.Object);

            //Act
            service.UpdateIndividual(individual);

            //Assert
            mockRepository.Verify(r => r.Update(individual));
        }

        [Test]
        public void IndividualService_UpdateIndividual_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var individual = new Individual { Id = TestConstants.ID_Exists, FirstName = "Foo", LastName = "Bar" };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Individual>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Individual>()).Returns(mockRepository.Object);

            //Arrange
            service = new IndividualService(mockUnitOfWork.Object);

            //Act
            service.UpdateIndividual(individual);

            //Assert
            mockUnitOfWork.Verify(db => db.Commit());
        }

        #endregion

        #endregion

        #region Helper Methods

        private static IQueryable<Family> GetFamilies()
        {
            var families = new List<Family> {
                                        new Family {Id = 1, HusbandId = 1, WifeId = 3},
                                        new Family {Id = 2, HusbandId = 2, WifeId = 4},
                                        new Family {Id = 3, HusbandId = 2, WifeId = 5}
                                    };
            return families.AsQueryable();
        }

        private static IQueryable<Individual> GetSpouses()
        {
            var individuals = new List<Individual>
                                  {
                                      new Individual { Id = 1, FirstName = "John", LastName = "Doe", Sex = Common.Sex.Male },
                                      new Individual { Id = 2, FirstName = "Jim", LastName = "Smith", Sex = Common.Sex.Male },
                                      new Individual { Id = 3, FirstName = "Jane", LastName = "Williams", Sex = Common.Sex.Female },
                                      new Individual { Id = 4, FirstName = "Mary", LastName = "Evans", Sex = Common.Sex.Female },
                                      new Individual { Id = 5, FirstName = "Betty", LastName = "Taylor", Sex = Common.Sex.Female },
                                      new Individual { Id = 6, FirstName = "Michael", LastName = "Smith", Sex = Common.Sex.Male }
                                  };
            return individuals.AsQueryable();
        }

        private static IQueryable<Individual> GetIndividuals()
        {
            var individuals = new List<Individual>();

            for (int i = 0; i < TestConstants.PAGE_TotalCount; i++)
            {
                individuals.Add(new Individual
                {
                    Id = i,
                    FirstName = String.Format(TestConstants.IND_FirstName, i),
                    LastName = (i <= TestConstants.IND_LastNameCount) ? TestConstants.IND_LastName : TestConstants.IND_AltLastName,
                    TreeId = TestConstants.TREE_Id,
                    FatherId = (i < 5 && i > 2) ? TestConstants.ID_FatherId : -1,
                    MotherId = (i < 5 && i > 2) ? TestConstants.ID_MotherId : -1
                });
            }

            return individuals.AsQueryable();
        }

        #endregion
    }
}
