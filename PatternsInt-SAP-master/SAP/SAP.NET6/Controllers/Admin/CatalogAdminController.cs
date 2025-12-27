using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAP.NET6.Data;
using SAP.NET6.Services.Catalog;
using SAP.NET6.Services.Catalog.FileProcessing;
using SAP.NET6.Services.Catalog.Implementations;
using SAP.NET6.ViewModels.Catalog;
using SAP.NET6.ViewModels.Catalog.Admin;

namespace SAP.NET6.Controllers.Admin
{
    [Authorize]
    [Route("admin/catalog")]
    public class CatalogAdminController : Controller
    {
        private ICatalogAdministration CatalogAdministration { get; }

        private ICatalogDataProvider CatalogDataProvider { get; }

        private ApplicationDbContext DbContext { get; }

        private CatalogFileProcessor CatalogFileProcessor { get; }

        public CatalogAdminController(ICatalogAdministration catalogAdministration,
            ICatalogDataProvider catalogDataProvider, ApplicationDbContext dbContext, CatalogFileProcessor catalogFileProcessor)
        {
            CatalogAdministration = catalogAdministration;
            CatalogDataProvider = catalogDataProvider;
            DbContext = dbContext;
            CatalogFileProcessor = catalogFileProcessor;
        }

        [Route("")]
        public async Task<ActionResult> Index()
        {
            var catalog = await CatalogDataProvider.GetCatalogAsync();
            return View(catalog);
        }

        [Route("node/{id}")]
        public async Task<ActionResult> Node(Guid id)
        {
            var catalog = await CatalogDataProvider.GetCatalogAsync(id);
            return View("Index", catalog);
        }

        [Route("create_category")]
        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [Route("create_category")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCategory(CreateCategoryViewModel category)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return View();
                }

                await CatalogAdministration.CreateCategoryAsync(category);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                return View();
            }
        }

        [Route("create_item")]
        public ActionResult CreateItem()
        {
            return View();
        }

        [HttpPost]
        [Route("create_item")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateItem(CreateItemViewModel item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                await CatalogAdministration.CreateItemAsync(item);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                return View();
            }
        }

        // Never implement delete of something by GET request!
        [Route("delete_item/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await CatalogAdministration.DeleteItemAsync(id);
            return RedirectToAction("Index");
        }

        [Route("upload_file")]
        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        [Route("upload_file")]
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
            if (file == null)
            {
                // It will be good to show error message for user
                return RedirectToAction("Index");
            }

            string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = Path.Combine(path, file.FileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Process the uploaded catalog file
            try
            {
                CatalogFileProcessor.ProcessCatalogFile(filePath);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                // For now, we'll just redirect with an error message
                TempData["ErrorMessage"] = $"Error processing catalog file: {ex.Message}";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}