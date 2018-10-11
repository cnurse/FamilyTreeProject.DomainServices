using System;
using System.Collections.Generic;
using FamilyTreeProject.Core;
using FamilyTreeProject.Core.Common;
using FamilyTreeProject.Core.Data;

namespace FamilyTreeProject.DomainServices.Common
{
    public abstract class AncestorEntityService<TAncestorEntity> : EntityService<TAncestorEntity> where TAncestorEntity : AncestorEntity
    {
        private readonly IRepository<Citation> _citationRepository;
        private readonly IRepository<Fact> _factRepository;
        
        protected AncestorEntityService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            if (!Repository.SupportsAggregates)
            {
                _citationRepository = UnitOfWork.GetRepository<Citation>();
                _factRepository = UnitOfWork.GetRepository<Fact>();
            }
        }
        
        public override void Add(TAncestorEntity ancestorEntity)
        {
            base.Add(ancestorEntity);
            
            if (!Repository.SupportsAggregates)
            {
                AddCitations(ancestorEntity.Citations, ancestorEntity);
                
                foreach (var fact in ancestorEntity.Facts)
                {
                    fact.OwnerId = ancestorEntity.UniqueId;
                    fact.OwnerType = ancestorEntity.EntityType;
                    fact.TreeId = ancestorEntity.TreeId;
                    fact.UniqueId = Guid.NewGuid().ToString();
                    _factRepository.Add(fact);
                    
                    AddCitations(fact.Citations, fact);
                    AddNotes(fact.Notes, fact);
                    AddMultimedia(fact.Multimedia, fact);
                }

                UnitOfWork.Commit();
            }
        }

        private void AddCitations(IList<Citation> citations,Entity entity)
        {
            foreach (var citation in citations)
            {
                citation.OwnerId = entity.UniqueId;
                citation.OwnerType = entity.EntityType;
                citation.TreeId = entity.TreeId;
                citation.UniqueId = Guid.NewGuid().ToString();
                _citationRepository.Add(citation);
                
                AddNotes(citation.Notes, entity);
                AddMultimedia(citation.Multimedia, entity);
            }
        }
    }
}