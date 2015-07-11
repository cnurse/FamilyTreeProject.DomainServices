//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

namespace FamilyTreeProject.DomainServices
{
    public class IndividualServiceSettings
    {
        public IndividualServiceSettings()
        {
            
        }

        public IndividualServiceSettings(IndividualServiceSettings settings)
        {
            IncludeChildren = settings.IncludeChildren;
            IncludeParents = settings.IncludeParents;
            IncludeSpouses = settings.IncludeSpouses;
            TreeId = settings.TreeId;
        }

        public bool IncludeChildren { get; set; }

        public bool IncludeParents { get; set; }

        public bool IncludeSpouses { get; set; }

        public int TreeId { get; set; }
    }
}
