//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using Naif.Core.Caching;
using Naif.Core.Contracts;
using Naif.Data;

namespace FamilyTreeProject.DomainServices
{
    public class FamilyTreeServiceFactory : IFamilyTreeServiceFactory
    {
        private readonly FamilyService _familyService;
        private readonly IndividualService _individualService;
        private readonly TreeService _treeService;

        public FamilyTreeServiceFactory(IUnitOfWork unitOfWork, ICacheProvider cache)
        {
            Requires.NotNull(unitOfWork);
            Requires.NotNull(cache);

            _familyService = new FamilyService(unitOfWork);
            _individualService = new IndividualService(unitOfWork);

            _treeService = new TreeService(unitOfWork, cache);

            _familyService.TreeService = _treeService;
            _individualService.TreeService = _treeService;
        }

        public IFamilyService CreateFamilyService()
        {
            return _familyService;
        }

        public IIndividualService CreateIndividualService()
        {
            return _individualService;
        }

        public ITreeService CreateTreeService()
        {
            return _treeService;
        }
    }
}
