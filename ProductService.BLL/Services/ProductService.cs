using AutoMapper;
using Microsoft.Extensions.Logging;
using ProductService.BLL.Exceptions;
using ProductService.Contracts.DTOs;
using ProductService.Contracts.Interfaces;
using ProductService.DAL.Entities;
using ProductService.DAL.Interfaces;

namespace ProductService.BLL.Services
{
    public class ProductService(IProductRepository productRepository, IMapper mapper, ILogger logger) : IProductService
    {
        /// <summary>
        /// Создает продукт.
        /// </summary>
        /// <param name="createProductDTO">DTO с данными для создания продукта.</param>
        /// <param name="ct">Токен отмены операции.</param>
        /// <returns>DTO продукта(+Id), если создан.</returns>
        public async Task<ProductDTO> CreateAsync(CreateProductDTO createProductDTO, CancellationToken ct)
        {
            logger.LogInformation("[ProductService]CreateAsync TRY");

            var mappedProduct = mapper.Map<Product>(createProductDTO);
            var res = await productRepository.CreateAsync(mappedProduct, ct);

            logger.LogInformation("[ProductService]CreateAsync CORRECT");

            return mapper.Map<ProductDTO>(res);
        }

        /// <summary>
        /// Удаляет продукт по идентификатору.
        /// </summary>
        /// <param name="id">ID продукта.</param>
        /// <param name="ct">Токен отмены операции.</param>
        /// <exception cref="NotFoundException"></exception>
        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            logger.LogInformation("[ProductService]DeleteAsync {id} TRY", id);

            await GetByIdAsync(id, ct);
            await productRepository.DeleteAsync(id, ct);

            logger.LogInformation("[ProductService]DeleteAsync id={id} CORRECT", id);
        }

        /// <summary>
        /// Возвращает все продукты.
        /// </summary>
        /// <remarks><b>Не рекомендуется</b> Используй: <see cref="GetPagedResult"/> </remarks>
        /// <param name="ct">Токен отмены операции.</param>
        /// <returns>Весь список продуктов.</returns>
        public async Task<List<ProductDTO>> GetAllAsync(CancellationToken ct)
        {
            logger.LogInformation("[ProductService]GetAllAsync TRY");

            var res = mapper.Map<List<ProductDTO>>(await productRepository.GetAllAsync(ct));

            logger.LogInformation("[ProductService]GetAllAsync CORRECT");
            return res;
        }

        /// <summary>
        /// Получает продукт по идентификатору.
        /// </summary>
        /// <param name="id">ID продукта.</param>
        /// <param name="ct">Токен отмены операции.</param>
        /// <returns>DTO продукта, если найден; иначе выбрасывает <c>NotFoundException</c>.</returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<ProductDTO> GetByIdAsync(int id, CancellationToken ct)
        {
            logger.LogInformation("[ProductService]GetByIdAsync TRY");

            var product = await productRepository.GetByIdAsync(id, ct);
            if (product == null)
                throw new NotFoundException($"[ProductService]GetByIdAsync id={id}");

            var mappedProduct = mapper.Map<ProductDTO>(product);

            logger.LogInformation("[ProductService]GetByIdAsync CORRECT");
            return mappedProduct;
        }

        /// <summary>
        /// Получает некоторую часть продуктов.
        /// </summary>
        /// <param name="page">Номер страницы.</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <param name="ct">Токен отмены операции.</param>
        /// <returns>"Страницу" с продуктами (метаданные + продукты).</returns>
        public async Task<PagedResult<ProductDTO>> GetPagedResult(int page, int pageSize, CancellationToken ct)
        {
            logger.LogInformation("[ProductService]GetPagedResult page={page} pageSize={pageSize} TRY", page, pageSize);

            var (resItems, totalCount) = await productRepository.GetPagedAsync(page, pageSize, ct);
            var mappedList = mapper.Map<List<ProductDTO>>(resItems);
            var pagedResult = new PagedResult<ProductDTO>()
            {
                Items = mappedList,
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            logger.LogInformation(
                "[ProductService]GetPagedResult page={page} pageSize={pageSize} CORRECT totalCount = {totalCount}", page, pageSize, totalCount);

            return pagedResult;
        }

        /// <summary>
        /// Обновляет продукт.
        /// </summary>
        /// <param name="updateProductDTO">DTO с данными для обновления продукта(может быть не полным)</param>
        /// <param name="ct">Токен отмены операции.</param>
        /// <returns>DTO продукта, если обновлен; иначе выбрасывает <c>NotFoundException</c>.</returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<ProductDTO> UpdateAsync(UpdateProductDTO updateProductDTO, CancellationToken ct)
        {
            logger.LogInformation("[ProductService]UpdateAsync TRY");

            var product = mapper.Map<Product>(await GetByIdAsync(updateProductDTO.Id, ct)); // тут проверяется его наличие и результат сразу маппится
            mapper.Map(updateProductDTO, product);

            var res = mapper.Map<ProductDTO>(await productRepository.UpdateAsync(product, ct));

            logger.LogInformation("[ProductService]UpdateAsync CORRECT");
            return res;
        }

        /// <summary>
        /// Ищет продукт по идентификатору.
        /// </summary>
        /// <param name="id">ID продукта.</param>
        /// <param name="ct">Токен отмены операции.</param>
        /// <returns>DTO продукта, если найден; иначе <c>null</c>.</returns>
        public async Task<ProductDTO?> FindByIdAsync(int id, CancellationToken ct)
        {
            logger.LogInformation("[ProductService]FindByIdAsync TRY");

            var product = await productRepository.GetByIdAsync(id, ct);
            if (product == null)
            {
                logger.LogInformation("[ProductService]FindByIdAsync NULL");
                return null;
            }

            var mappedProduct = mapper.Map<ProductDTO>(product);

            logger.LogInformation("[ProductService]FindByIdAsync CORRECT");

            return mappedProduct;
        }
    }
}
