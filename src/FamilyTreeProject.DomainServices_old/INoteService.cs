//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System.Collections.Generic;

namespace FamilyTreeProject.DomainServices
{
    public interface INoteService
    {
        /// <summary>
        ///   Adds a note to the data store and sets the <see cref = "Note.Id" /> property
        ///   of <paramref name = "newNote" /> to the id of the new note.
        /// </summary>
        /// <param name = "newNote">The note to add to the data store.</param>
        void AddNote(Note newNote);

        /// <summary>
        ///   Deletes a note from the data store
        /// </summary>
        /// <remarks>
        ///   The delete operation takes effect immediately
        /// </remarks>
        /// <param name = "note">The note to delete</param>
        void DeleteNote(Note note);

        /// <summary>
        ///   Retrieves a single Note
        /// </summary>
        /// <param name = "id">The Id of the Note to retrieve</param>
        /// <returns>An <see cref = "Note" /></returns>
        Note GetNote(int id);

        IEnumerable<Note> GetNotes(int treeId);

        /// <summary>
        ///   Updates a note in the data store.
        /// </summary>
        /// <param name = "note">The note to update in the data store.</param>
        void UpdateNote(Note note);
    }
}
