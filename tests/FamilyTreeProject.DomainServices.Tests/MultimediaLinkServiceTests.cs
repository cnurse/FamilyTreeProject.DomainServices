using System.Collections.Generic;
using FamilyTreeProject.Common.Models;
using FamilyTreeProject.DomainServices.Tests.Common;
using NUnit.Framework;

// ReSharper disable ObjectCreationAsStatement

namespace FamilyTreeProject.DomainServices.Tests
{
    [TestFixture]
    public class MultimediaLinkServiceTests : EntityServiceBaseTests<MultimediaLink, MultimediaLinkService>
    {
        protected override IEnumerable<MultimediaLink> GetEntities(int count)
        {
            var multimediaLinks = new List<MultimediaLink>();

            for (int i = 0; i < count; i++)
            {
                multimediaLinks.Add(new MultimediaLink
                {
                    Id = i,
                    File = "Foo",
                    TreeId = TestConstants.TREE_Id,
                });
            }

            return multimediaLinks;
        }

        protected override MultimediaLink NewEntity()
        {
            return new MultimediaLink { File = "Foo" };
        }

        protected override MultimediaLink UpdateEntity()
        {
            return new MultimediaLink { Id = TestConstants.ID_Exists, File = "Foo" };
        }
    }
}
