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
    public class FamilyServiceTests
    {
        private FamilyService service;

        #region Constructor Tests

        [Test]
        public void FamilyService_Constructor_Throws_If_UnitOfWork_Argument_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new FamilyService(null));
        }

        #endregion

        #region Tests

        #region AddFamily Tests

        [Test]
        public void FamilyService_AddFamily_Throws_On_Null_Family()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            service = new FamilyService(mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => service.AddFamily(null));
        }

        [Test]
        public void FamilyService_AddFamily_Calls_Repsoitory_Add_Method_With_The_Same_Family_Object_It_Recieved()
        {
            // Create test data
            var newFamily = new Family
            {
                Husband = new Individual
                {
                    FirstName = "Foo",
                    LastName = "Bar"
                },
                Wife = new Individual
                {
                    FirstName = "Bas",
                    LastName = "Bar"
                }
            };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Family>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Family>()).Returns(mockRepository.Object);

            //Arrange
            service = new FamilyService(mockUnitOfWork.Object);

            //Act
            service.AddFamily(newFamily);

            //Assert
            mockRepository.Verify(r => r.Add(newFamily));
        }

        [Test]
        public void FamilyService_AddFamily_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newFamily = new Family
            {
                Husband = new Individual
                {
                    FirstName = "Foo",
                    LastName = "Bar"
                },
                Wife = new Individual
                {
                    FirstName = "Bas",
                    LastName = "Bar"
                }
            };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Family>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Family>()).Returns(mockRepository.Object);

            //Arrange
            service = new FamilyService(mockUnitOfWork.Object);

            //Act
            service.AddFamily(newFamily);

            //Assert
            mockUnitOfWork.Verify(db => db.Commit());
        }

        #endregion

        #region DeleteFamily Tests

        [Test]
        public void FamilyService_DeleteFamily_Throws_On_Null_Family()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            service = new FamilyService(mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => service.DeleteFamily(null));
        }

        [Test]
        public void FamilyService_DeleteFamily_Calls_Repsoitory_Delete_Method_With_The_Same_Family_Object_It_Recieved()
        {
            // Create test data
            var newFamily = new Family
            {
                Husband = new Individual
                {
                    FirstName = "Foo",
                    LastName = "Bar"
                },
                Wife = new Individual
                {
                    FirstName = "Bas",
                    LastName = "Bar"
                }
            };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Family>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Family>()).Returns(mockRepository.Object);

            //Arrange
            service = new FamilyService(mockUnitOfWork.Object);

            //Act
            service.DeleteFamily(newFamily);

            //Assert
            mockRepository.Verify(r => r.Delete(newFamily));
        }

        [Test]
        public void FamilyService_DeleteFamily_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newFamily = new Family
            {
                Husband = new Individual
                {
                    FirstName = "Foo",
                    LastName = "Bar"
                },
                Wife = new Individual
                {
                    FirstName = "Bas",
                    LastName = "Bar"
                }
            };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Family>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Family>()).Returns(mockRepository.Object);

            //Arrange
            service = new FamilyService(mockUnitOfWork.Object);

            //Act
            service.DeleteFamily(newFamily);

            //Assert
            mockUnitOfWork.Verify(db => db.Commit());
        }

        #endregion

        #region GetFamily Tests

        [Test]
        public void FamilyService_GetFamily_Throws_On_Negative_Id()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            service = new FamilyService(mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => service.GetFamily(-1, false));
        }

        [Test]
        public void FamilyService_GetFamily_Calls_Repository_GetAll()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Family>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Family>()).Returns(mockRepository.Object);
            mockRepository.Setup(r => r.GetAll())
                                .Returns(GetFamilies());
            service = new FamilyService(mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            service.GetFamily(id, false);

            //Assert
            mockRepository.Verify(r => r.GetAll());
        }

        [Test]
        public void FamilyService_GetFamily_Returns_Family_On_Valid_Id()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Family>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Family>()).Returns(mockRepository.Object);
            mockRepository.Setup(r => r.GetAll())
                                .Returns(GetFamilies());
            service = new FamilyService(mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            var family = service.GetFamily(id, false);

            //Assert
            Assert.IsInstanceOf<Family>(family);
        }

        [Test]
        public void FamilyService_GetFamily_Returns_Null_On_InValid_Id()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Family>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Family>()).Returns(mockRepository.Object);
            mockRepository.Setup(r => r.GetAll())
                                .Returns(GetFamilies());
            service = new FamilyService(mockUnitOfWork.Object);
            const int id = TestConstants.ID_NotFound;

            //Act
            var family = service.GetFamily(id, false);

            //Assert
            Assert.IsNull(family);
        }

        #endregion

        #region GetFamilies Tests

        [Test]
        public void FamilyService_GetFamilies_Throws_On_Negative_TreeId()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            service = new FamilyService(mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => service.GetFamilies(-1));
        }

        [Test]
        public void FamilyService_GetFamilies_Calls_Repository_Find()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Family>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Family>()).Returns(mockRepository.Object);
            mockRepository.Setup(r => r.Find(It.IsAny<Expression<Func<Family, bool>>>()))
                                .Returns(GetFamilies());
            service = new FamilyService(mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            service.GetFamilies(treeId);

            //Assert
            mockRepository.Verify(r => r.Find(It.IsAny<Expression<Func<Family, bool>>>()));
        }

        #endregion

        #region UpdateFamily Tests

        [Test]
        public void FamilyService_UpdateFamily_Throws_On_Null_Family()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            service = new FamilyService(mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => service.AddFamily(null));
        }

        [Test]
        public void FamilyService_UpdateFamily_Calls_Repository_Update_Method_With_The_Same_Family_Object_It_Recieved()
        {
            // Create test data
            var family = new Family
            {
                Id = TestConstants.ID_Exists,
                Husband = new Individual
                {
                    FirstName = "Foo",
                    LastName = "Bar"
                },
                Wife = new Individual
                {
                    FirstName = "Bas",
                    LastName = "Bar"
                }
            };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Family>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Family>()).Returns(mockRepository.Object);

            //Arrange
            service = new FamilyService(mockUnitOfWork.Object);

            //Act
            service.UpdateFamily(family);

            //Assert
            mockRepository.Verify(r => r.Update(family));
        }

        [Test]
        public void FamilyService_UpdateFamily_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var family = new Family
            {
                Id = TestConstants.ID_Exists,
                Husband = new Individual
                {
                    FirstName = "Foo",
                    LastName = "Bar"
                },
                Wife = new Individual
                {
                    FirstName = "Bas",
                    LastName = "Bar"
                }
            };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Family>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Family>()).Returns(mockRepository.Object);

            //Arrange
            service = new FamilyService(mockUnitOfWork.Object);

            //Act
            service.UpdateFamily(family);

            //Assert
            mockUnitOfWork.Verify(db => db.Commit());
        }

        #endregion

        #endregion

        #region Helper Methods

        private static IQueryable<Family> GetFamilies()
        {
            var Families = new List<Family>();

            for (int i = 0; i < TestConstants.PAGE_TotalCount; i++)
            {
                Families.Add(new Family
                {
                    Id = i,
                    Husband = new Individual
                    {
                        FirstName = String.Format(TestConstants.IND_FirstName + "_Husband", i),
                        LastName = (i <= TestConstants.IND_LastNameCount) ? TestConstants.IND_LastName : TestConstants.IND_AltLastName
                    },
                    Wife = new Individual
                    {
                        FirstName = String.Format(TestConstants.IND_FirstName + "_Wife", i),
                        LastName = (i <= TestConstants.IND_LastNameCount) ? TestConstants.IND_LastName : TestConstants.IND_AltLastName
                    },
                    TreeId = TestConstants.TREE_Id,
                });
            }

            return Families.AsQueryable();
        }

        #endregion
    }
}
