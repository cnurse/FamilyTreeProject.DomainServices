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
    public class CitationServiceTests
    {
        private CitationService _service;
        private Mock<IUnitOfWork> _mockUnitOfWork;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Test]
        public void CitationService_Constructor_Throws_If_UnitOfWork_Argument_Is_Null()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new CitationService(null));
        }

        [Test]
        public void CitationService_Add_Throws_On_Null_Citation()
        {
            //Arrange
            _service = new CitationService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Add(null));
        }

        [Test]
        public void CitationService_Add_Calls_Repository_Add_Method_With_The_Same_Citation_Object_It_Recieved()
        {
            // Create test data
            var newCitation = new Citation
                                    {
                                        Text = "Foo",
                                        Page = "Bar"
                                    };

            //Create Mock
            var mockRepository = new Mock<IRepository<Citation>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Citation>()).Returns(mockRepository.Object);

            //Arrange
            _service = new CitationService(_mockUnitOfWork.Object);

            //Act
            _service.Add(newCitation);

            //Assert
            mockRepository.Verify(r => r.Add(newCitation));
        }

        [Test]
        public void CitationService_Add_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newCitation = new Citation
                                    {
                                        Text = "Foo",
                                        Page = "Bar"
                                    };

            //Create Mock
            var mockRepository = new Mock<IRepository<Citation>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Citation>()).Returns(mockRepository.Object);

            //Arrange
            _service = new CitationService(_mockUnitOfWork.Object);

            //Act
            _service.Add(newCitation);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        [Test]
        public void CitationService_Delete_Throws_On_Null_Citation()
        {
            //Arrange
            _service = new CitationService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(null));
        }

        [Test]
        public void CitationService_Delete_Calls_Repsoitory_Delete_Method_With_The_Same_Citation_Object_It_Recieved()
        {
            // Create test data
            var newCitation = new Citation
                                    {
                                        Text = "Foo",
                                        Page = "Bar"
                                    };

            //Create Mock
            var mockRepository = new Mock<IRepository<Citation>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Citation>()).Returns(mockRepository.Object);

            //Arrange
            _service = new CitationService(_mockUnitOfWork.Object);

            //Act
            _service.Delete(newCitation);

            //Assert
            mockRepository.Verify(r => r.Delete(newCitation));
        }

        [Test]
        public void CitationService_Delete_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newCitation = new Citation
                                    {
                                        Text = "Foo",
                                        Page = "Bar"
                                    };

            //Create Mock
            var mockRepository = new Mock<IRepository<Citation>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Citation>()).Returns(mockRepository.Object);

            //Arrange
            _service = new CitationService(_mockUnitOfWork.Object);

            //Act
            _service.Delete(newCitation);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        [Test]
        public void CitationService_Get_Throws_On_Negative_TreeId()
        {
            //Arrange
            _service = new CitationService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(It.IsAny<int>(), -1));
        }

        [Test]
        public void CitationService_Get_Throws_On_Negative_Id()
        {
            //Arrange
            _service = new CitationService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1, It.IsAny<int>()));
        }

        [Test]
        public void CitationService_Get_Calls_Repository_Get()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Citation>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<Citation>()).Returns(mockRepository.Object);

            _service = new CitationService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            _service.Get(id, It.IsAny<int>());

            //Assert
            mockRepository.Verify(r => r.Get(It.IsAny<int>()));
        }

        [Test]
        public void CitationService_Get_Returns_Citation_On_Valid_Id()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Citation>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetCitations(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Citation>()).Returns(mockRepository.Object);

            _service = new CitationService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            var citation = _service.Get(id, It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<Citation>(citation);
        }

        [Test]
        public void CitationService_Get_Returns_Null_On_InValid_Id()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Citation>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetCitations(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Citation>()).Returns(mockRepository.Object);

            _service = new CitationService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_NotFound;

            //Act
            var citation = _service.Get(id, It.IsAny<int>());

            //Assert
            Assert.IsNull(citation);
        }

        [Test]
        public void CitationService_Get_Overload_Throws_On_Negative_TreeId()
        {
            //Arrange
            _service = new CitationService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1));
        }

        [Test]
        public void CitationService_Get_Overload_Calls_Repository_Get()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Citation>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<Citation>()).Returns(mockRepository.Object);

            _service = new CitationService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            _service.Get(treeId);

            //Assert
            mockRepository.Verify(r => r.Get(It.IsAny<int>()));
        }

        [Test]
        public void CitationService_Get_Overload_Returns_List_Of_Citations()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Citation>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetCitations(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Citation>()).Returns(mockRepository.Object);

            _service = new CitationService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            var citations = _service.Get(treeId);

            //Assert
            Assert.IsInstanceOf<IEnumerable<Citation>>(citations);
            Assert.AreEqual(TestConstants.PAGE_TotalCount, citations.Count());
        }

        [Test]
        public void CitationService_Get_ByPage_Overload_Throws_On_Negative_TreeId()
        {
            //Arrange
            _service = new CitationService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1, t => true, 0, TestConstants.PAGE_RecordCount));
        }

        [Test]
        public void CitationService_Get_ByPage_Overload_Calls_Repository_Get()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Citation>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<Citation>()).Returns(mockRepository.Object);

            _service = new CitationService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            _service.Get(treeId, t => true, 0, TestConstants.PAGE_RecordCount);

            //Assert
            mockRepository.Verify(r => r.Get(It.IsAny<int>()));
        }

        [Test]
        public void CitationService_Get_ByPage_Overload_Returns_PagedList_Of_Citations()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Citation>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetCitations(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Citation>()).Returns(mockRepository.Object);

            _service = new CitationService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            var citations = _service.Get(treeId, t => true, 0, TestConstants.PAGE_RecordCount);

            //Assert
            Assert.IsInstanceOf<IPagedList<Citation>>(citations);
            Assert.AreEqual(TestConstants.PAGE_TotalCount, citations.TotalCount);
            Assert.AreEqual(TestConstants.PAGE_RecordCount, citations.PageSize);
        }

        [Test]
        public void CitationService_Update_Throws_On_Null_Citation()
        {
            //Arrange
            _service = new CitationService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(null));
        }

        [Test]
        public void CitationService_Update_Calls_Repository_Update_Method_With_The_Same_Citation_Object_It_Recieved()
        {
            // Create test data
            var citation = new Citation
                                {
                                    Id = TestConstants.ID_Exists,
                                    Text = "Foo",
                                    Page = "Bar"
                                };

            //Create Mock
            var mockRepository = new Mock<IRepository<Citation>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Citation>()).Returns(mockRepository.Object);

            //Arrange
            _service = new CitationService(_mockUnitOfWork.Object);

            //Act
            _service.Update(citation);

            //Assert
            mockRepository.Verify(r => r.Update(citation));
        }

        [Test]
        public void CitationService_Update_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var citation = new Citation
                                {
                                    Id = TestConstants.ID_Exists,
                                    Text = "Foo",
                                    Page = "Bar"
                                };

            //Create Mock
            var mockRepository = new Mock<IRepository<Citation>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Citation>()).Returns(mockRepository.Object);

            //Arrange
            _service = new CitationService(_mockUnitOfWork.Object);

            //Act
            _service.Update(citation);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        private static IQueryable<Citation> GetCitations(int count)
        {
            var citations = new List<Citation>();

            for (int i = 0; i < count; i++)
            {
                citations.Add(new Citation
                                    {
                                        Id = i,
                                        Text = String.Format(TestConstants.CIT_Text, i),
                                        Page = String.Format(TestConstants.CIT_Page, i),
                                        TreeId = TestConstants.TREE_Id
                                    });
            }

            return citations.AsQueryable();
        }
    }
}
