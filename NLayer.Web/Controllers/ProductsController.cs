using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _services;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public ProductsController(IProductService services, ICategoryService categoryService, IMapper mapper)
        {
            _services = services;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _services.GetProductWithCategory());
        }

        [HttpGet]
        public async Task<IActionResult> Save() //product eklerken categoryde eklemem lazım. o yüzden servisini implemente ettim
        //kategori servisim olmasaydı IService genericle beraber kategori verirdim tabisi 
        {
            var categories = await _categoryService.GetAllAsync();

            //List<CategoryDTO> gelecek bana; neyi categoriesleri
            var categoriesDto = _mapper.Map<List<CategoryDTO>>(categories.ToList());

            //viewbagla beraber dropdownlist'i dolduracağım bir değişken tanımladım; dropdowlist olarak selectlist alıcak
            //selectlist = bana bir liste ver, dropdowndan 1 şey seçilince ben bu listeden idyi göstericem, kullanıcı name görcek.
            ViewBag.Categories = new SelectList(categoriesDto, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                await _services.AddAsnc(_mapper.Map<Product>(productDTO));
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryService.GetAllAsync();
            var categoriesDto = _mapper.Map<List<CategoryDTO>>(categories.ToList());
            ViewBag.Categories = new SelectList(categoriesDto, "Id", "Name");

            return View();
        }

        [ServiceFilter(typeof(NotFoundFilter<Product>))] //filterin ctorunda parametre aldığı için servicefilterla tanımlayacağım.
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var products = await _services.GetByIdAsync(id);

            var categories = await _categoryService.GetAllAsync();
            var categoriesDto = _mapper.Map<List<CategoryDTO>>(categories.ToList());
            
            //SelectList bana diyot ki bana seçilen değeri ver. 1 params daha ekledim.
            ViewBag.Categories = new SelectList(categoriesDto, "Id", "Name", products.CategoryId);
            return View(_mapper.Map<ProductDTO>(products));
        }

        [HttpPost]
        public async Task<IActionResult> Update (ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                await _services.UpdateAsync(_mapper.Map<Product>(productDTO));
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryService.GetAllAsync();
            var categoriesDto = _mapper.Map<List<CategoryDTO>>(categories.ToList());
            ViewBag.Categories = new SelectList(categoriesDto, "Id", "Name", productDTO.CategoryId);
            return View(productDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
           var product = await _services.GetByIdAsync(id);
            await _services.RemoveAsync(product);
            return RedirectToAction(nameof(Index));
        }


    }
}
