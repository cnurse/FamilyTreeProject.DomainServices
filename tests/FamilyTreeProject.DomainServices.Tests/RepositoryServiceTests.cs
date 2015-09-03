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
using FamilyTreeProject.TestUtilities;
using Moq;
using Naif.Core.Collections;
using Naif.Data;
using NUnit.Framework;

namespace FamilyTreeProject.DomainServices.Tests
{
    [TestFixture]
    public class RepositoryServiceTests
    {
        private RepositoryService _service;
        private Mock<IUnitOfWork> _mockUnitOfWork;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Test]
        public void RepositoryService_Constructor_Throws_If_UnitOfWork_Argument_Is_Null()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new RepositoryService(null));
        }

        [Test]
        public void RepositoryService_Add_Throws_On_Null_Repository()
        {
            //Arrange
            _service = new RepositoryService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Add(null));
        }

        [Test]
        public void RepositoryService_Add_Calls_Repository_Add_Method_With_The_Same_Repository_Object_It_Recieved()
        {
            // Create test data
            var newRepository = new Repository
                                    {
                                        Name = "Foo",
                                        Address = "Bar"
                                    };

            //Create Mock
            var mockRepository = new Mock<IRepository<Repository>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Repository>()).Returns(mockRepository.Object);

            //Arrange
            _service = new RepositoryService(_mockUnitOfWork.Object);

            //Act
            _service.Add(newRepository);

            //Assert
            mockRepository.Verify(r => r.Add(newRepository));
        }

        [Test]
        public void RepositoryService_Add_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newRepository = new Repository
                                    {
                                        Name = "Foo",
                                        Address = "Bar"
                                    };

            //Create Mock
            var mockRepository = new Mock<IRepository<Repository>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Repository>()).Returns(mockRepository.Object);

            //Arrange
            _service = new RepositoryService(_mockUnitOfWork.Object);

            //Act
            _service.Add(newRepository);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        [Test]
        public void RepositoryService_Delete_Throws_On_Null_Repository()
        {
            //Arrange
            _service = new RepositoryService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(null));
        }

        [Test]
        public void RepositoryService_Delete_Calls_Repsoitory_Delete_Method_With_The_Same_Repository_Object_It_Recieved()
        {
            // Create test data
            var newRepository = new Repository
                                        {
                                            Name = "Foo",
                                            Address = "Bar"
                                        };

            //Create Mock
            var mockRepository = new Mock<IRepository<Repository>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Repository>()).Returns(mockRepository.Object);

            //Arrange
            _service = new RepositoryService(_mockUnitOfWork.Object);

            //Act
            _service.Delete(newRepository);

            //Assert
            mockRepository.Verify(r => r.Delete(newRepository));
        }

        [Test]
        public void RepositoryService_Delete_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newRepository = new Repository
                                        {
                                            Name = "Foo",
                                            Address = "Bar"
                                        };

            //Create Mock
            var mockRepository = new Mock<IRepository<Repository>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Repository>()).Returns(mockRepository.Object);

            //Arrange
            _service = new RepositoryService(_mockUnitOfWork.Object);

            //Act
            _service.Delete(newRepository);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        [Test]
        public void RepositoryService_Get_Throws_On_Negative_TreeId()
        {
            //Arrange
            _service = new RepositoryService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(It.IsAny<int>(), -1));
        }

        [Test]
        public void RepositoryService_Get_Throws_On_Negative_Id()
        {
            //Arrange
            _service = new RepositoryService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1, It.IsAny<int>()));
        }

        [Test]
        public void RepositoryService_Get_Calls_Repository_Get()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Repository>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<Repository>()).Returns(mockRepository.Object);

            _service = new RepositoryService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            _service.Get(id, It.IsAny<int>());

            //Assert
            mockRepository.Verify(r => r.Get(It.IsAny<int>()));
        }

        [Test]
        public void RepositoryService_Get_Returns_Repository_On_Valid_Id()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Repository>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetRepositorys(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Repository>()).Returns(mockRepository.Object);

            _service = new RepositoryService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            var individual = _service.Get(id, It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<Repository>(individual);
        }

        [Test]
        public void RepositoryService_Get_Returns_Null_On_InValid_Id()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Repository>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetRepositorys(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Repository>()).Returns(mockRepository.Object);

            _service = new RepositoryService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_NotFound;

            //Act
            var individual = _service.Get(id, It.IsAny<int>());

            //Assert
            Assert.IsNull(individual);
        }

        [Test]
        public void RepositoryService_Get_Overload_Throws_On_Negative_TreeId()
        {
            //Arrange
            _service = new RepositoryService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1));
        }

        [Test]
        public void RepositoryService_Get_Overload_Calls_Repository_Get()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Repository>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<Repository>()).Returns(mockRepository.Object);

            _service = new RepositoryService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            _service.Get(treeId);

            //Assert
            mockRepository.Verify(r => r.Get(It.IsAny<int>()));
        }

        [Test]
        public void RepositoryService_Get_Overload_Returns_List_Of_Repositorys()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Repository>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetRepositorys(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Repository>()).Returns(mockRepository.Object);

            _service = new RepositoryService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            var repositorys = _service.Get(treeId);

            //Assert
            Assert.IsInstanceOf<IEnumerable<Repository>>(repositorys);
            Assert.AreEqual(TestConstants.PAGE_TotalCount, repositorys.Count());
        }

        [Test]
        public void RepositoryService_Get_ByPage_Overload_Throws_On_Negative_TreeId()
        {
            //Arrange
            _service = new RepositoryService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1, t => true, 0, TestConstants.PAGE_RecordCount));
        }

        [Test]
        public void RepositoryService_Get_ByPage_Overload_Calls_Repository_Get()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Repository>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<Repository>()).Returns(mockRepository.Object);

            _service = new RepositoryService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            _service.Get(treeId, t => true, 0, TestConstants.PAGE_RecordCount);

            //Assert
            mockRepository.Verify(r => r.Get(It.IsAny<int>()));
        }

        [Test]
        public void RepositoryService_Get_ByPage_Overload_Returns_PagedList_Of_Repositorys()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Repository>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetRepositorys(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Repository>()).Returns(mockRepository.Object);

            _service = new RepositoryService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            var repositorys = _service.Get(treeId, t => true, 0, TestConstants.PAGE_RecordCount);

            //Assert
            Assert.IsInstanceOf<IPagedList<Repository>>(repositorys);
            Assert.AreEqual(TestConstants.PAGE_TotalCount, repositorys.TotalCount);
            Assert.AreEqual(TestConstants.PAGE_RecordCount, repositorys.PageSize);
        }

        [Test]
        public void RepositoryService_Update_Throws_On_Null_Repository()
        {
            //Arrange
            _service = new RepositoryService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(null));
        }

        [Test]
        public void RepositoryService_Update_Calls_Repository_Update_Method_With_The_Same_Repository_Object_It_Recieved()
        {
            // Create test data
            var individual = new Repository { Id = TestConstants.ID_Exists, Name = "Foo", Address = "Bar" };

            //Create Mock
            var mockRepository = new Mock<IRepository<Repository>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Repository>()).Returns(mockRepository.Object);

            //Arrange
            _service = new RepositoryService(_mockUnitOfWork.Object);

            //Act
            _service.Update(individual);

            //Assert
            mockRepository.Verify(r => r.Update(individual));
        }

        [Test]
        public void RepositoryService_Update_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var individual = new Repository { Id = TestConstants.ID_Exists, Name = "Foo", Address = "Bar" };

            //Create Mock
            var mockRepository = new Mock<IRepository<Repository>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Repository>()).Returns(mockRepository.Object);

            //Arrange
            _service = new RepositoryService(_mockUnitOfWork.Object);

            //Act
            _service.Update(individual);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        private static IQueryable<Repository> GetRepositorys(int count)
        {
            var individuals = new List<Repository>();

            for (int i = 0; i < count; i++)
            {
                individuals.Add(new Repository
                {
                    Id = i,
                    Name = String.Format(TestConstants.REP_Name, i),
                    Address = String.Format(TestConstants.REP_Address, i),
                    TreeId = TestConstants.TREE_Id
                });
            }

            return individuals.AsQueryable();
        }
    }
}
