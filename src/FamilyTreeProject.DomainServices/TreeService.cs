using System;
using System.Collections.Generic;
using System.Linq;
using FamilyTreeProject.Core;
using FamilyTreeProject.Core.Collections;
using FamilyTreeProject.Core.Common;
using FamilyTreeProject.Core.Contracts;
using FamilyTreeProject.Core.Data;

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
            //_familyRepository = _unitOfWork.GetRepository<Family>();
            //_individualRepository = _unitOfWork.GetRepository<Individual>();
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

        //private readonly IRepository<Family> _familyRepository;
        //private readonly IRepository<Individual> _individualRepository;

        //private void GetFamilies(Tree tree)
        //{
        //    tree.Families = _familyRepository.Get(tree.TreeId);
        //}

        //private void GetIndividuals(Tree tree)
        //{
        //    tree.Individuals = _individualRepository.Get(tree.TreeId);
        //}

        //private void ProcessFamilies(Tree tree)
        //{
        //    foreach (var family in tree.Families)
        //    {
        //        if (family.HusbandId.HasValue)
        //        {
        //            family.Husband = tree.Individuals.SingleOrDefault(i => i.Id == family.HusbandId.Value);
        //        }
        //        if (family.WifeId.HasValue)
        //        {
        //            family.Wife = tree.Individuals.SingleOrDefault(i => i.Id == family.WifeId.Value);
        //        }

        //        if (family.HusbandId.HasValue && family.WifeId.HasValue)
        //        {
        //            family.Children = tree.Individuals.Where(ind => ind.FatherId == family.HusbandId.Value && ind.MotherId == family.WifeId.Value).ToList();
        //        }
        //        else if (family.HusbandId.HasValue)
        //        {
        //            family.Children = tree.Individuals.Where(ind => ind.FatherId == family.HusbandId.Value && !ind.MotherId.HasValue).ToList();
        //        }
        //        else if (family.WifeId.HasValue)
        //        {
        //            family.Children = tree.Individuals.Where(ind => !ind.FatherId.HasValue && ind.MotherId == family.WifeId.Value).ToList();
        //        }
        //    }
        //}

        //private void ProcessIndividuals(Tree tree)
        //{
        //    foreach (var individual in tree.Individuals)
        //    {
        //        if (individual.FatherId > 0)
        //        {
        //            var father = tree.Individuals.SingleOrDefault(i => i.Id == individual.FatherId);
        //            if (father != null)
        //            {
        //                individual.Father = father;
        //            }
        //        }
        //        if (individual.MotherId > 0)
        //        {
        //            var mother = tree.Individuals.SingleOrDefault(i => i.Id == individual.MotherId);
        //            if (mother != null)
        //            {
        //                individual.Mother = mother;
        //            }
        //        }
        //        individual.Children = tree.Individuals.Where(ind => ind.FatherId == individual.Id || ind.MotherId == individual.Id).ToList();

        //        individual.Spouses = new List<Individual>();

        //        var families = tree.Families.Where(f => f.HusbandId == individual.Id || f.WifeId == individual.Id);
        //        foreach (Family fam in families)
        //        {
        //            Individual spouse = null;

        //            if (fam.HusbandId == individual.Id)
        //            {
        //                if (fam.WifeId.HasValue && fam.WifeId.Value > 0)
        //                {
        //                    spouse = tree.Individuals.SingleOrDefault(i => i.Id == fam.WifeId.Value);
        //                }
        //            }
        //            else
        //            {
        //                if (fam.HusbandId.HasValue && fam.HusbandId.Value > 0)
        //                {
        //                    spouse = tree.Individuals.SingleOrDefault(i => i.Id == fam.HusbandId.Value);
        //                }
        //            }
        //            if (spouse != null)
        //            {
        //                individual.Spouses.Add(spouse);
        //            }
        //        }
        //    }
        //}
    }
}
