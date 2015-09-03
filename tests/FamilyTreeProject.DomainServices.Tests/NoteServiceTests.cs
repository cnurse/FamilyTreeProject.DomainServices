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
    public class NoteServiceTests
    {
        private NoteService _service;
        private Mock<IUnitOfWork> _mockUnitOfWork;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Test]
        public void NoteService_Constructor_Throws_If_UnitOfWork_Argument_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new NoteService(null));
        }

        [Test]
        public void NoteService_Add_Throws_On_Null_Note()
        {
            //Arrange
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _service = new NoteService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Add(null));
        }

        [Test]
        public void NoteService_Add_Calls_Repsoitory_Add_Method_With_The_Same_Note_Object_It_Recieved()
        {
            // Create test data
            var newNote = new Note
            {
                Text = "Foo"
            };

            //Create Mock
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<Note>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Note>()).Returns(mockRepository.Object);

            //Arrange
            _service = new NoteService(_mockUnitOfWork.Object);

            //Act
            _service.Add(newNote);

            //Assert
            mockRepository.Verify(r => r.Add(newNote));
        }

        [Test]
        public void NoteService_Add_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newNote = new Note
            {
                Text = "Foo"
            };

            //Create Mock
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<Note>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Note>()).Returns(mockRepository.Object);

            //Arrange
            _service = new NoteService(_mockUnitOfWork.Object);

            //Act
            _service.Add(newNote);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        [Test]
        public void NoteService_Delete_Throws_On_Null_Note()
        {
            //Arrange
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _service = new NoteService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(null));
        }

        [Test]
        public void NoteService_Delete_Calls_Repsoitory_Delete_Method_With_The_Same_Note_Object_It_Recieved()
        {
            // Create test data
            var newNote = new Note
            {
                Text = "Foo"
            };

            //Create Mock
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<Note>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Note>()).Returns(mockRepository.Object);

            //Arrange
            _service = new NoteService(_mockUnitOfWork.Object);

            //Act
            _service.Delete(newNote);

            //Assert
            mockRepository.Verify(r => r.Delete(newNote));
        }

        [Test]
        public void NoteService_Delete_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newNote = new Note
            {
                Text = "Foo"
            };

            //Create Mock
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<Note>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Note>()).Returns(mockRepository.Object);

            //Arrange
            _service = new NoteService(_mockUnitOfWork.Object);

            //Act
            _service.Delete(newNote);

            //Assert
            _mockUnitOfWork.Verify(d => d.Commit());
        }

        [Test]
        public void NoteService_Get_Throws_On_Negative_Id()
        {
            //Arrange
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _service = new NoteService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1, It.IsAny<int>()));
        }

        [Test]
        public void NoteService_Get_Throws_On_Negative_TreeId()
        {
            //Arrange
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _service = new NoteService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(It.IsAny<int>(), -1));
        }

        [Test]
        public void NoteService_Get_Calls_Repository_GetAll()
        {
            //Arrange
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<Note>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Note>()).Returns(mockRepository.Object);

            _service = new NoteService(_mockUnitOfWork.Object);

            const int id = TestConstants.ID_Exists;

            //Act
            _service.Get(id, TestConstants.TREE_Id);

            //Assert
            mockRepository.Verify(r => r.Get(TestConstants.TREE_Id));
        }

        [Test]
        public void NoteService_Get_Returns_Note_On_Valid_Id()
        {
            //Arrange
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<Note>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetNotes(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(d => d.GetRepository<Note>()).Returns(mockRepository.Object);

            _service = new NoteService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            var note = _service.Get(id, TestConstants.TREE_Id);

            //Assert
            Assert.IsInstanceOf<Note>(note);
        }

        [Test]
        public void NoteService_Get_Returns_Null_On_InValid_Id()
        {
            //Arrange
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<Note>>();
            mockRepository.Setup(r => r.GetAll()).Returns(GetNotes(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(d => d.GetRepository<Note>()).Returns(mockRepository.Object);

            _service = new NoteService(_mockUnitOfWork.Object);
            const int id = TestConstants.ID_NotFound;

            //Act
            var note = _service.Get(id, TestConstants.TREE_Id);

            //Assert
            Assert.IsNull(note);
        }

        [Test]
        public void NoteService_Get_Overload_Throws_On_Negative_TreeId()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            _service = new NoteService(mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1));
        }

        [Test]
        public void NoteService_Get_Overload_Calls_Repository_Get()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<Note>>();
            mockUnitOfWork.Setup(d => d.GetRepository<Note>()).Returns(mockRepository.Object);
            _service = new NoteService(mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            _service.Get(treeId);

            //Assert
            mockRepository.Verify(r => r.Get(treeId));
        }

        [Test]
        public void NoteService_Get_Overload_Returns_List_Of_Notes()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Note>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetNotes(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Note>()).Returns(mockRepository.Object);

            _service = new NoteService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            var notes = _service.Get(treeId);

            //Assert
            Assert.IsInstanceOf<IEnumerable<Note>>(notes);
            Assert.AreEqual(TestConstants.PAGE_TotalCount, notes.Count());
        }

        [Test]
        public void NoteService_Get_ByPage_Overload_Throws_On_Negative_TreeId()
        {
            //Arrange
            _service = new NoteService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.Get(-1, t => true, 0, TestConstants.PAGE_RecordCount));
        }

        [Test]
        public void NoteService_Get_ByPage_Overload_Calls_Repository_Get()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Note>>();
            _mockUnitOfWork.Setup(u => u.GetRepository<Note>()).Returns(mockRepository.Object);

            _service = new NoteService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            _service.Get(treeId, t => true, 0, TestConstants.PAGE_RecordCount);

            //Assert
            mockRepository.Verify(r => r.Get(It.IsAny<int>()));
        }

        [Test]
        public void NoteService_Get_ByPage_Overload_Returns_PagedList_Of_Notes()
        {
            //Arrange
            var mockRepository = new Mock<IRepository<Note>>();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(GetNotes(TestConstants.PAGE_TotalCount));
            _mockUnitOfWork.Setup(u => u.GetRepository<Note>()).Returns(mockRepository.Object);

            _service = new NoteService(_mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            var notes = _service.Get(treeId, t => true, 0, TestConstants.PAGE_RecordCount);

            //Assert
            Assert.IsInstanceOf<IPagedList<Note>>(notes);
            Assert.AreEqual(TestConstants.PAGE_TotalCount, notes.TotalCount);
            Assert.AreEqual(TestConstants.PAGE_RecordCount, notes.PageSize);
        }

        [Test]
        public void NoteService_Update_Throws_On_Null_Note()
        {
            //Arrange
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            //Arrange
            _service = new NoteService(_mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => _service.Add(null));
        }

        [Test]
        public void NoteService_Update_Calls_Repository_Update_Method_With_The_Same_Note_Object_It_Recieved()
        {
            // Create test data
            var note = new Note
            {
                Text = "Foo"
            };

            //Create Mock
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<Note>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Note>()).Returns(mockRepository.Object);

            //Arrange
            _service = new NoteService(_mockUnitOfWork.Object);

            //Act
            _service.Update(note);

            //Assert
            mockRepository.Verify(r => r.Update(note));
        }

        [Test]
        public void NoteService_Update_Calls_UnitOfWork_Commit_Method_()
        {
            // Create test data
            var note = new Note
            {
                Text = "Foo"
            };

            //Create Mock
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<Note>>();
            _mockUnitOfWork.Setup(d => d.GetRepository<Note>()).Returns(mockRepository.Object);

            //Arrange
            _service = new NoteService(_mockUnitOfWork.Object);

            //Act
            _service.Update(note);

            //Assert
            _mockUnitOfWork.Verify(db => db.Commit());
        }

        private static IQueryable<Note> GetNotes(int count)
        {
            var notes = new List<Note>();

            for (int i = 0; i < count; i++)
            {
                notes.Add(new Note
                                {
                                    Id = i,
                                    Text = "Foo",
                                    TreeId = TestConstants.TREE_Id,
                                });
            }

            return notes.AsQueryable();
        }
    }
}
