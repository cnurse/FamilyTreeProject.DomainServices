//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System.Collections.Generic;
using System.Linq;
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
        private readonly ILinqRepository<Individual> _individualRepository;
        private readonly ILinqRepository<Family> _familyRepository;

        /// <summary>
        /// Constructs an Individuals Service to manage Individuals
        /// </summary>
        /// <param name = "unitOfWork">The Unit Of Work to use to retrieve data</param>
        /// <param name = "repository">The Repository to use.</param>
        public IndividualService(IUnitOfWork unitOfWork)
        {
            //Contract
            Requires.NotNull(unitOfWork);

            _unitOfWork = unitOfWork;
            _individualRepository = _unitOfWork.GetLinqRepository<Individual>();
            _familyRepository = _unitOfWork.GetLinqRepository<Family>();
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
        /// <returns>An <see cref = "IList{Individual}" />.</returns>
        public IList<Individual> GetChildren(int parentId)
        {
            //Contract
            Requires.NotNegative("parentId", parentId);

            return _individualRepository.Find(ind => ind.FatherId == parentId || ind.MotherId == parentId).ToList();
        }

        /// <summary>
        ///   Retrieves a single individual
        /// </summary>
        /// <param name = "id">The Id of the Indiidual to retrieve</param>
        /// <param name = "includeChildren">A flag that indicates whether to get the children of the Individual</param>
        /// <returns>An <see cref = "Individual" /></returns>
        public Individual GetIndividual(int id, bool includeChildren)
        {
            //Contract
            Requires.NotNegative("id", id);

            var individual = _individualRepository.GetAll().SingleOrDefault(i => i.Id == id);

            if (individual != null)
            {
                if (individual.FatherId > 0)
                {
                    var father = _individualRepository.GetAll().SingleOrDefault(i => i.Id == individual.FatherId);
                    if (father != null)
                    {
                        individual.Father = father;
                    }
                }
                if (individual.MotherId > 0)
                {
                    var mother = _individualRepository.GetAll().SingleOrDefault(i => i.Id == individual.MotherId);
                    if (mother != null)
                    {
                        individual.Mother = mother;
                    }
                }
                individual.Children = includeChildren ? GetChildren(id) : new List<Individual>();
            }

            return individual;
        }

        public IEnumerable<Individual> GetIndividuals(int treeId)
        {
            //Contract
            Requires.NotNegative("treeId", treeId);

            return _individualRepository.Find(ind => ind.TreeId == treeId);
        }

        public IList<Individual> GetSpouses(int individualId)
        {
            //Contract
            Requires.NotNegative("individualId", individualId);

            var spouses = new List<Individual>();
            var families = _familyRepository.Find(f => f.HusbandId == individualId || f.WifeId == individualId);
            foreach (Family fam in families)
            {
                Individual spouse = null;
                if (fam.HusbandId == individualId)
                {
                    if (fam.WifeId.HasValue && fam.WifeId.Value > 0)
                    {
                        spouse = GetIndividual(fam.WifeId.Value, false);
                    }
                }
                else
                {
                    if (fam.HusbandId.HasValue && fam.HusbandId.Value > 0)
                    {
                        spouse = GetIndividual(fam.HusbandId.Value, false);
                    }
                }
                if (spouse != null)
                {
                    spouses.Add(spouse);
                }
            }


            return spouses;
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
