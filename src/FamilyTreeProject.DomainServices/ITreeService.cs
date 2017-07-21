//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System;
using System.Collections.Generic;
using FamilyTreeProject.Collections;

namespace FamilyTreeProject.DomainServices
{
    public interface ITreeService
    {
        /// <summary>
        ///   Adds a tree to the data store and sets the <see cref = "Tree.TreeId" /> property
        ///   of the <paramref name = "tree" /> to the id of the new tree.
        /// </summary>
        /// <param name = "tree">The tree to add to the data store.</param>
        void Add(Tree tree);

        /// <summary>
        /// Deletes a tree from the data store
        /// </summary>
        /// <remarks>
        ///   The delete operation takes effect immediately
        /// </remarks>
        /// <param name = "tree">The tree to delete</param>
        void Delete(Tree tree);

        /// <summary>
        /// Retrieves a single tree
        /// </summary>
        /// <param name = "treeId">The Id of the tree</param>
        /// <returns>A <see cref = "Tree" /></returns>
        Tree Get(string treeId);

        /// <summary>
        /// Retrieves all the trees
        /// </summary>
        /// <returns>A collection of <see cref = "Tree" /> objects</returns>
        IEnumerable<Tree> Get();

        /// <summary>
        /// Gets a page of trees based on a predicate
        /// </summary>
        /// <param name="predicate">The predicate to use</param>
        /// <param name="pageIndex">The page index to return</param>
        /// <param name="pageSize">The page size</param>
        /// <returns>List of trees</returns>
        IPagedList<Tree> Get(Func<Tree, bool> predicate, int pageIndex, int pageSize);

        /// <summary>
        /// Updates a tree in the data store.
        /// </summary>
        /// <param name = "tree">The tree to update in the data store.</param>
        void Update(Tree tree);
    }
}
