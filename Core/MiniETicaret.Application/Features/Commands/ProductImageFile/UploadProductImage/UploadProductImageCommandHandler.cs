using MediatR;
using MiniETicaret.Application.Abstractions.Storage;
using MiniETicaret.Application.Repositories;

namespace MiniETicaret.Application.Features.Commands.ProductImageFile.UploadProductImage;


public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
{
    readonly IStorageService _storageService;
    readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
    readonly IProductReadRepository _productReadRepository;

    public UploadProductImageCommandHandler(IProductImageFileWriteRepository productImageFileWriteRepository, IProductReadRepository productReadRepository, IStorageService storageService)
    {
        _productImageFileWriteRepository = productImageFileWriteRepository;
        _productReadRepository = productReadRepository;
        _storageService = storageService;
    }

    public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
    {
        List<(string fileName, string pathOrContainerName)> result = await 
            _storageService.UploadAsync("photo-images", request.Files);

        Domain.Entities.Product product = await _productReadRepository.GetByIdAsync(request.Id);

        await _productImageFileWriteRepository.AddRangeAsync(result.Select(r => new 
            Domain.Entities.ProductImageFile
        {
            FileName = r.fileName,
            Path = r.pathOrContainerName,
            Storage = _storageService.StorageName,
            Products = new List<Domain.Entities.Product>() {product}
        }).ToList());

        await _productImageFileWriteRepository.SaveAsync();
        return new ();
    }
}


