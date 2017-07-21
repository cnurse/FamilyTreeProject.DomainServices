//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System.Collections.Generic;
using FamilyTreeProject.TestUtilities;
using NUnit.Framework;

namespace FamilyTreeProject.DomainServices.Tests
{
    [TestFixture]
    public class FamilyServiceTests : EntityServiceBaseTests<Family, FamilyService>
    {
        protected override IEnumerable<Family> GetEntities(int count)
        {
            var familys = new List<Family>();

            for (int i = 0; i < count; i++)
            {
                familys.Add(new Family
                {
                    Id = i.ToString(),
                    WifeId = (i < 5 && i > 2) ? TestConstants.ID_WifeId : string.Empty,
                    HusbandId = (i < 5 && i > 2) ? TestConstants.ID_HusbandId : string.Empty,
                    TreeId = TestConstants.TREE_Id
                });
            }

            return familys;
        }

        protected override Family NewEntity()
        {
            return new Family
            {
                WifeId = TestConstants.ID_WifeId,
                HusbandId = TestConstants.ID_HusbandId
            };
        }

        protected override Family UpdateEntity()
        {
            return new Family
            {
                Id = TestConstants.ID_Exists,
                WifeId = TestConstants.ID_WifeId,
                HusbandId = TestConstants.ID_HusbandId
            };
        }
    }
}
