using AutoMapper;
using HanziCollector.Abstraction;
using HanziCollector.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Models.HanziCollector;
using Repositories.UnitOfWork.Abstractions;

namespace HanziCollector.Implementations;

internal class HanziService : IHanziService
{
    private readonly ITextDocumentReader _textDocumentReader;
    private readonly ICrawlerService _crawler;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHanziDbService _hanziDbService;
    private readonly IMapper _mapper;

    public HanziService(ITextDocumentReader textDocumentReader, ICrawlerService crawlerService, IUnitOfWork unitOfWork, IHanziDbService hanziDbService, IMapper mapper)
    {
        _textDocumentReader = textDocumentReader;
        _crawler = crawlerService;
        _unitOfWork = unitOfWork;
        _hanziDbService = hanziDbService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<HanziFromHvDic>> ImportFromTextDocumentFile(string filePath, int takeFrom = 0, int takeTo = 100)
    {
        List<HanziFromHvDic> result;
        try
        {
            var characters = _textDocumentReader.ReadToCharArray(filePath);
            var chunk = characters
                .Skip(takeFrom)
                .Take(takeTo)
                .ToList();
            result = chunk.Select(async c => await _crawler.CrawlSingle(c))
                .Select(t => t.Result)
                .ToList();
            
            if (!result.Any())
            {
                return result;
            }

            foreach (var fromHvDic in result)
            {
                if (string.IsNullOrEmpty(fromHvDic.Pinyin) || string.IsNullOrEmpty(fromHvDic.HanViet))
                {
                    fromHvDic.Processed = false; 
                    continue;
                }
                
                var hanzi = _mapper.Map<Hanzi>(fromHvDic);
                await _unitOfWork.Hanzis.Add(hanzi);
                fromHvDic.Processed = true; 
            }

            await _unitOfWork.CompleteAsync();
        }
        catch (IOException e)
        {
            Console.WriteLine(e);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unknown Exception: {e.Message}");
            throw;
        }

        return result;
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

    public async Task<IEnumerable<Hanzi>> GetAllInDb(int skip, int take)
    {
        return await _hanziDbService.ReadRange(skip, take);
    }

    public async Task<bool> Delete(string id)
    {
        var deleted = await _unitOfWork.Hanzis.Delete(id);
        await _unitOfWork.CompleteAsync();
        return deleted;
    }
}