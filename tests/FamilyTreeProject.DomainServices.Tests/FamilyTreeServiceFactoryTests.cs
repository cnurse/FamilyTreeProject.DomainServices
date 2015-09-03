//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************


using System;
using Moq;
using Naif.Core.Caching;
using Naif.Data;
using NUnit.Framework;

namespace FamilyTreeProject.DomainServices.Tests
{
    [TestFixture]
    public class FamilyTreeServiceFactoryTests
    {
        private FamilyTreeServiceFactory _serviceFactory;
        private Mock<ICacheProvider> _mockCache;
        private Mock<IUnitOfWork> _mockUnitOfWork;

        [SetUp]
        public void SetUp()
        {
            _mockCache = new Mock<ICacheProvider>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Test]
        public void FamilyTreeServiceFactory_Constructor_Throws_If_Cache_Argument_Is_Null()
        {
            //Arrange

            //Act,Assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new FamilyTreeServiceFactory(_mockUnitOfWork.Object, null));
        }

        [Test]
        public void FamilyTreeServiceFactory_Constructor_Throws_If_UnitOfWork_Argument_Is_Null()
        {
            //Arrange

            //Act,Assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new FamilyTreeServiceFactory(null, _mockCache.Object));
        }

        [Test]
        public void CreateFamilyService_Returns_FamilyService()
        {
            //Arrange
            _serviceFactory = new FamilyTreeServiceFactory(_mockUnitOfWork.Object, _mockCache.Object);

            //Act
            var service = _serviceFactory.CreateFamilyService();

            //Assert
            Assert.IsInstanceOf<FamilyService>(service);
        }

        [Test]
        public void CreateIndividualService_Returns_IndividualService()
        {
            //Arrange
            _serviceFactory = new FamilyTreeServiceFactory(_mockUnitOfWork.Object, _mockCache.Object);

            //Act
            var service = _serviceFactory.CreateIndividualService();

            //Assert
            Assert.IsInstanceOf<IndividualService>(service);
        }

        [Test]
        public void CreateTreeService_Returns_TreeService()
        {
            //Arrange
            _serviceFactory = new FamilyTreeServiceFactory(_mockUnitOfWork.Object, _mockCache.Object);

            //Act
            var service = _serviceFactory.CreateIndividualService();

            //Assert
            Assert.IsInstanceOf<IndividualService>(service);
        }
    }
}
