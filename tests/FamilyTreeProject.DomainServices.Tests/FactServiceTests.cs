//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System;
using System.Collections.Generic;
using FamilyTreeProject.TestUtilities;
using NUnit.Framework;

namespace FamilyTreeProject.DomainServices.Tests
{
    [TestFixture]
    public class FactServiceTests : EntityServiceBaseTests<Fact, FactService>
    {
        protected override IEnumerable<Fact> GetEntities(int count)
        {
            var facts = new List<Fact>();

            for (int i = 0; i < count; i++)
            {
                facts.Add(new Fact
                {
                    Id = i.ToString(),
                    Date = String.Format(TestConstants.EVN_Date, i),
                    Place = String.Format(TestConstants.EVN_Place, i),
                    TreeId = TestConstants.TREE_Id
                });
            }

            return facts;
        }

        protected override Fact NewEntity()
        {
            return new Fact { Date = "Foo", Place = "Bar" };
        }

        protected override Fact UpdateEntity()
        {
            return new Fact { Id = TestConstants.ID_Exists, Date = "Foo", Place = "Bar" };
        }
    }
}
