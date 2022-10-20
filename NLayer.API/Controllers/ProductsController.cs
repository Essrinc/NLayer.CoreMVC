using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    public class ProductsController : CustomBaseController
    {
        private readonly IMapper _mapper;
        //controlerlar sadece serviceleri bilir. kesinlikle repoyu referans almayacaklar ctorlarında.
        private readonly IProductService _service;
        public ProductsController(IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
            _service = productService;
        }

        [HttpGet("[action]")] // getler karışmasın diye ismini verdim. = action direkt metodun ismini verdi.
        public async Task<IActionResult> GetProductsWithCategory()
        {
            //özelleştirilmişservis oldu dönüştürmeyle uğraşmadım. orda yaptım.
            return CreateActionResult(await _service.GetProductWithCategory());
        }





        [HttpGet]
        public async Task<IActionResult> All()
        {
            var products = await _service.GetAllAsync();
            //bu bir entity geriye dto dönmem lazım mapleme yaparım.
            var productsDTOs = _mapper.Map<List<ProductDTO>>(products.ToList());
            return CreateActionResult(CustomResponseDTO<List<ProductDTO>>.Success(200, productsDTOs));
        }

        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            //if (product ==null) -- her durumda bunu mu yapıcam? DUPLICATE!! OLMAZ.Service'de bul bak. 
            //{
            //    return CreateActionResult(CustomResponseDTO<ProductDTO>.Fail(400, "Bu id'ye sahip ürün bulunamadı"));
            //}
            //bu bir entity geriye dto dönmem lazım mapleme yaparım.
            var productsDTO = _mapper.Map<ProductDTO>(product);
            return CreateActionResult(CustomResponseDTO<ProductDTO>.Success(200, productsDTO));
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDTO productDTO) 
        {
            var product = await _service.AddAsnc(_mapper.Map<Product>(productDTO));//pdtoyu pye dönüştürdüm.
            //bu bir entity geriye dto dönmem lazım mapleme yaparım.
            var productsDTO = _mapper.Map<ProductDTO>(product); //tekrar dtoya dönüştürdüm.
            return CreateActionResult(CustomResponseDTO<ProductDTO>.Success(201, productsDTO));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDTO productDTO)
        {
            await _service.UpdateAsync(_mapper.Map<Product>(productDTO));//pdtoyu pye dönüştürdüm.
            return CreateActionResult(CustomResponseDTO<ProductDTO>.Success(204));
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var product = await _service.GetByIdAsync(id); //önce idyi aldım ki sileyim.

             await _service.RemoveAsync(product);

            return CreateActionResult(CustomResponseDTO<NoContentDTO>.Success(204));
        }

    }
}
