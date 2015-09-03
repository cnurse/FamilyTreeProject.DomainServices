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
using Naif.Core.Collections;
using Naif.Data;
using NUnit.Framework;

namespace FamilyTreeProject.DomainServices.Tests
{
    [TestFixture]
    public class IndividualServiceTests
    {
        private IndividualService _service;
        private Mock<IUnitOfWork> _mockUnitOfWork;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Test]
        public void IndividualService_Constructor_Throws_If_UnitOfWork_Argument_Is_Null()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new IndividualService(null));
        }

        [Test]
        public void IndividualService_Add_Throws_On_Null_Individual()
        {
            //Arrange
            _service = new IndividualService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Add(null));
        }

        [Test]
        public void IndividualService_Add_Calls_Repository_Add_Method_With_The_Same_Individual_Object_It_Recieved()
        {
            // Create test data
            var newIndividual = new Individual
                                    {
                                        FirstName = "Foo",
                                        LastName = "Bar"
                                    };

            //Create Mock
            var mockRepository = new Mock<IRepository<Individual>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Individual>()).Returns(mockRepository.Object);

            //Arrange
            _service = new IndividualService(_mockUnitOfWork.Object);

            //Act
            _service.Add(newIndividual);

            //Assert
            mockRepository.Verify(r => r.Add(newIndividual));
        }

        [Test]
        public void IndividualService_Add_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newIndividual = new Individual
                                    {
                                        FirstName = "Foo",
                                        LastName = "Bar"
                                    };

            //Create Mock
            var mockRepository = new Mock<IRepository<Individual>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Individual>()).Returns(mockRepository.Object);

            //Arrange
            _service = new IndividualService(_mockUnitOfWork.Object);

            //Act
            _service.Add(newIndividual);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        [Test]
        public void IndividualService_Delete_Throws_On_Null_Individual()
        {
            //Arrange
            _service = new IndividualService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(null));
        }

        [Test]
        public void IndividualService_Delete_Calls_Repsoitory_Delete_Method_With_The_Same_Individual_Object_It_Recieved()
        {
            // Create test data
            var newIndividual = new Individual
                                        {
                                            FirstName = "Foo",
                                            LastName = "Bar"
                                        };

            //Create Mock
            var mockRepository = new Mock<IRepository<Individual>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Individual>()).Returns(mockRepository.Object);

            //Arrange
            _service = new IndividualService(_mockUnitOfWork.Object);

            //Act
            _service.Delete(newIndividual);

            //Assert
            mockRepository.Verify(r => r.Delete(newIndividual));
        }

        [Test]
        public void IndividualService_Delete_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newIndividual = new Individual
                                        {
                                            FirstName = "Foo",
                                            LastName = "Bar"
                                        };

            //Create Mock
            var mockRepository = new Mock<IRepository<Individual>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Individual>()).Returns(mockRepository.Object);

            //Arrange
            _service = new IndividualService(_mockUnitOfWork.Object);

            //Act
            _service.Delete(newIndividual);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        [Test]
        public void IndividualService_Get_Throws_On_Negative_TreeId()
        {
            //Arrange
            _service = new IndividualService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(It.IsAny<int>(), -1));
        }

        [Test]
        public void IndividualService_Get_Throws_On_Negative_Id()
        {
            //Arrange
            _service = new IndividualService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1, It.IsAny<int>()));
        }

        [Test]
        public void IndividualService_Get_Calls_Repository_Get()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Individual>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<Individual>()).Returns(mockRepository.Object);

            _service = new IndividualService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            _service.Get(id, It.IsAny<int>());

            //Assert
            mockRepository.Verify(r => r.Get(It.IsAny<int>()));
        }

        [Test]
        public void IndividualService_Get_Returns_Individual_On_Valid_Id()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Individual>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetIndividuals(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Individual>()).Returns(mockRepository.Object);

            _service = new IndividualService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            var individual = _service.Get(id, It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<Individual>(individual);
        }

        [Test]
        public void IndividualService_Get_Returns_Null_On_InValid_Id()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Individual>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetIndividuals(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Individual>()).Returns(mockRepository.Object);

            _service = new IndividualService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_NotFound;

            //Act
            var individual = _service.Get(id, It.IsAny<int>());

            //Assert
            Assert.IsNull(individual);
        }

        [Test]
        public void IndividualService_Get_Overload_Throws_On_Negative_TreeId()
        {
            //Arrange
            _service = new IndividualService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1));
        }

        [Test]
        public void IndividualService_Get_Overload_Calls_Repository_Get()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Individual>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<Individual>()).Returns(mockRepository.Object);

            _service = new IndividualService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            _service.Get(treeId);

            //Assert
            mockRepository.Verify(r => r.Get(It.IsAny<int>()));
        }

        [Test]
        public void IndividualService_Get_Overload_Returns_List_Of_Individuals()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Individual>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetIndividuals(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Individual>()).Returns(mockRepository.Object);

            _service = new IndividualService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            var individuals = _service.Get(treeId);

            //Assert
            Assert.IsInstanceOf<IEnumerable<Individual>>(individuals);
            Assert.AreEqual(TestConstants.PAGE_TotalCount, individuals.Count());
        }

        [Test]
        public void IndividualService_Get_ByPage_Overload_Throws_On_Negative_TreeId()
        {
            //Arrange
            _service = new IndividualService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1, t => true, 0, TestConstants.PAGE_RecordCount));
        }

        [Test]
        public void IndividualService_Get_ByPage_Overload_Calls_Repository_Get()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Individual>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<Individual>()).Returns(mockRepository.Object);

            _service = new IndividualService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            _service.Get(treeId, t => true, 0, TestConstants.PAGE_RecordCount);

            //Assert
            mockRepository.Verify(r => r.Get(It.IsAny<int>()));
        }

        [Test]
        public void IndividualService_Get_ByPage_Overload_Returns_PagedList_Of_Individuals()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Individual>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetIndividuals(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Individual>()).Returns(mockRepository.Object);

            _service = new IndividualService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            var individuals = _service.Get(treeId, t => true, 0, TestConstants.PAGE_RecordCount);

            //Assert
            Assert.IsInstanceOf<IPagedList<Individual>>(individuals);
            Assert.AreEqual(TestConstants.PAGE_TotalCount, individuals.TotalCount);
            Assert.AreEqual(TestConstants.PAGE_RecordCount, individuals.PageSize);
        }

        [Test]
        public void IndividualService_Update_Throws_On_Null_Individual()
        {
            //Arrange
            _service = new IndividualService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(null));
        }

        [Test]
        public void IndividualService_Update_Calls_Repository_Update_Method_With_The_Same_Individual_Object_It_Recieved()
        {
            // Create test data
            var individual = new Individual { Id = TestConstants.ID_Exists, FirstName = "Foo", LastName = "Bar" };

            //Create Mock
            var mockRepository = new Mock<IRepository<Individual>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Individual>()).Returns(mockRepository.Object);

            //Arrange
            _service = new IndividualService(_mockUnitOfWork.Object);

            //Act
            _service.Update(individual);

            //Assert
            mockRepository.Verify(r => r.Update(individual));
        }

        [Test]
        public void IndividualService_Update_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var individual = new Individual { Id = TestConstants.ID_Exists, FirstName = "Foo", LastName = "Bar" };

            //Create Mock
            var mockRepository = new Mock<IRepository<Individual>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Individual>()).Returns(mockRepository.Object);

            //Arrange
            _service = new IndividualService(_mockUnitOfWork.Object);

            //Act
            _service.Update(individual);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        private static IQueryable<Individual> GetIndividuals(int count)
        {
            var individuals = new List<Individual>();

            for (int i = 0; i < count; i++)
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
    }
}
