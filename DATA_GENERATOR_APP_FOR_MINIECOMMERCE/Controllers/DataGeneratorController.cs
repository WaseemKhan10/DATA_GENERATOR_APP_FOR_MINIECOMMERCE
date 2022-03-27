using DATA_GENERATOR_APP_FOR_MINIECOMMERCE.Models;
using DATA_GENERATOR_APP_FOR_MINIECOMMERCE.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DATA_GENERATOR_APP_FOR_MINIECOMMERCE.Controllers
{
    public class DataGeneratorController : Controller
    {
        private readonly IDataGenerator<ProductModel> _contactsGeneratorService;
        public IProductRepository _ProductRepository { get; }

        public DataGeneratorController(IDataGenerator<ProductModel> dataGeneratorService, IProductRepository ProductRepository)
        {
            _contactsGeneratorService = dataGeneratorService;
            _ProductRepository = ProductRepository;
        }
        [HttpGet]
        public  ActionResult GenerateData()
        {
            return View();  
        }
       
        [HttpPost]
        public async Task<string> GenerateData(IFormFile image)
        {

            byte[] img = null;
            if (image != null)
            {
                if (image.Length > 0)
                {
                    var fileName = Path.GetFileName(image.FileName);
                    var fileExtension = Path.GetExtension(fileName);
                    var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);
                    using (var target = new MemoryStream())
                    {
                        image.CopyTo(target);
                        img = target.ToArray();
                    }
                }
            }
            var data = _contactsGeneratorService.Collection(100);
            var product = await _ProductRepository.GenerateProducts(data, img);
            return "Products Generated Successfully";
        }
    }
}
