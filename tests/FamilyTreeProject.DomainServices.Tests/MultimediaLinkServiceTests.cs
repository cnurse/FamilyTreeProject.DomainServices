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

// ReSharper disable ObjectCreationAsStatement

namespace FamilyTreeProject.DomainServices.Tests
{
    [TestFixture]
    public class MultimediaLinkServiceTests
    {
        private MultimediaLinkService _service;
        private Mock<IUnitOfWork> _mockUnitOfWork;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Test]
        public void MultimediaLinkService_Constructor_Throws_If_UnitOfWork_Argument_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new MultimediaLinkService(null));
        }

        [Test]
        public void MultimediaLinkService_Add_Throws_On_Null_MultimediaLink()
        {
            //Arrange
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _service = new MultimediaLinkService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Add(null));
        }

        [Test]
        public void MultimediaLinkService_Add_Calls_Repsoitory_Add_Method_With_The_Same_MultimediaLink_Object_It_Recieved()
        {
            // Create test data
            var newMultimediaLink = new MultimediaLink
                                            {
                                                File = "Foo"
                                            };

            //Create Mock
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<MultimediaLink>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<MultimediaLink>()).Returns(mockRepository.Object);

            //Arrange
            _service = new MultimediaLinkService(_mockUnitOfWork.Object);

            //Act
            _service.Add(newMultimediaLink);

            //Assert
            mockRepository.Verify(r => r.Add(newMultimediaLink));
        }

        [Test]
        public void MultimediaLinkService_Add_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newMultimediaLink = new MultimediaLink
                                            {
                                                File = "Foo"
                                            };

            //Create Mock
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<MultimediaLink>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<MultimediaLink>()).Returns(mockRepository.Object);

            //Arrange
            _service = new MultimediaLinkService(_mockUnitOfWork.Object);

            //Act
            _service.Add(newMultimediaLink);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        [Test]
        public void MultimediaLinkService_Delete_Throws_On_Null_MultimediaLink()
        {
            //Arrange
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _service = new MultimediaLinkService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(null));
        }

        [Test]
        public void MultimediaLinkService_Delete_Calls_Repsoitory_Delete_Method_With_The_Same_MultimediaLink_Object_It_Recieved()
        {
            // Create test data
            var newMultimediaLink = new MultimediaLink
                                            {
                                                File = "Foo"
                                            };

            //Create Mock
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<MultimediaLink>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<MultimediaLink>()).Returns(mockRepository.Object);

            //Arrange
            _service = new MultimediaLinkService(_mockUnitOfWork.Object);

            //Act
            _service.Delete(newMultimediaLink);

            //Assert
            mockRepository.Verify(r => r.Delete(newMultimediaLink));
        }

        [Test]
        public void MultimediaLinkService_Delete_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newMultimediaLink = new MultimediaLink
                                            {
                                                File = "Foo"
                                            };

            //Create Mock
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<MultimediaLink>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<MultimediaLink>()).Returns(mockRepository.Object);

            //Arrange
            _service = new MultimediaLinkService(_mockUnitOfWork.Object);

            //Act
            _service.Delete(newMultimediaLink);

            //Assert
            _mockUnitOfWork.Verify(d => d.Commit());
        }

        [Test]
        public void MultimediaLinkService_Get_Throws_On_Negative_Id()
        {
            //Arrange
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _service = new MultimediaLinkService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1, It.IsAny<int>()));
        }

        [Test]
        public void MultimediaLinkService_Get_Throws_On_Negative_TreeId()
        {
            //Arrange
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _service = new MultimediaLinkService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(It.IsAny<int>(), -1));
        }

        [Test]
        public void MultimediaLinkService_Get_Calls_Repository_GetAll()
        {
            //Arrange
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<MultimediaLink>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<MultimediaLink>()).Returns(mockRepository.Object);

            _service = new MultimediaLinkService(_mockUnitOfWork.Object);

            const int id = TestConstants.ID_Exists;

            //Act
            _service.Get(id, TestConstants.TREE_Id);

            //Assert
            mockRepository.Verify(r => r.Get(TestConstants.TREE_Id));
        }

        [Test]
        public void MultimediaLinkService_Get_Returns_MultimediaLink_On_Valid_Id()
        {
            //Arrange
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<MultimediaLink>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetMultimediaLinks(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(d => d.GetRepository<MultimediaLink>()).Returns(mockRepository.Object);

            _service = new MultimediaLinkService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            var multimediaLink = _service.Get(id, TestConstants.TREE_Id);

            //Assert
            Assert.IsInstanceOf<MultimediaLink>(multimediaLink);
        }

        [Test]
        public void MultimediaLinkService_Get_Returns_Null_On_InValid_Id()
        {
            //Arrange
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<MultimediaLink>>();
            mockRepository.Setup(r => r.GetAll()).Returns(GetMultimediaLinks(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(d => d.GetRepository<MultimediaLink>()).Returns(mockRepository.Object);

            _service = new MultimediaLinkService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_NotFound;

            //Act
            var multimediaLink = _service.Get(id, TestConstants.TREE_Id);

            //Assert
            Assert.IsNull(multimediaLink);
        }

        [Test]
        public void MultimediaLinkService_Get_Overload_Throws_On_Negative_TreeId()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            _service = new MultimediaLinkService(mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1));
        }

        [Test]
        public void MultimediaLinkService_Get_Overload_Calls_Repository_Get()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<MultimediaLink>>();
            mockUnitOfWork.Setup(d => d.GetRepository<MultimediaLink>()).Returns(mockRepository.Object);
            _service = new MultimediaLinkService(mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            _service.Get(treeId);

            //Assert
            mockRepository.Verify(r => r.Get(treeId));
        }

        [Test]
        public void MultimediaLinkService_Get_Overload_Returns_List_Of_MultimediaLinks()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<MultimediaLink>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetMultimediaLinks(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<MultimediaLink>()).Returns(mockRepository.Object);

            _service = new MultimediaLinkService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            var multimediaLinks = _service.Get(treeId);

            //Assert
            Assert.IsInstanceOf<IEnumerable<MultimediaLink>>(multimediaLinks);
            Assert.AreEqual(TestConstants.PAGE_TotalCount, multimediaLinks.Count());
        }

        [Test]
        public void MultimediaLinkService_Get_ByPage_Overload_Throws_On_Negative_TreeId()
        {
            //Arrange
            _service = new MultimediaLinkService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1, t => true, 0, TestConstants.PAGE_RecordCount));
        }

        [Test]
        public void MultimediaLinkService_Get_ByPage_Overload_Calls_Repository_Get()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<MultimediaLink>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<MultimediaLink>()).Returns(mockRepository.Object);

            _service = new MultimediaLinkService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            _service.Get(treeId, t => true, 0, TestConstants.PAGE_RecordCount);

            //Assert
            mockRepository.Verify(r => r.Get(It.IsAny<int>()));
        }

        [Test]
        public void MultimediaLinkService_Get_ByPage_Overload_Returns_PagedList_Of_MultimediaLinks()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<MultimediaLink>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetMultimediaLinks(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<MultimediaLink>()).Returns(mockRepository.Object);

            _service = new MultimediaLinkService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            var multimediaLinks = _service.Get(treeId, t => true, 0, TestConstants.PAGE_RecordCount);

            //Assert
            Assert.IsInstanceOf<IPagedList<MultimediaLink>>(multimediaLinks);
            Assert.AreEqual(TestConstants.PAGE_TotalCount, multimediaLinks.TotalCount);
            Assert.AreEqual(TestConstants.PAGE_RecordCount, multimediaLinks.PageSize);
        }

        [Test]
        public void MultimediaLinkService_Update_Throws_On_Null_MultimediaLink()
        {
            //Arrange
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            //Arrange
            _service = new MultimediaLinkService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Add(null));
        }

        [Test]
        public void MultimediaLinkService_Update_Calls_Repository_Update_Method_With_The_Same_MultimediaLink_Object_It_Recieved()
        {
            // Create test data
            var multimediaLink = new MultimediaLink
                                        {
                                            File = "Foo"
                                        };

            //Create Mock
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<MultimediaLink>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<MultimediaLink>()).Returns(mockRepository.Object);

            //Arrange
            _service = new MultimediaLinkService(_mockUnitOfWork.Object);

            //Act
            _service.Update(multimediaLink);

            //Assert
            mockRepository.Verify(r => r.Update(multimediaLink));
        }

        [Test]
        public void MultimediaLinkService_Update_Calls_UnitOfWork_Commit_Method_()
        {
            // Create test data
            var multimediaLink = new MultimediaLink
                                        {
                                            File = "Foo"
                                        };

            //Create Mock
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<MultimediaLink>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<MultimediaLink>()).Returns(mockRepository.Object);

            //Arrange
            _service = new MultimediaLinkService(_mockUnitOfWork.Object);

            //Act
            _service.Update(multimediaLink);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        private static IQueryable<MultimediaLink> GetMultimediaLinks(int count)
        {
            var multimediaLinks = new List<MultimediaLink>();

            for (int i = 0; i < count; i++)
            {
                multimediaLinks.Add(new MultimediaLink
                                {
                                    Id = i,
                                    File = "Foo",
                                    TreeId = TestConstants.TREE_Id,
                                });
            }

            return multimediaLinks.AsQueryable();
        }
    }
}
