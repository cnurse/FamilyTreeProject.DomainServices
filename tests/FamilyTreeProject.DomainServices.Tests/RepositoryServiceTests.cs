﻿using System;
using System.Collections.Generic;
using FamilyTreeProject.Common.Models;
using FamilyTreeProject.DomainServices.Tests.Common;
using NUnit.Framework;

namespace FamilyTreeProject.DomainServices.Tests
{
    [TestFixture]
    public class RepositoryServiceTests: EntityServiceBaseTests<Repository, RepositoryService>
    {
        protected override IEnumerable<Repository> GetEntities(int count)
        {
            var repositories = new List<Repository>();

            for (int i = 0; i < count; i++)
            {
                repositories.Add(new Repository
                {
                    Id = i,
                    Name = String.Format(TestConstants.REP_Name, i),
                    Address = String.Format(TestConstants.REP_Address, i),
                    TreeId = TestConstants.TREE_Id
                });
            }

            return repositories;
        }

        protected override Repository NewEntity()
        {
            return new Repository { Name = "Foo", Address = "Bar" };
        }

        protected override Repository UpdateEntity()
        {
            return new Repository { Id = TestConstants.ID_Exists, Name = "Foo", Address = "Bar" };
        }
    }
}
