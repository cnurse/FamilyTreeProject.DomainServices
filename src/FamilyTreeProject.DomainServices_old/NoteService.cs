//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System.Collections.Generic;
using System.Linq;
using Naif.Core.Contracts;
using Naif.Data;

namespace FamilyTreeProject.DomainServices
{
    public class NoteService : INoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Note> _repository;

        /// <summary>
        ///   Constructs a Note Service that will use the specified
        ///   <see cref = "IUnitOfWork"></see>
        ///   to retrieve data
        /// </summary>
        /// <param name = "unitOfWork">The <see cref = "IUnitOfWork"></see>
        ///   to use to retrieve data</param>
        public NoteService(IUnitOfWork unitOfWork)
        {
            //Contract
            Requires.NotNull(unitOfWork);

            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<Note>();
        }

        /// <summary>
        ///   Adds a note to the data store and sets the <see cref = "Note.Id" /> property
        ///   of <paramref name = "note" /> to the id of the new note.
        /// </summary>
        /// <param name = "note">The note to add to the data store.</param>
        public void AddNote(Note note)
        {
            //Contract
            Requires.NotNull(note);

            _repository.Add(note);
            _unitOfWork.Commit();
        }

        /// <summary>
        ///   Deletes a note from the data store
        /// </summary>
        /// <remarks>
        ///   The delete operation takes effect immediately
        /// </remarks>
        /// <param name = "note">The note to delete</param>
        public void DeleteNote(Note note)
        {
            //Contract
            Requires.NotNull(note);

            _repository.Delete(note);
            _unitOfWork.Commit();
        }

        /// <summary>
        ///   Retrieves a single Note
        /// </summary>
        /// <param name = "id">The Id of the Note to retrieve</param>
        /// <returns>An <see cref = "Note" /></returns>
        public Note GetNote(int id)
        {
            //Contract
            Requires.NotNegative("id", id);

            return _repository.GetAll().SingleOrDefault(n => n.Id == id);
        }

        public IEnumerable<Note> GetNotes(int treeId)
        {
            //Contract
            Requires.NotNegative("treeId", treeId);

            return _repository.Find(ind => ind.TreeId == treeId);
        }

        /// <summary>
        ///   Updates a note in the data store.
        /// </summary>
        /// <param name = "note">The note to update in the data store.</param>
        public void UpdateNote(Note note)
        {
            //Contract
            Requires.NotNull(note);

            _repository.Update(note);
            _unitOfWork.Commit();
        }
    }
}
