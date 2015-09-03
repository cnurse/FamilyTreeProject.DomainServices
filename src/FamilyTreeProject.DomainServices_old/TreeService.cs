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
using Naif.Core.Caching;
using Naif.Core.Contracts;
using Naif.Data;

namespace FamilyTreeProject.DomainServices
{
    public class TreeService : ITreeService
    {
        private const string FormattedTreeCache = "FTP_Tree_{0}";

        private readonly ICacheProvider _cache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Tree> _repository;
        private readonly IRepository<Family> _familyRepository;
        private readonly IRepository<Individual> _individualRepository;

        public TreeService(IUnitOfWork unitOfWork, ICacheProvider cache)
        {
            Requires.NotNull(unitOfWork);
            Requires.NotNull(cache);

            _unitOfWork = unitOfWork;
            _cache = cache;

            _repository = _unitOfWork.GetRepository<Tree>();
            _familyRepository = _unitOfWork.GetRepository<Family>();
            _individualRepository = _unitOfWork.GetRepository<Individual>();
        }

        public void AddTree(Tree tree)
        {
            //Contract
            Requires.NotNull(tree);

            _repository.Add(tree);
            _unitOfWork.Commit();
        }

        public void DeleteTree(Tree tree)
        {
            //Contract
            Requires.NotNull(tree);

            _repository.Delete(tree);
            _unitOfWork.Commit();
        }

        private void GetFamilies(Tree tree)
        {
            tree.Families = _familyRepository.Get(tree.TreeId);
        }

        private void GetIndividuals(Tree tree)
        {
            tree.Individuals = _individualRepository.Get(tree.TreeId);
        }

        public Tree GetTree(int treeId)
        {
            Requires.NotNegative("treeId", treeId);

            var cacheKey = String.Format(FormattedTreeCache, treeId);

            var cacheTree = _cache.GetCachedObject<Tree>(cacheKey, () =>
                            {
                                var tree = GetTrees().SingleOrDefault(t => t.TreeId == treeId);

                                if (tree != null)
                                {
                                    GetIndividuals(tree);

                                    GetFamilies(tree);

                                    ProcessIndividuals(tree);

                                    ProcessFamilies(tree);
                                }

                                return tree;
                            });


            return cacheTree;
        }

        public IEnumerable<Tree> GetTrees()
        {
            return _repository.GetAll();
        }

        private void ProcessFamilies(Tree tree)
        {
            foreach (var family in tree.Families)
            {
                if (family.HusbandId.HasValue)
                {
                    family.Husband = tree.Individuals.SingleOrDefault(i => i.Id == family.HusbandId.Value);
                }
                if (family.WifeId.HasValue)
                {
                    family.Wife = tree.Individuals.SingleOrDefault(i => i.Id == family.WifeId.Value);
                }

                if (family.HusbandId.HasValue && family.WifeId.HasValue)
                {
                    family.Children = tree.Individuals.Where(ind => ind.FatherId == family.HusbandId.Value && ind.MotherId == family.WifeId.Value).ToList();
                }
                else if (family.HusbandId.HasValue)
                {
                    family.Children = tree.Individuals.Where(ind => ind.FatherId == family.HusbandId.Value && !ind.MotherId.HasValue).ToList();
                }
                else if (family.WifeId.HasValue)
                {
                    family.Children = tree.Individuals.Where(ind => !ind.FatherId.HasValue && ind.MotherId == family.WifeId.Value).ToList();
                }
            }
        }

        private void ProcessIndividuals(Tree tree)
        {
            foreach (var individual in tree.Individuals)
            {
                if (individual.FatherId > 0)
                {
                    var father = tree.Individuals.SingleOrDefault(i => i.Id == individual.FatherId);
                    if (father != null)
                    {
                        individual.Father = father;
                    }
                }
                if (individual.MotherId > 0)
                {
                    var mother = tree.Individuals.SingleOrDefault(i => i.Id == individual.MotherId);
                    if (mother != null)
                    {
                        individual.Mother = mother;
                    }
                }
                individual.Children = tree.Individuals.Where(ind => ind.FatherId == individual.Id || ind.MotherId == individual.Id).ToList();

                individual.Spouses = new List<Individual>();

                var families = tree.Families.Where(f => f.HusbandId == individual.Id || f.WifeId == individual.Id);
                foreach (Family fam in families)
                {
                    Individual spouse = null;

                    if (fam.HusbandId == individual.Id)
                    {
                        if (fam.WifeId.HasValue && fam.WifeId.Value > 0)
                        {
                            spouse = tree.Individuals.SingleOrDefault(i => i.Id == fam.WifeId.Value);
                        }
                    }
                    else
                    {
                        if (fam.HusbandId.HasValue && fam.HusbandId.Value > 0)
                        {
                            spouse = tree.Individuals.SingleOrDefault(i => i.Id == fam.HusbandId.Value);
                        }
                    }
                    if (spouse != null)
                    {
                        individual.Spouses.Add(spouse);
                    }
                }
            }
        }

        public void UpdateTree(Tree tree)
        {
            //Contract
            Requires.NotNull(tree);

            _repository.Update(tree);
            _unitOfWork.Commit();
        }
    }
}
