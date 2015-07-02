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
    public class TreeServiceTests
    {
        private TreeService service;


        [Test]
        public void TreeService_Constructor_Throws_If_UnitOfWork_Argument_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new TreeService(null));
        }

        [Test]
        public void TreeService_AddTree_Throws_On_Null_Tree()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            service = new TreeService(mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => service.AddTree(null));
        }

        [Test]
        public void TreeService_AddTree_Calls_Repsoitory_AddTree_Method_With_The_Same_Tree_Object_It_Recieved()
        {
            // Create test data
            var newTree = new Tree
                                {
                                    Name = "Foo"
                                };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Tree>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Tree>()).Returns(mockRepository.Object);

            //Arrange
            service = new TreeService(mockUnitOfWork.Object);

            //Act
            service.AddTree(newTree);

            //Assert
            mockRepository.Verify(r => r.Add(newTree));
        }

        [Test]
        public void TreeService_AddTree_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newTree = new Tree
            {
                Name = "Foo"
            };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Tree>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Tree>()).Returns(mockRepository.Object);

            //Arrange
            service = new TreeService(mockUnitOfWork.Object);

            //Act
            service.AddTree(newTree);

            //Assert
            mockUnitOfWork.Verify(db => db.Commit());
        }

        [Test]
        public void TreeService_DeleteTree_Throws_On_Null_Tree()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            service = new TreeService(mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => service.DeleteTree(null));
        }

        [Test]
        public void FamilyService_DeleteTree_Calls_Repsoitory_Delete_Method_With_The_Same_Tree_Object_It_Recieved()
        {
            // Create test data
            var newTree = new Tree
            {
                Name = "Foo"
            };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Tree>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Tree>()).Returns(mockRepository.Object);

            //Arrange
            service = new TreeService(mockUnitOfWork.Object);

            //Act
            service.DeleteTree(newTree);

            //Assert
            mockRepository.Verify(r => r.Delete(newTree));
        }

        [Test]
        public void TreeService_DeleteTree_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newTree = new Tree
            {
                Name = "Foo"
            };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Tree>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Tree>()).Returns(mockRepository.Object);

            //Arrange
            service = new TreeService(mockUnitOfWork.Object);

            //Act
            service.DeleteTree(newTree);

            //Assert
            mockUnitOfWork.Verify(d => d.Commit());
        }

        [Test]
        public void TreeService_GetTree_Throws_On_Negative_Id()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            service = new TreeService(mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => service.GetTree(-1));
        }

        [Test]
        public void TreeService_GetTree_Calls_Repository_GetAll()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Tree>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Tree>()).Returns(mockRepository.Object);
            service = new TreeService(mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            service.GetTree(id);

            //Assert
            mockRepository.Verify(r => r.GetAll());
        }

        [Test]
        public void TreeService_GetTree_Returns_Tree_On_Valid_Id()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Tree>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Tree>()).Returns(mockRepository.Object);
            mockRepository.Setup(r => r.GetAll())
                                .Returns(GetTrees());
            service = new TreeService(mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            Tree note = service.GetTree(id);

            //Assert
            Assert.IsInstanceOf<Tree>(note);
        }

        [Test]
        public void TreeService_GetTree_Returns_Null_On_InValid_Id()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Tree>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Tree>()).Returns(mockRepository.Object);
            mockRepository.Setup(r => r.GetAll())
                                .Returns(GetTrees());
            service = new TreeService(mockUnitOfWork.Object);
            const int id = TestConstants.ID_NotFound;

            //Act
            var note = service.GetTree(id);

            //Assert
            Assert.IsNull(note);
        }

        [Test]
        public void TreeService_GetTrees_Calls_Repository_Find()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Tree>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Tree>()).Returns(mockRepository.Object);
            service = new TreeService(mockUnitOfWork.Object);

            //Act
            service.GetTrees();

            //Assert
            mockRepository.Verify(r => r.GetAll());
        }

        [Test]
        public void TreeService_UpdateTree_Throws_On_Null_Tree()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            service = new TreeService(mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => service.AddTree(null));
        }

        [Test]
        public void TreeService_UpdateTree_Calls_Repository_Update_Method_With_The_Same_Tree_Object_It_Recieved()
        {
            // Create test data
            var note = new Tree
            {
                Name = "Foo"
            };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Tree>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Tree>()).Returns(mockRepository.Object);

            //Arrange
            service = new TreeService(mockUnitOfWork.Object);

            //Act
            service.UpdateTree(note);

            //Assert
            mockRepository.Verify(r => r.Update(note));
        }

        [Test]
        public void TreeService_UpdateTree_Calls_UnitOfWork_Commit_Method_()
        {
            // Create test data
            var note = new Tree
            {
                Name = "Foo"
            };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Tree>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Tree>()).Returns(mockRepository.Object);

            //Arrange
            service = new TreeService(mockUnitOfWork.Object);

            //Act
            service.UpdateTree(note);

            //Assert
            mockUnitOfWork.Verify(db => db.Commit());
        }


        private static IQueryable<Tree> GetTrees()
        {
            var Trees = new List<Tree>();

            for (int i = 0; i < TestConstants.PAGE_TotalCount; i++)
            {
                Trees.Add(new Tree
                {
                    TreeId = i,
                    Name = "Foo"
                });
            }

            return Trees.AsQueryable();
        }
    }
}
