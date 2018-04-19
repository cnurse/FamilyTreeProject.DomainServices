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
using FamilyTreeProject.Core;
using FamilyTreeProject.Core.Collections;
using FamilyTreeProject.Core.Common;
using FamilyTreeProject.Core.Contracts;
using FamilyTreeProject.Core.Data;

namespace FamilyTreeProject.DomainServices.Common
{
    /// <summary>
    /// Abstract base service for the domain services.  This class handles the "core" CRUD operations.
    /// </summary>
    public abstract class EntityService<TEntity> : IEntityService<TEntity> where TEntity : Entity
    {
        private readonly IRepository<MultimediaLink> MultimediaRepository;
        private readonly IRepository<Note> NoteRepository;

        protected EntityService(IUnitOfWork unitOfWork)
        {
            //Contract
            Requires.NotNull(unitOfWork);

            UnitOfWork = unitOfWork;
            Repository = UnitOfWork.GetRepository<TEntity>();
            NoteRepository = UnitOfWork.GetRepository<Note>();
            MultimediaRepository = UnitOfWork.GetRepository<MultimediaLink>();
        }

        protected IRepository<TEntity> Repository { get; private set; }
        protected IUnitOfWork UnitOfWork { get; private set; }


        /// <summary>
        ///   Adds an entity to the data store and sets the <see cref = "BaseEntity.Id" /> property
        ///   of the <paramref name = "entity" /> to the id of the new entity.
        /// </summary>
        /// <param name = "entity">The entity to add to the data store.</param>
        public virtual void Add(TEntity entity)
        {
            //Contract
            Requires.NotNull(entity);

            if (entity is IUniqueEntity uniqueEntity)
            {
                if (string.IsNullOrEmpty(uniqueEntity.UniqueId))
                {
                    uniqueEntity.UniqueId = Guid.NewGuid().ToString();
                }
            }
           
            Repository.Add(entity);
            UnitOfWork.Commit();

            //if repository does not support aggregates then add the notes
            if (!Repository.SupportsAggregates)
            {
                AddNotes(entity.Notes, entity);
                
                AddMultimedia(entity.Multimedia, entity);
                
                UnitOfWork.Commit();
            }
        }

        protected void AddMultimedia(IList<MultimediaLink> multimediaLinks,TEntity entity)
        {
            foreach (var multimedia in multimediaLinks)
            {
                multimedia.OwnerId = entity.Id;
                multimedia.TreeId = entity.TreeId;
                MultimediaRepository.Add(multimedia);
            }
        }

        protected void AddNotes(IList<Note> notes,TEntity entity)
        {
            foreach (var note in notes)
            {
                note.OwnerId = entity.Id;
                note.TreeId = entity.TreeId;
                NoteRepository.Add(note);
            }
        }

        /// <summary>
        ///   Deletes an entity from the data store
        /// </summary>
        /// <remarks>
        ///   The delete operation takes effect immediately
        /// </remarks>
        /// <param name = "entity">The entity to delete</param>
        public virtual void Delete(TEntity entity)
        {
            //Contract
            Requires.NotNull(entity);

            Repository.Delete(entity);
            UnitOfWork.Commit();
        }

        /// <summary>
        /// Retrieves a single TEntity
        /// </summary>
        /// <param name = "id">The Id of the entity to retrieve</param>
        /// <param name = "treeId">The Id of the Tree</param>
        /// <returns>A <see cref = "TEntity" /></returns>
        public virtual TEntity Get(int id, int treeId)
        {
            Requires.NotNegative("id", id);

            return Get(treeId).SingleOrDefault(n => n.Id == id);
        }

        /// <summary>
        /// Retrieves all the entities of this type for a Tree
        /// </summary>
        /// <param name = "treeId">The Id of the Tree</param>
        /// <returns>A collection of <see cref = "TEntity" /> objects</returns>
        public virtual IEnumerable<TEntity> Get(int treeId)
        {
            //Contract
            Requires.NotNegative("treeId", treeId);

            return Repository.Find(t => t.TreeId == treeId);
        }

        /// <summary>
        /// Gets a list of entities based on a predicate
        /// </summary>
        /// <param name="treeId">The Id of the tree</param>
        /// <param name="predicate">The predicate to use</param>
        /// <returns>List of entities</returns>

        public IEnumerable<TEntity> Get(int treeId, Func<TEntity, bool> predicate)
        {
            return Get(treeId).Where(predicate);
        }

        /// <summary>
        /// Gets a page of entities based on a predicate
        /// </summary>
        /// <param name="treeId">The Id of the tree</param>
        /// <param name="predicate">The predicate to use</param>
        /// <param name="pageIndex">The page index to return</param>
        /// <param name="pageSize">The page size</param>
        /// <returns>List of entities</returns>
        public virtual IPagedList<TEntity> Get(int treeId, Func<TEntity, bool> predicate, int pageIndex, int pageSize)
        {
            return new PagedList<TEntity>(Get(treeId).Where(predicate), pageIndex, pageSize);
        }

        /// <summary>
        /// Updates an entity in the data store.
        /// </summary>
        /// <param name = "entity">The entity to update in the data store.</param>
        public virtual void Update(TEntity entity)
        {
            //Contract
            Requires.NotNull(entity);

            Repository.Update(entity);
            UnitOfWork.Commit();
        }
    }
}
