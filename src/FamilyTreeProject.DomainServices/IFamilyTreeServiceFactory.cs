//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

namespace FamilyTreeProject.DomainServices
{
    public interface IFamilyTreeServiceFactory
    {
        ICitationService CreateCitationService();

        IFactService CreateFactService();

        IFamilyService CreateFamilyService();

        IIndividualService CreateIndividualService();

        IMultimediaLinkService CreateMultimediaService();

        INoteService CreateNoteService();

        IRepositoryService CreateRepositoryService();

        ISourceService CreateSourceService();

        ITreeService CreateTreeService();
    }
}
