using System;
using System.Collections.Generic;
using System.Linq;
using FamilyTreeProject.Common.Data;
using FamilyTreeProject.Common.Models;
using Naif.Core.Collections;
using Naif.Core.Contracts;

namespace FamilyTreeProject.DomainServices
{
    public class TreeService : ITreeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Tree> _repository;

        public TreeService(IUnitOfWork unitOfWork)
        {
            Requires.NotNull(unitOfWork);

            _unitOfWork = unitOfWork;

            _repository = _unitOfWork.GetRepository<Tree>();
        }

        /// <summary>
        ///   Adds a tree to the data store and sets the <see cref = "Tree.Id" /> property
        ///   of the <paramref name = "tree" /> to the id of the new tree.
        /// </summary>
        /// <param name = "tree">The tree to add to the data store.</param>
        public void Add(Tree tree)
        {
            //Contract
            Requires.NotNull(tree);

            if (string.IsNullOrEmpty(tree.UniqueId))
            {
                tree.UniqueId = Guid.NewGuid().ToString();
            }
            
            _repository.Add(tree);
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Deletes a tree from the data store
        /// </summary>
        /// <remarks>
        ///   The delete operation takes effect immediately
        /// </remarks>
        /// <param name = "tree">The tree to delete</param>
        public void Delete(Tree tree)
        {
            //Contract
            Requires.NotNull(tree);

            _repository.Delete(tree);
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Gets a collection of trees based on a predicate
        /// </summary>
        /// <param name="predicate">The predicate to use</param>
        /// <returns>List of trees</returns>
        public IEnumerable<Tree> Find(Func<Tree, bool> predicate)
        {
            return _repository.Find(predicate);
        }

        /// <summary>
        /// Retrieves a single tree
        /// </summary>
        /// <param name = "id">The Id of the tree</param>
        /// <returns>A <see cref = "Tree" /></returns>
        public Tree Get(int id)
        {
            Requires.NotNegative("id", id);

            return Get().SingleOrDefault(t => t.Id == id);
        }

        /// <summary>
        /// Retrieves all the trees
        /// </summary>
        /// <returns>A collection of <see cref = "Tree" /> objects</returns>
        public IEnumerable<Tree> Get()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Gets a page of trees based on a predicate
        /// </summary>
        /// <param name="predicate">The predicate to use</param>
        /// <param name="pageIndex">The page index to return</param>
        /// <param name="pageSize">The page size</param>
        /// <returns>List of trees</returns>
        public IPagedList<Tree> Get(Func<Tree, bool> predicate, int pageIndex, int pageSize)
        {
            return new PagedList<Tree>(Get().Where(predicate), pageIndex, pageSize);
        }

        /// <summary>
        /// Updates a tree in the data store.
        /// </summary>
        /// <param name = "tree">The tree to update in the data store.</param>
        public void Update(Tree tree)
        {
            //Contract
            Requires.NotNull(tree);

            _repository.Update(tree);
            _unitOfWork.Commit();
        }
    }
}
