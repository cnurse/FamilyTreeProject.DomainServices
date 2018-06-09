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
                    fact.OwnerId = ancestorEntity.Id;
                    fact.TreeId = ancestorEntity.TreeId;
                    _factRepository.Add(fact);
                    
                    AddCitations(fact.Citations, ancestorEntity);
                    AddNotes(fact.Notes, ancestorEntity);
                    AddMultimedia(fact.Multimedia, ancestorEntity);
                }

                UnitOfWork.Commit();
            }
        }

        private void AddCitations(IList<Citation> citations,TAncestorEntity ancestorEntity)
        {
            foreach (var citation in citations)
            {
                citation.OwnerId = ancestorEntity.Id;
                citation.TreeId = ancestorEntity.TreeId;
                _citationRepository.Add(citation);
                
                AddNotes(citation.Notes, ancestorEntity);
                AddMultimedia(citation.Multimedia, ancestorEntity);
            }
        }
    }
}