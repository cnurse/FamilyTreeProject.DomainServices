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
    public class FactServiceTests
    {
        private FactService _service;
        private Mock<IUnitOfWork> _mockUnitOfWork;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Test]
        public void FactService_Constructor_Throws_If_UnitOfWork_Argument_Is_Null()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new FactService(null));
        }

        [Test]
        public void FactService_Add_Throws_On_Null_Fact()
        {
            //Arrange
            _service = new FactService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Add(null));
        }

        [Test]
        public void FactService_Add_Calls_Repository_Add_Method_With_The_Same_Fact_Object_It_Recieved()
        {
            // Create test data
            var newFact = new Fact
            {
                Date = "Foo",
                Place = "Bar"
            };

            //Create Mock
            var mockRepository = new Mock<IRepository<Fact>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Fact>()).Returns(mockRepository.Object);

            //Arrange
            _service = new FactService(_mockUnitOfWork.Object);

            //Act
            _service.Add(newFact);

            //Assert
            mockRepository.Verify(r => r.Add(newFact));
        }

        [Test]
        public void FactService_Add_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newFact = new Fact
            {
                Date = "Foo",
                Place = "Bar"
            };

            //Create Mock
            var mockRepository = new Mock<IRepository<Fact>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Fact>()).Returns(mockRepository.Object);

            //Arrange
            _service = new FactService(_mockUnitOfWork.Object);

            //Act
            _service.Add(newFact);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        [Test]
        public void FactService_Delete_Throws_On_Null_Fact()
        {
            //Arrange
            _service = new FactService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(null));
        }

        [Test]
        public void FactService_Delete_Calls_Repsoitory_Delete_Method_With_The_Same_Fact_Object_It_Recieved()
        {
            // Create test data
            var newFact = new Fact
            {
                Date = "Foo",
                Place = "Bar"
            };

            //Create Mock
            var mockRepository = new Mock<IRepository<Fact>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Fact>()).Returns(mockRepository.Object);

            //Arrange
            _service = new FactService(_mockUnitOfWork.Object);

            //Act
            _service.Delete(newFact);

            //Assert
            mockRepository.Verify(r => r.Delete(newFact));
        }

        [Test]
        public void FactService_Delete_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newFact = new Fact
            {
                Date = "Foo",
                Place = "Bar"
            };

            //Create Mock
            var mockRepository = new Mock<IRepository<Fact>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Fact>()).Returns(mockRepository.Object);

            //Arrange
            _service = new FactService(_mockUnitOfWork.Object);

            //Act
            _service.Delete(newFact);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        [Test]
        public void FactService_Get_Throws_On_Negative_TreeId()
        {
            //Arrange
            _service = new FactService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(It.IsAny<int>(), -1));
        }

        [Test]
        public void FactService_Get_Throws_On_Negative_Id()
        {
            //Arrange
            _service = new FactService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1, It.IsAny<int>()));
        }

        [Test]
        public void FactService_Get_Calls_Repository_Get()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Fact>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<Fact>()).Returns(mockRepository.Object);

            _service = new FactService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            _service.Get(id, It.IsAny<int>());

            //Assert
            mockRepository.Verify(r => r.Get(It.IsAny<int>()));
        }

        [Test]
        public void FactService_Get_Returns_Fact_On_Valid_Id()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Fact>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetFacts(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Fact>()).Returns(mockRepository.Object);

            _service = new FactService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            var fact = _service.Get(id, It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<Fact>(fact);
        }

        [Test]
        public void FactService_Get_Returns_Null_On_InValid_Id()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Fact>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetFacts(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Fact>()).Returns(mockRepository.Object);

            _service = new FactService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_NotFound;

            //Act
            var fact = _service.Get(id, It.IsAny<int>());

            //Assert
            Assert.IsNull(fact);
        }

        [Test]
        public void FactService_Get_Overload_Throws_On_Negative_TreeId()
        {
            //Arrange
            _service = new FactService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1));
        }

        [Test]
        public void FactService_Get_Overload_Calls_Repository_Get()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Fact>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<Fact>()).Returns(mockRepository.Object);

            _service = new FactService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            _service.Get(treeId);

            //Assert
            mockRepository.Verify(r => r.Get(It.IsAny<int>()));
        }

        [Test]
        public void FactService_Get_Overload_Returns_List_Of_Facts()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Fact>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetFacts(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Fact>()).Returns(mockRepository.Object);

            _service = new FactService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            var facts = _service.Get(treeId);

            //Assert
            Assert.IsInstanceOf<IEnumerable<Fact>>(facts);
            Assert.AreEqual(TestConstants.PAGE_TotalCount, facts.Count());
        }

        [Test]
        public void FactService_Get_ByPage_Overload_Throws_On_Negative_TreeId()
        {
            //Arrange
            _service = new FactService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1, t => true, 0, TestConstants.PAGE_RecordCount));
        }

        [Test]
        public void FactService_Get_ByPage_Overload_Calls_Repository_Get()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Fact>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<Fact>()).Returns(mockRepository.Object);

            _service = new FactService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            _service.Get(treeId, t => true, 0, TestConstants.PAGE_RecordCount);

            //Assert
            mockRepository.Verify(r => r.Get(It.IsAny<int>()));
        }

        [Test]
        public void FactService_Get_ByPage_Overload_Returns_PagedList_Of_Facts()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Fact>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetFacts(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Fact>()).Returns(mockRepository.Object);

            _service = new FactService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            var facts = _service.Get(treeId, t => true, 0, TestConstants.PAGE_RecordCount);

            //Assert
            Assert.IsInstanceOf<IPagedList<Fact>>(facts);
            Assert.AreEqual(TestConstants.PAGE_TotalCount, facts.TotalCount);
            Assert.AreEqual(TestConstants.PAGE_RecordCount, facts.PageSize);
        }

        [Test]
        public void FactService_Update_Throws_On_Null_Fact()
        {
            //Arrange
            _service = new FactService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(null));
        }

        [Test]
        public void FactService_Update_Calls_Repository_Update_Method_With_The_Same_Fact_Object_It_Recieved()
        {
            // Create test data
            var fact = new Fact { Id = TestConstants.ID_Exists, Date = "Foo", Place = "Bar" };

            //Create Mock
            var mockRepository = new Mock<IRepository<Fact>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Fact>()).Returns(mockRepository.Object);

            //Arrange
            _service = new FactService(_mockUnitOfWork.Object);

            //Act
            _service.Update(fact);

            //Assert
            mockRepository.Verify(r => r.Update(fact));
        }

        [Test]
        public void FactService_Update_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var fact = new Fact { Id = TestConstants.ID_Exists, Date = "Foo", Place = "Bar" };

            //Create Mock
            var mockRepository = new Mock<IRepository<Fact>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Fact>()).Returns(mockRepository.Object);

            //Arrange
            _service = new FactService(_mockUnitOfWork.Object);

            //Act
            _service.Update(fact);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        private static IQueryable<Fact> GetFacts(int count)
        {
            var facts = new List<Fact>();

            for (int i = 0; i < count; i++)
            {
                facts.Add(new Fact
                {
                    Id = i,
                    Date = String.Format(TestConstants.EVN_Date, i),
                    Place = String.Format(TestConstants.EVN_Place, i),
                    TreeId = TestConstants.TREE_Id
                });
            }

            return facts.AsQueryable();
        }
    }
}
