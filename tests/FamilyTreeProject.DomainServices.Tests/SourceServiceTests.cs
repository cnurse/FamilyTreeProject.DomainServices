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
// ReSharper disable UnusedVariable

namespace FamilyTreeProject.DomainServices.Tests
{
    [TestFixture]
    public class SourceServiceTests : EntityServiceBaseTests<Source, SourceService>
    {
        protected override IEnumerable<Source> GetEntities(int count)
        {
            var sources = new List<Source>();

            for (int i = 0; i < count; i++)
            {
                sources.Add(new Source
                {
                    Id = i.ToString(),
                    Author = String.Format(TestConstants.SRC_Author, i),
                    Title = String.Format(TestConstants.SRC_Title, i),
                    TreeId = TestConstants.TREE_Id
                });
            }

            return sources;
        }

        protected override Source NewEntity()
        {
            return new Source { Author = "Foo", Title = "Bar" };
        }

        protected override Source UpdateEntity()
        {
            return new Source { Id = TestConstants.ID_Exists, Author = "Foo", Title = "Bar" };
        }
    }
}
