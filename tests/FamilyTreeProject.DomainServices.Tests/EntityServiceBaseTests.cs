using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FamilyTreeProject.Common.Data;
using FamilyTreeProject.Common.Models;
using FamilyTreeProject.DomainServices.Common;
using FamilyTreeProject.DomainServices.Tests.Common;
using Moq;
using Naif.Core.Collections;
using NUnit.Framework;
// ReSharper disable UnusedVariable

namespace FamilyTreeProject.DomainServices.Tests
{
    public abstract class EntityServiceBaseTests<TEntity, TService> where TEntity : Entity where TService : IEntityService<TEntity>
    {
        private TService _service;
        private Mock<IUnitOfWork> _mockUnitOfWork;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var repository = new Mock<IRepository<TEntity>>();
            repository.Setup(r => r.SupportsAggregates).Returns(true);
            _mockUnitOfWork.Setup(u => u.GetRepository<TEntity>()).Returns(repository.Object);
        }

        [Test]
        public void Service_Constructor_Throws_If_UnitOfWork_Argument_Is_Null()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<TargetInvocationException>(() => CreateService(null));
        }

        [Test]
        public void Service_Add_Throws_On_Null_Entity()
        {
            //Arrange
            _service = CreateService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Add(null));
        }

        [Test]
        public void Service_Add_Calls_Entity_Add_Method_With_The_Same_Entity_It_Recieved()
        {
            // Create test data
            var newEntity = NewEntity();

            //Create Mock
            var mockRepository = SetUpRepository(_mockUnitOfWork);

            //Arrange
            _service = CreateService(_mockUnitOfWork.Object);

            //Act
            _service.Add(newEntity);

            //Assert
            mockRepository.Verify(r => r.Add(newEntity));
        }

        [Test]
        public void Service_Add_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newEntity = NewEntity();

            //Create Mock
            var mockRepository = SetUpRepository(_mockUnitOfWork);

            //Arrange
            _service = CreateService(_mockUnitOfWork.Object);

            //Act
            _service.Add(newEntity);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        [Test]
        public void Service_Delete_Throws_On_Null_Source()
        {
            //Arrange
            _service = CreateService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(null));
        }

        [Test]
        public void Service_Delete_Calls_Repsoitory_Delete_Method_With_The_Same_Entity_It_Recieved()
        {
            // Create test data
            var newEntity = NewEntity();

            //Create Mock
            var mockRepository = SetUpRepository(_mockUnitOfWork);

            //Arrange
            _service = CreateService(_mockUnitOfWork.Object);

            //Act
            _service.Delete(newEntity);

            //Assert
            mockRepository.Verify(r => r.Delete(newEntity));
        }

        [Test]
        public void Service_Delete_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newEntity = NewEntity();

            //Create Mock
            var mockRepository = SetUpRepository(_mockUnitOfWork);

            //Arrange
            _service = CreateService(_mockUnitOfWork.Object);

            //Act
            _service.Delete(newEntity);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        [Test]
        public void Service_Get_Throws_On_Null_TreeId()
        {
            //Arrange
            _service = CreateService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentException>(() => _service.Get(It.IsAny<int>(), String.Empty));
        }

        [Test]
        public void Service_Get_Throws_On_Null_Id()
        {
            //Arrange
            _service = CreateService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1, It.IsAny<string>()));
        }

        [Test]
        public void Service_Get_Calls_Source_Get()
        {
            //Arrange
            var mockRepository = SetUpRepository(_mockUnitOfWork);

            _service = CreateService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            _service.Get(id, TestConstants.TREE_Id);

            //Assert
            VerifyFind(mockRepository);
        }

        [Test]
        public void Service_Get_Returns_Entity_On_Valid_Id()
        {
            //Arrange
            var mockRepository = SetUpRepository(_mockUnitOfWork, GetEntities, TestConstants.PAGE_TotalCount);

            _service = CreateService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            var individual = _service.Get(id, TestConstants.TREE_Id);

            //Assert
            Assert.IsInstanceOf<TEntity>(individual);
        }

        [Test]
        public void Service_Get_Returns_Null_On_InValid_Id()
        {
            //Arrange
            var mockRepository = SetUpRepository(_mockUnitOfWork, GetEntities, TestConstants.PAGE_NotFound);

            _service = CreateService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_NotFound;

            //Act
            var individual = _service.Get(id, TestConstants.TREE_Id);

            //Assert
            Assert.IsNull(individual);
        }

        [Test]
        public void Service_Get_Overload_Throws_On_Null_TreeId()
        {
            //Arrange
            _service = CreateService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentException>(() => _service.Get(String.Empty));
        }

        [Test]
        public void Service_Get_Overload_Calls_Repository_Get()
        {
            //Arrange
            var mockRepository = SetUpRepository(_mockUnitOfWork);

            _service = CreateService(_mockUnitOfWork.Object);
            string treeId = TestConstants.TREE_Id;

            //Act
            _service.Get(treeId);

            //Assert
            VerifyFind(mockRepository);
        }

        [Test]
        public void Service_Get_Overload_Returns_List_Of_Entities()
        {
            //Arrange
            var mockRepository = SetUpRepository(_mockUnitOfWork, GetEntities, TestConstants.PAGE_TotalCount);

            _service = CreateService(_mockUnitOfWork.Object);
            string treeId = TestConstants.TREE_Id;

            //Act
            var sources = _service.Get(treeId);

            //Assert
            Assert.IsInstanceOf<IEnumerable<TEntity>>(sources);
            Assert.AreEqual(TestConstants.PAGE_TotalCount, sources.Count());
        }

        [Test]
        public void Service_Get_ByPage_Overload_Throws_On_Null_TreeId()
        {
            //Arrange
            _service = CreateService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentException>(() => _service.Get(String.Empty, t => true, 0, TestConstants.PAGE_RecordCount));
        }

        [Test]
        public void Service_Get_ByPage_Overload_Calls_Repository_Find()
        {
            //Arrange
            var mockRepository = SetUpRepository(_mockUnitOfWork);

            _service = CreateService(_mockUnitOfWork.Object);
            string treeId = TestConstants.TREE_Id;

            //Act
            _service.Get(treeId, t => true, 0, TestConstants.PAGE_RecordCount);

            //Assert
            VerifyFind(mockRepository);
        }

        [Test]
        public void Service_Get_ByPage_Overload_Returns_PagedList_Of_Sources()
        {
            //Arrange
            var mockRepository = SetUpRepository(_mockUnitOfWork, GetEntities, TestConstants.PAGE_TotalCount);

            _service = CreateService(_mockUnitOfWork.Object);
            string treeId = TestConstants.TREE_Id;

            //Act
            var sources = _service.Get(treeId, t => true, 0, TestConstants.PAGE_RecordCount);

            //Assert
            Assert.IsInstanceOf<IPagedList<TEntity>>(sources);
            Assert.AreEqual(TestConstants.PAGE_TotalCount, sources.TotalCount);
            Assert.AreEqual(TestConstants.PAGE_RecordCount, sources.PageSize);
        }

        [Test]
        public void Service_Update_Throws_On_Null_Entity()
        {
            //Arrange
            _service = CreateService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(null));
        }

        [Test]
        public void Service_Update_Calls_Source_Update_Method_With_The_Same_Entity_It_Recieved()
        {
            // Create test data
            var entity = UpdateEntity();

            //Create Mock
            var mockRepository = SetUpRepository(_mockUnitOfWork);

            //Arrange
            _service = CreateService(_mockUnitOfWork.Object);

            //Act
            _service.Update(entity);

            //Assert
            mockRepository.Verify(r => r.Update(entity));
        }

        [Test]
        public void Service_Update_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var entity = UpdateEntity();

            //Create Mock
            var mockRepository = SetUpRepository(_mockUnitOfWork);

            //Arrange
            _service = CreateService(_mockUnitOfWork.Object);

            //Act
            _service.Update(entity);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }


        protected TService CreateService(IUnitOfWork unitOfWork)
        {
            return (TService)Activator.CreateInstance(typeof(TService), new object[] { unitOfWork });
        }

        protected abstract TEntity NewEntity();

        protected abstract IEnumerable<TEntity> GetEntities(int count);

        protected abstract TEntity UpdateEntity();

        private Mock<IRepository<TEntity>> SetUpRepository(Mock<IUnitOfWork> mockUnitOfWork)
        {
            var mockRepository = new Mock<IRepository<TEntity>>();
            mockUnitOfWork.Setup(u => u.GetRepository<TEntity>()).Returns(mockRepository.Object);

            return mockRepository;
        }

        private Mock<IRepository<TEntity>> SetUpRepository(Mock<IUnitOfWork> mockUnitOfWork, Func<int, IEnumerable<TEntity>> getEntities, int entityCount)
        {
            var mockRepository = new Mock<IRepository<TEntity>>();
            mockRepository.Setup(r => r.Find(It.IsAny<Func<TEntity, bool>>())).Returns(getEntities(entityCount));

            mockUnitOfWork.Setup(u => u.GetRepository<TEntity>()).Returns(mockRepository.Object);

            return mockRepository;
        }

        private void VerifyFind(Mock<IRepository<TEntity>> mockRepository)
        {
            mockRepository.Verify(r => r.Find(It.IsAny<Func<TEntity, bool>>()));
        }
    }
}
