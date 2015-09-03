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
    public interface ITreeService
    {
        void AddTree(Tree tree);

        void DeleteTree(Tree tree);

        Tree GetTree(int treeId);

        IEnumerable<Tree> GetTrees();

        void UpdateTree(Tree tree);
    }
}
