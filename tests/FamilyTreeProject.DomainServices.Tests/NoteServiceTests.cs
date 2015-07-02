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
    public class NoteServiceTests
    {
        private NoteService service;

        #region Constructor Tests

        [Test]
        public void NoteService_Constructor_Throws_If_UnitOfWork_Argument_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new NoteService(null));
        }

        #endregion

        #region Tests

        #region AddNote Tests

        [Test]
        public void NoteService_AddNote_Throws_On_Null_Note()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            service = new NoteService(mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => service.AddNote(null));
        }

        [Test]
        public void NoteService_AddNote_Calls_Repsoitory_AddNote_Method_With_The_Same_Note_Object_It_Recieved()
        {
            // Create test data
            var newNote = new Note
            {
                Text = "Foo"
            };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Note>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Note>()).Returns(mockRepository.Object);

            //Arrange
            service = new NoteService(mockUnitOfWork.Object);

            //Act
            service.AddNote(newNote);

            //Assert
            mockRepository.Verify(r => r.Add(newNote));
        }

        [Test]
        public void NoteService_AddNote_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newNote = new Note
            {
                Text = "Foo"
            };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Note>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Note>()).Returns(mockRepository.Object);

            //Arrange
            service = new NoteService(mockUnitOfWork.Object);

            //Act
            service.AddNote(newNote);

            //Assert
            mockUnitOfWork.Verify(db => db.Commit());
        }

        #endregion

        #region DeleteNote Tests

        [Test]
        public void NoteService_DeleteNote_Throws_On_Null_Note()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            service = new NoteService(mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => service.DeleteNote(null));
        }

        [Test]
        public void FamilyService_DeleteNote_Calls_Repsoitory_Delete_Method_With_The_Same_Note_Object_It_Recieved()
        {
            // Create test data
            var newNote = new Note
            {
                Text = "Foo"
            };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Note>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Note>()).Returns(mockRepository.Object);

            //Arrange
            service = new NoteService(mockUnitOfWork.Object);

            //Act
            service.DeleteNote(newNote);

            //Assert
            mockRepository.Verify(r => r.Delete(newNote));
        }

        [Test]
        public void NoteService_DeleteNote_Calls_UnitOfWork_Commit_Method()
        {
            // Create test data
            var newNote = new Note
            {
                Text = "Foo"
            };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Note>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Note>()).Returns(mockRepository.Object);

            //Arrange
            service = new NoteService(mockUnitOfWork.Object);

            //Act
            service.DeleteNote(newNote);

            //Assert
            mockUnitOfWork.Verify(d => d.Commit());
        }

        #endregion

        #region GetNote Tests

        [Test]
        public void NoteService_GetNote_Throws_On_Negative_Id()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            service = new NoteService(mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => service.GetNote(-1));
        }

        [Test]
        public void NoteService_GetNote_Calls_Repository_GetAll()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Note>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Note>()).Returns(mockRepository.Object);
            service = new NoteService(mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            service.GetNote(id);

            //Assert
            mockRepository.Verify(r => r.GetAll());
        }

        [Test]
        public void NoteService_GetNote_Returns_Note_On_Valid_Id()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Note>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Note>()).Returns(mockRepository.Object);
            mockRepository.Setup(r => r.GetAll())
                                .Returns(GetNotes());
            service = new NoteService(mockUnitOfWork.Object);
            const int id = TestConstants.ID_Exists;

            //Act
            Note note = service.GetNote(id);

            //Assert
            Assert.IsInstanceOf<Note>(note);
        }

        [Test]
        public void NoteService_GetNote_Returns_Null_On_InValid_Id()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Note>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Note>()).Returns(mockRepository.Object);
            mockRepository.Setup(r => r.GetAll())
                                .Returns(GetNotes());
            service = new NoteService(mockUnitOfWork.Object);
            const int id = TestConstants.ID_NotFound;

            //Act
            var note = service.GetNote(id);

            //Assert
            Assert.IsNull(note);
        }

        #endregion

        #region GetNotes Tests

        [Test]
        public void NoteService_GetNotes_Throws_On_Negative_TreeId()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            service = new NoteService(mockUnitOfWork.Object);

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => service.GetNotes(-1));
        }

        [Test]
        public void NoteService_GetNotes_Calls_Repository_Find()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Note>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Note>()).Returns(mockRepository.Object);
            service = new NoteService(mockUnitOfWork.Object);
            const int treeId = TestConstants.TREE_Id;

            //Act
            service.GetNotes(treeId);

            //Assert
            mockRepository.Verify(r => r.Find(It.IsAny<Expression<Func<Note, bool>>>()));
        }

        #endregion

        #region UpdateNote Tests

        [Test]
        public void NoteService_UpdateNote_Throws_On_Null_Note()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            service = new NoteService(mockUnitOfWork.Object);

            //Assert
            Assert.Throws<ArgumentNullException>(() => service.AddNote(null));
        }

        [Test]
        public void NoteService_UpdateNote_Calls_Repository_Update_Method_With_The_Same_Note_Object_It_Recieved()
        {
            // Create test data
            var note = new Note
            {
                Text = "Foo"
            };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Note>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Note>()).Returns(mockRepository.Object);

            //Arrange
            service = new NoteService(mockUnitOfWork.Object);

            //Act
            service.UpdateNote(note);

            //Assert
            mockRepository.Verify(r => r.Update(note));
        }

        [Test]
        public void NoteService_UpdateNote_Calls_UnitOfWork_Commit_Method_()
        {
            // Create test data
            var note = new Note
            {
                Text = "Foo"
            };

            //Create Mock
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<ILinqRepository<Note>>();
            mockUnitOfWork.Setup(d => d.GetLinqRepository<Note>()).Returns(mockRepository.Object);

            //Arrange
            service = new NoteService(mockUnitOfWork.Object);

            //Act
            service.UpdateNote(note);

            //Assert
            mockUnitOfWork.Verify(db => db.Commit());
        }

        #endregion

        #endregion

        #region Helper Methods

        private static IQueryable<Note> GetNotes()
        {
            var Notes = new List<Note>();

            for (int i = 0; i < TestConstants.PAGE_TotalCount; i++)
            {
                Notes.Add(new Note
                {
                    Id = i,
                    Text = "Foo",
                    TreeId = TestConstants.TREE_Id,
                });
            }

            return Notes.AsQueryable();
        }

        #endregion
    }
}
