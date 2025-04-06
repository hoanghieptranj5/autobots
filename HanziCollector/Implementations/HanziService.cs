using AutoMapper;
using CosmosRepository.Abstractions;
using CosmosRepository.Entities.HanziCollector;
using HanziCollector.Abstraction;
using HanziCollector.Models;

namespace HanziCollector.Implementations;

internal class HanziService : IHanziService
{
    private readonly ITextDocumentReader _textDocumentReader;
    private readonly ICrawlerService _crawler;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHanziDbService _hanziDbService;
    private readonly IMapper _mapper;

    public HanziService(ITextDocumentReader textDocumentReader, ICrawlerService crawlerService, IUnitOfWork unitOfWork,
        IHanziDbService hanziDbService, IMapper mapper)
    {
        _textDocumentReader = textDocumentReader;
        _crawler = crawlerService;
        _unitOfWork = unitOfWork;
        _hanziDbService = hanziDbService;
        _mapper = mapper;
    }

    public async Task<HanziFromHvDic> GetHanziInformationFromHvDic(string hanzi)
    {
        var result = await _crawler.CrawlSingle(hanzi);
        return result;
    }

    public async Task<Hanzi?> GetSingle(string id)
    {
        var result = await _unitOfWork.Hanzis.GetById(id);
        return result;
    }

    public async Task<IEnumerable<Hanzi>> GetAllInDb()
    {
        return await _hanziDbService.ReadAll();
    }

    public Task<IEnumerable<Hanzi>> GetAllInDb(int skip, int take)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Hanzi>> GetRandomList(int length, int min, int max)
    {
        return await _hanziDbService.ReadRandomHanziList(length);
    }

    public async Task<bool> Delete(string id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<string>> FindMissingIds(string filePath, int skip, int take)
    {
        throw new NotImplementedException();
    }
}
