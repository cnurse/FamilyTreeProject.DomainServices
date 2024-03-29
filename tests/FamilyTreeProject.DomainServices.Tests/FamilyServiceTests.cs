﻿using System;
using System.Collections.Generic;
using FamilyTreeProject.Common.Models;
using FamilyTreeProject.DomainServices.Tests.Common;
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
                    Id = i,
                    WifeId = (i < 5 && i > 2) ? TestConstants.ID_WifeId : String.Empty,
                    HusbandId = (i < 5 && i > 2) ? TestConstants.ID_HusbandId : String.Empty,
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
