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
    public class TreeService : ITreeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILinqRepository<Tree> _repository;

        public TreeService(IUnitOfWork unitOfWork)
        {
            //Contract
            Requires.NotNull(unitOfWork);

            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetLinqRepository<Tree>();
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

        public Tree GetTree(int treeId)
        {
            Requires.NotNegative("treeId", treeId);

            return GetTrees().SingleOrDefault(t => t.TreeId == treeId);
        }

        public IEnumerable<Tree> GetTrees()
        {
            return _repository.GetAll();
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
