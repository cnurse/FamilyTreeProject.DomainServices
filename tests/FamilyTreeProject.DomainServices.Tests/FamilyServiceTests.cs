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
    public class FamilyServiceTests
    {
        private FamilyService _service;
        private Mock<IUnitOfWork> _mockUnitOfWork;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Test]
        public void FamilyService_Constructor_Throws_If_UnitOfWork_Argument_Is_Null()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new FamilyService(null));
        }

        [Test]
        public void FamilyService_Add_Throws_On_Null_Family()
        {
            //Arrange
            _service = new FamilyService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Add(null));
        }

        [Test]
        public void FamilyService_Add_Calls_Repository_Add_Method_With_The_Same_Family_Object_It_Recieved()
        {
            // Create test data
            var newFamily = new Family
                                {
                                    WifeId = TestConstants.ID_WifeId,
                                    HusbandId = TestConstants.ID_HusbandId
                                };

            //Create Mock
            var mockRepository = new Mock<IRepository<Family>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Family>()).Returns(mockRepository.Object);

            //Arrange
            _service = new FamilyService(_mockUnitOfWork.Object);

            //Act
            _service.Add(newFamily);

            //Assert
            mockRepository.Verify(r => r.Add(newFamily));
        }

        [Test]
        public void FamilyService_Add_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newFamily = new Family
                                    {
                                        WifeId = TestConstants.ID_WifeId,
                                        HusbandId = TestConstants.ID_HusbandId
                                    };

            //Create Mock
            var mockRepository = new Mock<IRepository<Family>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Family>()).Returns(mockRepository.Object);

            //Arrange
            _service = new FamilyService(_mockUnitOfWork.Object);

            //Act
            _service.Add(newFamily);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        [Test]
        public void FamilyService_Delete_Throws_On_Null_Family()
        {
            //Arrange
            _service = new FamilyService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(null));
        }

        [Test]
        public void FamilyService_Delete_Calls_Repsoitory_Delete_Method_With_The_Same_Family_Object_It_Recieved()
        {
            // Create test data
            var newFamily = new Family
                                    {
                                        WifeId = TestConstants.ID_WifeId,
                                        HusbandId = TestConstants.ID_HusbandId
                                    };

            //Create Mock
            var mockRepository = new Mock<IRepository<Family>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Family>()).Returns(mockRepository.Object);

            //Arrange
            _service = new FamilyService(_mockUnitOfWork.Object);

            //Act
            _service.Delete(newFamily);

            //Assert
            mockRepository.Verify(r => r.Delete(newFamily));
        }

        [Test]
        public void FamilyService_Delete_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newFamily = new Family
                                    {
                                        WifeId = TestConstants.ID_WifeId,
                                        HusbandId = TestConstants.ID_HusbandId
                                    };

            //Create Mock
            var mockRepository = new Mock<IRepository<Family>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Family>()).Returns(mockRepository.Object);

            //Arrange
            _service = new FamilyService(_mockUnitOfWork.Object);

            //Act
            _service.Delete(newFamily);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        [Test]
        public void FamilyService_Get_Throws_On_Negative_TreeId()
        {
            //Arrange
            _service = new FamilyService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(It.IsAny<int>(), -1));
        }

        [Test]
        public void FamilyService_Get_Throws_On_Negative_Id()
        {
            //Arrange
            _service = new FamilyService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1, It.IsAny<int>()));
        }

        [Test]
        public void FamilyService_Get_Calls_Repository_Get()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Family>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<Family>()).Returns(mockRepository.Object);

            _service = new FamilyService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            _service.Get(id, It.IsAny<int>());

            //Assert
            mockRepository.Verify(r => r.Get(It.IsAny<int>()));
        }

        [Test]
        public void FamilyService_Get_Returns_Family_On_Valid_Id()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Family>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetFamilys(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Family>()).Returns(mockRepository.Object);

            _service = new FamilyService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            var family = _service.Get(id, It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<Family>(family);
        }

        [Test]
        public void FamilyService_Get_Returns_Null_On_InValid_Id()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Family>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetFamilys(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Family>()).Returns(mockRepository.Object);

            _service = new FamilyService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_NotFound;

            //Act
            var family = _service.Get(id, It.IsAny<int>());

            //Assert
            Assert.IsNull(family);
        }

        [Test]
        public void FamilyService_Get_Overload_Throws_On_Negative_TreeId()
        {
            //Arrange
            _service = new FamilyService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1));
        }

        [Test]
        public void FamilyService_Get_Overload_Calls_Repository_Get()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Family>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<Family>()).Returns(mockRepository.Object);

            _service = new FamilyService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            _service.Get(treeId);

            //Assert
            mockRepository.Verify(r => r.Get(It.IsAny<int>()));
        }

        [Test]
        public void FamilyService_Get_Overload_Returns_List_Of_Familys()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Family>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetFamilys(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Family>()).Returns(mockRepository.Object);

            _service = new FamilyService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            var familys = _service.Get(treeId);

            //Assert
            Assert.IsInstanceOf<IEnumerable<Family>>(familys);
            Assert.AreEqual(TestConstants.PAGE_TotalCount, familys.Count());
        }

        [Test]
        public void FamilyService_Get_ByPage_Overload_Throws_On_Negative_TreeId()
        {
            //Arrange
            _service = new FamilyService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1, t => true, 0, TestConstants.PAGE_RecordCount));
        }

        [Test]
        public void FamilyService_Get_ByPage_Overload_Calls_Repository_Get()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Family>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<Family>()).Returns(mockRepository.Object);

            _service = new FamilyService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            _service.Get(treeId, t => true, 0, TestConstants.PAGE_RecordCount);

            //Assert
            mockRepository.Verify(r => r.Get(It.IsAny<int>()));
        }

        [Test]
        public void FamilyService_Get_ByPage_Overload_Returns_PagedList_Of_Familys()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Family>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetFamilys(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Family>()).Returns(mockRepository.Object);

            _service = new FamilyService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            var familys = _service.Get(treeId, t => true, 0, TestConstants.PAGE_RecordCount);

            //Assert
            Assert.IsInstanceOf<IPagedList<Family>>(familys);
            Assert.AreEqual(TestConstants.PAGE_TotalCount, familys.TotalCount);
            Assert.AreEqual(TestConstants.PAGE_RecordCount, familys.PageSize);
        }


        [Test]
        public void FamilyService_Update_Throws_On_Null_Family()
        {
            //Arrange
            _service = new FamilyService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(null));
        }

        [Test]
        public void FamilyService_Update_Calls_Repository_Update_Method_With_The_Same_Family_Object_It_Recieved()
        {
            // Create test data
            var family = new Family
                                    {
                                        Id = TestConstants.ID_Exists,
                                        WifeId = TestConstants.ID_WifeId,
                                        HusbandId = TestConstants.ID_HusbandId
                                    };

            //Create Mock
            var mockRepository = new Mock<IRepository<Family>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Family>()).Returns(mockRepository.Object);

            //Arrange
            _service = new FamilyService(_mockUnitOfWork.Object);

            //Act
            _service.Update(family);

            //Assert
            mockRepository.Verify(r => r.Update(family));
        }

        [Test]
        public void FamilyService_Update_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var family = new Family
                                    {
                                        Id = TestConstants.ID_Exists,
                                        WifeId = TestConstants.ID_WifeId,
                                        HusbandId = TestConstants.ID_HusbandId
                                    };

            //Create Mock
            var mockRepository = new Mock<IRepository<Family>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Family>()).Returns(mockRepository.Object);

            //Arrange
            _service = new FamilyService(_mockUnitOfWork.Object);

            //Act
            _service.Update(family);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        private static IQueryable<Family> GetFamilys(int count)
        {
            var familys = new List<Family>();

            for (int i = 0; i < count; i++)
            {
                familys.Add(new Family
                                    {
                                        Id = i,
                                        WifeId = (i < 5 && i > 2) ? TestConstants.ID_WifeId : -1,
                                        HusbandId = (i < 5 && i > 2) ? TestConstants.ID_HusbandId : -1,
                                        TreeId = TestConstants.TREE_Id
                                    });
            }

            return familys.AsQueryable();
        }
    }
}
