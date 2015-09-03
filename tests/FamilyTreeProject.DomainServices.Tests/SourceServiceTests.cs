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
    public class SourceServiceTests
    {
        private SourceService _service;
        private Mock<IUnitOfWork> _mockUnitOfWork;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Test]
        public void SourceService_Constructor_Throws_If_UnitOfWork_Argument_Is_Null()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new SourceService(null));
        }

        [Test]
        public void SourceService_Add_Throws_On_Null_Source()
        {
            //Arrange
            _service = new SourceService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Add(null));
        }

        [Test]
        public void SourceService_Add_Calls_Source_Add_Method_With_The_Same_Source_Object_It_Recieved()
        {
            // Create test data
            var newSource = new Source
                                    {
                                        Author = "Foo",
                                        Title = "Bar"
                                    };

            //Create Mock
            var mockRepository = new Mock<IRepository<Source>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Source>()).Returns(mockRepository.Object);

            //Arrange
            _service = new SourceService(_mockUnitOfWork.Object);

            //Act
            _service.Add(newSource);

            //Assert
            mockRepository.Verify(r => r.Add(newSource));
        }

        [Test]
        public void SourceService_Add_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newSource = new Source
                                    {
                                        Author = "Foo",
                                        Title = "Bar"
                                    };

            //Create Mock
            var mockRepository = new Mock<IRepository<Source>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Source>()).Returns(mockRepository.Object);

            //Arrange
            _service = new SourceService(_mockUnitOfWork.Object);

            //Act
            _service.Add(newSource);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        [Test]
        public void SourceService_Delete_Throws_On_Null_Source()
        {
            //Arrange
            _service = new SourceService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(null));
        }

        [Test]
        public void SourceService_Delete_Calls_Repsoitory_Delete_Method_With_The_Same_Source_Object_It_Recieved()
        {
            // Create test data
            var newSource = new Source
                                        {
                                            Author = "Foo",
                                            Title = "Bar"
                                        };

            //Create Mock
            var mockRepository = new Mock<IRepository<Source>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Source>()).Returns(mockRepository.Object);

            //Arrange
            _service = new SourceService(_mockUnitOfWork.Object);

            //Act
            _service.Delete(newSource);

            //Assert
            mockRepository.Verify(r => r.Delete(newSource));
        }

        [Test]
        public void SourceService_Delete_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newSource = new Source
                                        {
                                            Author = "Foo",
                                            Title = "Bar"
                                        };

            //Create Mock
            var mockRepository = new Mock<IRepository<Source>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Source>()).Returns(mockRepository.Object);

            //Arrange
            _service = new SourceService(_mockUnitOfWork.Object);

            //Act
            _service.Delete(newSource);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        [Test]
        public void SourceService_Get_Throws_On_Negative_TreeId()
        {
            //Arrange
            _service = new SourceService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(It.IsAny<int>(), -1));
        }

        [Test]
        public void SourceService_Get_Throws_On_Negative_Id()
        {
            //Arrange
            _service = new SourceService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1, It.IsAny<int>()));
        }

        [Test]
        public void SourceService_Get_Calls_Source_Get()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Source>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<Source>()).Returns(mockRepository.Object);

            _service = new SourceService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            _service.Get(id, It.IsAny<int>());

            //Assert
            mockRepository.Verify(r => r.Get(It.IsAny<int>()));
        }

        [Test]
        public void SourceService_Get_Returns_Source_On_Valid_Id()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Source>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetSources(TestConstants.PAGE_NotFound));
            _mockUnitOfWork.Setup(u => u.GetRepository<Source>()).Returns(mockRepository.Object);

            _service = new SourceService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            var individual = _service.Get(id, It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<Source>(individual);
        }

        [Test]
        public void SourceService_Get_Returns_Null_On_InValid_Id()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Source>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetSources(TestConstants.PAGE_NotFound));
            _mockUnitOfWork.Setup(u => u.GetRepository<Source>()).Returns(mockRepository.Object);

            _service = new SourceService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_NotFound;

            //Act
            var individual = _service.Get(id, It.IsAny<int>());

            //Assert
            Assert.IsNull(individual);
        }

        [Test]
        public void SourceService_Get_Overload_Throws_On_Negative_TreeId()
        {
            //Arrange
            _service = new SourceService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1));
        }

        [Test]
        public void SourceService_Get_Overload_Calls_Repository_Get()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Source>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<Source>()).Returns(mockRepository.Object);

            _service = new SourceService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            _service.Get(treeId);

            //Assert
            mockRepository.Verify(r => r.Get(It.IsAny<int>()));
        }

        [Test]
        public void SourceService_Get_Overload_Returns_List_Of_Sources()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Source>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetSources(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Source>()).Returns(mockRepository.Object);

            _service = new SourceService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            var sources = _service.Get(treeId);

            //Assert
            Assert.IsInstanceOf<IEnumerable<Source>>(sources);
            Assert.AreEqual(TestConstants.PAGE_TotalCount, sources.Count());
        }

        [Test]
        public void SourceService_Get_ByPage_Overload_Throws_On_Negative_TreeId()
        {
            //Arrange
            _service = new SourceService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1, t => true, 0, TestConstants.PAGE_RecordCount));
        }

        [Test]
        public void SourceService_Get_ByPage_Overload_Calls_Repository_Get()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Source>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<Source>()).Returns(mockRepository.Object);

            _service = new SourceService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            _service.Get(treeId, t => true, 0, TestConstants.PAGE_RecordCount);

            //Assert
            mockRepository.Verify(r => r.Get(It.IsAny<int>()));
        }

        [Test]
        public void SourceService_Get_ByPage_Overload_Returns_PagedList_Of_Sources()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Source>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetSources(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Source>()).Returns(mockRepository.Object);

            _service = new SourceService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            var sources = _service.Get(treeId, t => true, 0, TestConstants.PAGE_RecordCount);

            //Assert
            Assert.IsInstanceOf<IPagedList<Source>>(sources);
            Assert.AreEqual(TestConstants.PAGE_TotalCount, sources.TotalCount);
            Assert.AreEqual(TestConstants.PAGE_RecordCount, sources.PageSize);
        }

        [Test]
        public void SourceService_Update_Throws_On_Null_Source()
        {
            //Arrange
            _service = new SourceService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(null));
        }

        [Test]
        public void SourceService_Update_Calls_Source_Update_Method_With_The_Same_Source_Object_It_Recieved()
        {
            // Create test data
            var individual = new Source { Id = TestConstants.ID_Exists, Author = "Foo", Title = "Bar" };

            //Create Mock
            var mockRepository = new Mock<IRepository<Source>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Source>()).Returns(mockRepository.Object);

            //Arrange
            _service = new SourceService(_mockUnitOfWork.Object);

            //Act
            _service.Update(individual);

            //Assert
            mockRepository.Verify(r => r.Update(individual));
        }

        [Test]
        public void SourceService_Update_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var individual = new Source { Id = TestConstants.ID_Exists, Author = "Foo", Title = "Bar" };

            //Create Mock
            var mockRepository = new Mock<IRepository<Source>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Source>()).Returns(mockRepository.Object);

            //Arrange
            _service = new SourceService(_mockUnitOfWork.Object);

            //Act
            _service.Update(individual);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        private static IQueryable<Source> GetSources(int count)
        {
            var individuals = new List<Source>();

            for (int i = 0; i < count; i++)
            {
                individuals.Add(new Source
                {
                    Id = i,
                    Author = String.Format(TestConstants.SRC_Author, i),
                    Title = String.Format(TestConstants.SRC_Title, i),
                    TreeId = TestConstants.TREE_Id
                });
            }

            return individuals.AsQueryable();
        }
    }
}
