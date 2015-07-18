//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using Naif.Core.Caching;
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
        private const string FormattedCacheKey = "FTP_LinkedIndividuals_{0}";

        private readonly ICacheProvider _cache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Individual> _individualRepository;
        private readonly IRepository<Family> _familyRepository;

        /// <summary>
        /// Constructs an Individuals Service to manage Individuals
        /// </summary>
        /// <param name = "unitOfWork">The Unit Of Work to use to retrieve data</param>
        /// <param name = "cache">The Cache Provider to use</param>
        public IndividualService(IUnitOfWork unitOfWork, ICacheProvider cache)
        {
            Requires.NotNull(unitOfWork);
            Requires.NotNull(cache);

            _unitOfWork = unitOfWork;
            _cache = cache;
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

        ///// <summary>
        /////   Retrieves all the children of an Individual
        ///// </summary>
        ///// <param name = "parentId">The Id of the Parent</param>
        ///// <param name = "settings">A <see cref = "IndividualServiceSettings" /> object that contains settings 
        ///// to control the individual returned</param>
        ///// <returns>An <see cref = "IList{T}" />.</returns>
        //public IList<Individual> GetChildren(int parentId, IndividualServiceSettings settings)
        //{
        //    Requires.NotNegative("parentId", parentId);

        //    return _individualRepository.Get(settings.TreeId).Where(ind => ind.FatherId == parentId || ind.MotherId == parentId).ToList();
        //}

        /// <summary>
        /// Retrieves a single Individual
        /// </summary>
        /// <param name = "id">The Id of the Indiidual to retrieve</param>
        /// <param name="treeId">The Id of the tree</param>
        /// <returns>An <see cref = "Individual" /></returns>
        public Individual GetIndividual(int id, int treeId)
        {
            //Contract
            Requires.NotNegative("id", id);
            Requires.NotNegative("treeId", treeId);

            return GetIndividuals(treeId).SingleOrDefault(i => i.Id == id);
        }

        /// <summary>
        /// Retrieves all the individuals for a particular tree
        /// </summary>
        /// <param name="treeId">The Id of the tree</param>
        /// <returns>A collection of <see cref = "Individual" />objects</returns>
        public IEnumerable<Individual> GetIndividuals(int treeId)
        {
            Requires.NotNegative("treeId", treeId);

            var cacheKey = String.Format(FormattedCacheKey, treeId);

            var individuals = _cache.GetCachedObject<IEnumerable<Individual>>(cacheKey, () =>
                                {
                                    var list = _individualRepository.Get(treeId).ToList();
                                    foreach (var individual in list)
                                    {
                                        LinkRelatives(individual, list);
                                    }

                                    return list;
                                });

            return individuals;
        }

        ///// <summary>
        /////   Retrieves all the spouses of an Individual
        ///// </summary>
        ///// <param name = "individualId">The Id of the Individual</param>
        ///// <param name = "settings">A <see cref = "IndividualServiceSettings" /> object that contains settings 
        ///// to control the individuals returned</param>
        ///// <returns>An <see cref = "IList{Individual}" />.</returns>
        //public IList<Individual> GetSpouses(int individualId, IndividualServiceSettings settings)
        //{
        //    Requires.NotNegative("individualId", individualId);

        //    var spouses = new List<Individual>();

        //    LinkSpouses(individualId, spouses, settings);

        //    return spouses;
        //}

        private void LinkRelatives(Individual individual, IList<Individual> list)
        {
            if (individual != null)
            {
                if (individual.FatherId > 0)
                {
                    var father = list.SingleOrDefault(i => i.Id == individual.FatherId);
                    if (father != null)
                    {
                        individual.Father = father;
                    }
                }
                if (individual.MotherId > 0)
                {
                    var mother = list.SingleOrDefault(i => i.Id == individual.MotherId);
                    if (mother != null)
                    {
                        individual.Mother = mother;
                    }
                }
                individual.Children = list.Where(ind => ind.FatherId == individual.Id || ind.MotherId == individual.Id).ToList();

                individual.Spouses = new List<Individual>();
                var families = _familyRepository.Find(f => f.HusbandId == individual.Id || f.WifeId == individual.Id);
                foreach (Family fam in families)
                {
                    Individual spouse = null;

                    if (fam.HusbandId == individual.Id)
                    {
                        if (fam.WifeId.HasValue && fam.WifeId.Value > 0)
                        {
                            spouse = list.SingleOrDefault(i => i.Id == fam.WifeId.Value);
                        }
                    }
                    else
                    {
                        if (fam.HusbandId.HasValue && fam.HusbandId.Value > 0)
                        {
                            spouse = list.SingleOrDefault(i => i.Id == fam.HusbandId.Value);
                        }
                    }
                    if (spouse != null)
                    {
                        individual.Spouses.Add(spouse);
                    }
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
