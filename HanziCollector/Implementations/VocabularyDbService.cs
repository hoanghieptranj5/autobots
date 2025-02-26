using AutoMapper;
using HanziCollector.Abstraction;
using Microsoft.Extensions.Logging;
using Repositories.Models.Vocabulary;
using Repositories.UnitOfWork.Abstractions;

namespace HanziCollector.Implementations;

public class VocabularyDbService : IVocabularyDbService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public VocabularyDbService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> SaveSingle(Vocabulary vocabulary)
    {
        bool inserted;
        try
        {
            inserted = await _unitOfWork.Vocabularies.Add(vocabulary);
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to save vocabulary: {ex.Message}");
        }

        return inserted;
    }

    public async Task<IEnumerable<Vocabulary>> ReadAll()
    {
        var results = await _unitOfWork.Vocabularies.All();
        return results;
    }

    public Task<IEnumerable<Vocabulary>> ReadRange(int skip, int take)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Vocabulary>> ReadRandomList()
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteSingle(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateSingle(Vocabulary vocabulary)
    {
        await _unitOfWork.Vocabularies.Upsert(vocabulary);
        await _unitOfWork.CompleteAsync();
        return true;
    }
}
