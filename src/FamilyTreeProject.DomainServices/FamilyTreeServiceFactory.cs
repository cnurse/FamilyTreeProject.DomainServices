//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using FamilyTreeProject.Contracts;
using FamilyTreeProject.Data;

namespace FamilyTreeProject.DomainServices
{
    public class FamilyTreeServiceFactory : IFamilyTreeServiceFactory
    {
        private readonly ICitationService _citationService;
        private readonly IFactService _factService;
        private readonly IFamilyService _familyService;
        private readonly IIndividualService _individualService;
        private readonly IMultimediaLinkService _multimediaService;
        private readonly INoteService _noteService;
        private readonly IRepositoryService _repositoryService;
        private readonly ISourceService _sourceService;
        private readonly ITreeService _treeService;

        public FamilyTreeServiceFactory(IUnitOfWork unitOfWork)
        {
            Requires.NotNull(unitOfWork);

            _citationService = new CitationService(unitOfWork);
            _familyService = new FamilyService(unitOfWork);
            _individualService = new IndividualService(unitOfWork);
            _factService = new FactService(unitOfWork);
            _multimediaService = new MultimediaLinkService(unitOfWork);
            _noteService = new NoteService(unitOfWork);
            _repositoryService = new RepositoryService(unitOfWork);
            _sourceService = new SourceService(unitOfWork);
            _treeService = new TreeService(unitOfWork);
        }

        public ICitationService CreateCitationService()
        {
            return _citationService;
        }

        public IFactService CreateFactService()
        {
            return _factService;
        }

        public IFamilyService CreateFamilyService()
        {
            return _familyService;
        }

        public IIndividualService CreateIndividualService()
        {
            return _individualService;
        }

        public IMultimediaLinkService CreateMultimediaService()
        {
            return _multimediaService;
        }

        public INoteService CreateNoteService()
        {
            return _noteService;
        }

        public IRepositoryService CreateRepositoryService()
        {
            return _repositoryService;
        }

        public ISourceService CreateSourceService()
        {
            return _sourceService;
        }

        public ITreeService CreateTreeService()
        {
            return _treeService;
        }
    }
}
