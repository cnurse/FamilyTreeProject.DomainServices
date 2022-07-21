using FamilyTreeProject.Common.Data;
using Naif.Core.Contracts;

namespace FamilyTreeProject.DomainServices
{
    public class FamilyTreeServiceFactory : IFamilyTreeServiceFactory
    {
        private ICitationService _citationService;
        private IFactService _factService;
        private IFamilyService _familyService;
        private IIndividualService _individualService;
        private IMultimediaLinkService _multimediaService;
        private INoteService _noteService;
        private IRepositoryService _repositoryService;
        private ISourceService _sourceService;
        private ITreeService _treeService;

        private readonly IUnitOfWork _unitOfWork;

        public FamilyTreeServiceFactory(IUnitOfWork unitOfWork)
        {
            Requires.NotNull(unitOfWork);

            _unitOfWork = unitOfWork;
        }

        public ICitationService CreateCitationService()
        {
            return _citationService ?? (_citationService = new CitationService(_unitOfWork));
        }

        public IFactService CreateFactService()
        {
            return _factService ?? (_factService = new FactService(_unitOfWork));
        }

        public IFamilyService CreateFamilyService()
        {
            return _familyService ?? (_familyService = new FamilyService(_unitOfWork));
        }

        public IIndividualService CreateIndividualService()
        {
            return _individualService ?? (_individualService = new IndividualService(_unitOfWork));
        }

        public IMultimediaLinkService CreateMultimediaService()
        {
            return _multimediaService ?? ( _multimediaService = new MultimediaLinkService(_unitOfWork));
        }

        public INoteService CreateNoteService()
        {
            return _noteService ?? (_noteService = new NoteService(_unitOfWork));
        }

        public IRepositoryService CreateRepositoryService()
        {
            return _repositoryService ?? (_repositoryService = new RepositoryService(_unitOfWork));
        }

        public ISourceService CreateSourceService()
        {
            return _sourceService ?? (_sourceService = new SourceService(_unitOfWork));
        }

        public ITreeService CreateTreeService()
        {
            return _treeService ?? (_treeService = new TreeService(_unitOfWork));
        }
    }
}
