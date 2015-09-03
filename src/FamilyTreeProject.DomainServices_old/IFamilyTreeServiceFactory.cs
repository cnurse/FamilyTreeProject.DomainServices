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
        IFamilyService CreateFamilyService();

        IIndividualService CreateIndividualService();

        ITreeService CreateTreeService();
    }
}
