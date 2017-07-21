//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************


using System;
using FamilyTreeProject.Data;
using Moq;
using NUnit.Framework;

namespace FamilyTreeProject.DomainServices.Tests
{
    [TestFixture]
    public class FamilyTreeServiceFactoryTests
    {
        private FamilyTreeServiceFactory _serviceFactory;
        private Mock<IUnitOfWork> _mockUnitOfWork;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Test]
        public void FamilyTreeServiceFactory_Constructor_Throws_If_UnitOfWork_Argument_Is_Null()
        {
            //Arrange

            //Act,Assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new FamilyTreeServiceFactory(null));
        }

        [Test]
        public void CreateFamilyService_Returns_FamilyService()
        {
            //Arrange
            _serviceFactory = new FamilyTreeServiceFactory(_mockUnitOfWork.Object);

            //Act
            var service = _serviceFactory.CreateFamilyService();

            //Assert
            Assert.IsInstanceOf<FamilyService>(service);
        }

        [Test]
        public void CreateIndividualService_Returns_IndividualService()
        {
            //Arrange
            _serviceFactory = new FamilyTreeServiceFactory(_mockUnitOfWork.Object);

            //Act
            var service = _serviceFactory.CreateIndividualService();

            //Assert
            Assert.IsInstanceOf<IndividualService>(service);
        }

        [Test]
        public void CreateTreeService_Returns_TreeService()
        {
            //Arrange
            _serviceFactory = new FamilyTreeServiceFactory(_mockUnitOfWork.Object);

            //Act
            var service = _serviceFactory.CreateIndividualService();

            //Assert
            Assert.IsInstanceOf<IndividualService>(service);
        }
    }
}
