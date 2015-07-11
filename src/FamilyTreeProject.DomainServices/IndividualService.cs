//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using Naif.Core.Contracts;
using Naif.Data;

namespace FamilyTreeProject.DomainServices
{
    /// <summary>
    ///   The IndividualsService provides a Facade to the Individuals store,
    ///   allowing for additional business logic to be applied.
    /// </summary>
    public class IndividualService : IIndividualService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Individual> _individualRepository;
        private readonly IRepository<Family> _familyRepository;

        /// <summary>
        /// Constructs an Individuals Service to manage Individuals
        /// </summary>
        /// <param name = "unitOfWork">The Unit Of Work to use to retrieve data</param>
        public IndividualService(IUnitOfWork unitOfWork)
        {
            //Contract
            Requires.NotNull(unitOfWork);

            _unitOfWork = unitOfWork;
            _individualRepository = _unitOfWork.GetRepository<Individual>();
            _familyRepository = _unitOfWork.GetRepository<Family>();
        }

        /// <summary>
        ///   Adds an individual to the data store and sets the <see cref = "Individual.Id" /> property
        ///   of <paramref name = "individual" /> to the id of the new individual.
        /// </summary>
        /// <param name = "individual">The individual to add to the data store.</param>
        public void AddIndividual(Individual individual)
        {
            //Contract
            Requires.NotNull(individual);

            _individualRepository.Add(individual);
            _unitOfWork.Commit();
        }

        /// <summary>
        ///   Deletes an individual from the data store
        /// </summary>
        /// <remarks>
        ///   The delete operation takes effect immediately
        /// </remarks>
        /// <param name = "individual">The individual to delete</param>
        public void DeleteIndividual(Individual individual)
        {
            //Contract
            Requires.NotNull(individual);

            _individualRepository.Delete(individual);
            _unitOfWork.Commit();
        }

        /// <summary>
        ///   Retrieves all the children of an Individual
        /// </summary>
        /// <param name = "parentId">The Id of the Parent</param>
        /// <param name = "settings">A <see cref = "IndividualServiceSettings" /> object that contains settings 
        /// to control the individual returned</param>
        /// <returns>An <see cref = "IList{T}" />.</returns>
        public IList<Individual> GetChildren(int parentId, IndividualServiceSettings settings)
        {
            Requires.NotNegative("parentId", parentId);

            return _individualRepository.Get(settings.TreeId).Where(ind => ind.FatherId == parentId || ind.MotherId == parentId).ToList();
        }

        /// <summary>
        /// Retrieves a single Individual
        /// </summary>
        /// <param name = "id">The Id of the Indiidual to retrieve</param>
        /// <param name = "settings">A <see cref = "IndividualServiceSettings" /> object that contains settings 
        /// to control the individual returned</param>
        /// <returns>An <see cref = "Individual" /></returns>
        public Individual GetIndividual(int id, IndividualServiceSettings settings)
        {
            //Contract
            Requires.NotNegative("id", id);

            var individual = _individualRepository.Get(settings.TreeId).SingleOrDefault(i => i.Id == id);

            LinkRelatives(individual, settings);

            return individual;
        }

        /// <summary>
        ///   Retrieves a collection of individuals
        /// </summary>
        /// <param name = "settings">A <see cref = "IndividualServiceSettings" /> object that contains settings 
        /// to control the individuals returned</param>
        /// <returns>A collection of <see cref = "Individual" />objects</returns>
        public IEnumerable<Individual> GetIndividuals(IndividualServiceSettings settings)
        {
            Requires.NotNull(settings);
            Requires.PropertyNotNegative(settings, "TreeId");

            var individuals = _individualRepository.Get(settings.TreeId).ToList();
            foreach (var individual in individuals)
            {
                LinkRelatives(individual, settings);
            }

            return individuals;
        }

        /// <summary>
        ///   Retrieves all the spouses of an Individual
        /// </summary>
        /// <param name = "individualId">The Id of the Individual</param>
        /// <param name = "settings">A <see cref = "IndividualServiceSettings" /> object that contains settings 
        /// to control the individuals returned</param>
        /// <returns>An <see cref = "IList{Individual}" />.</returns>
        public IList<Individual> GetSpouses(int individualId, IndividualServiceSettings settings)
        {
            Requires.NotNegative("individualId", individualId);

            var spouses = new List<Individual>();

            LinkSpouses(individualId, spouses, settings);

            return spouses;
        }

        private void LinkRelatives(Individual individual, IndividualServiceSettings settings)
        {
            if (individual != null)
            {
                if (settings.IncludeParents && individual.FatherId > 0)
                {
                    var father = _individualRepository.Get(settings.TreeId).SingleOrDefault(i => i.Id == individual.FatherId);
                    if (father != null)
                    {
                        individual.Father = father;
                    }
                }
                if (settings.IncludeParents && individual.MotherId > 0)
                {
                    var mother = _individualRepository.Get(settings.TreeId).SingleOrDefault(i => i.Id == individual.MotherId);
                    if (mother != null)
                    {
                        individual.Mother = mother;
                    }
                }
                individual.Children = settings.IncludeChildren ? GetChildren(individual.Id, settings) : new List<Individual>();

                LinkSpouses(individual.Id, individual.Spouses, settings);
            }
        }

        private void LinkSpouses(int inidivudalId, IList<Individual> spouses, IndividualServiceSettings settings)
        {
            if (spouses == null)
            {
                spouses = new List<Individual>();
            }

            var families = _familyRepository.Find(f => f.HusbandId == inidivudalId || f.WifeId == inidivudalId);
            foreach (Family fam in families)
            {
                Individual spouse = null;
                IndividualServiceSettings spouseSettings = new IndividualServiceSettings(settings)
                {
                    IncludeSpouses = false
                };

                if (fam.HusbandId == inidivudalId)
                {
                    if (fam.WifeId.HasValue && fam.WifeId.Value > 0)
                    {
                        spouse = GetIndividual(fam.WifeId.Value, spouseSettings);
                    }
                }
                else
                {
                    if (fam.HusbandId.HasValue && fam.HusbandId.Value > 0)
                    {
                        spouse = GetIndividual(fam.HusbandId.Value, spouseSettings);
                    }
                }
                if (spouse != null)
                {
                    spouses.Add(spouse);
                }
            }
        }

        /// <summary>
        ///   Updates an individual in the data store.
        /// </summary>
        /// <param name = "individual">The individual to update in the data store.</param>
        public void UpdateIndividual(Individual individual)
        {
            //Contract
            Requires.NotNull(individual);

            _individualRepository.Update(individual);
            _unitOfWork.Commit();
        }
    }
}
