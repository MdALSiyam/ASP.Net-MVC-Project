using ArtGalleryManagementSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ArtGalleryManagementSystem.Controllers
{
    public class ArtsController : Controller
    {
        protected readonly ArtGalleryDBEntities _context = new ArtGalleryDBEntities();
        [HttpGet]
        public ActionResult Index()
        {
            var artList = _context.Arts.Include(d => d.DataRelations.Select(e => e.Exhibition)).OrderByDescending(a => a.ArtID).ToList();
            return View(artList);
        }

        public ActionResult AddExhibitions(int? id)
        {
            ViewBag.exhibition = new SelectList(_context.Exhibitions.ToList(),
                "ExhibitionID", "ExhibitionName", (id != null) ? id.ToString() : "");
            return PartialView("_AddExhibitions");
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(ArtViewModel vobj, int[] exhibitionId)
        {
            if (ModelState.IsValid)
            {
                Art art = new Art
                {
                    ArtTitle = vobj.ArtTitle,
                    ArtistName = vobj.ArtistName,
                    DateOfCreation = vobj.DateOfCreation,
                    Price = vobj.Price,
                    Discount = vobj.Discount,
                    IsAvailable = vobj.IsAvailable,
                };
                HttpPostedFileBase file = vobj.PicturePath;
                if (file != null)
                {
                    string filePath = Path.Combine("/images/", Guid.NewGuid().ToString())
                        + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath(filePath));
                    art.Picture = filePath;
                }
                foreach (int item in exhibitionId)
                {
                    DataRelation d = new DataRelation
                    {
                        Art = art,
                        ArtID = art.ArtID,
                        ExhibitionID = item
                    };
                    _context.DataRelations.Add(d);
                }
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            else { return View(); }
        }
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var art = await _context.Arts.Where(x => x.ArtID == id).FirstOrDefaultAsync();
            if (art == null)
            {
                return HttpNotFound("No Art found");
            }
            if (!string.IsNullOrEmpty(art.Picture))
            {
                string imgPath = art.Picture;
                if (System.IO.File.Exists(imgPath))
                {
                    System.IO.File.Delete(imgPath);
                }
            }
            _context.Arts.Remove(art);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Art art = _context.Arts.First(x => x.ArtID == id);
            var exhibition = _context.DataRelations.Where(x => x.ArtID == id).ToList();
            ArtViewModel vobj = new ArtViewModel
            {
                ArtID = art.ArtID,
                ArtTitle = art.ArtTitle,
                ArtistName = art.ArtistName,
                DateOfCreation = art.DateOfCreation,
                Price = art.Price,
                Discount = art.Discount,
                IsAvailable = art.IsAvailable,
                Picture = art.Picture,
            };
            if (exhibition.Count > 0)
            {
                foreach (var item in exhibition)
                {
                    vobj.ExhibitionList.Add(item.ExhibitionID);
                }
            }
            return View(vobj);

        }
        [HttpPost]
        public ActionResult Edit(ArtViewModel vobj, int[] exhibitionId)
        {
            if (ModelState.IsValid)
            {
                Art art = new Art
                {
                    ArtID = vobj.ArtID,
                    ArtTitle = vobj.ArtTitle,
                    ArtistName = vobj.ArtistName,
                    DateOfCreation = vobj.DateOfCreation,
                    Price = vobj.Price,
                    Discount = vobj.Discount,
                    IsAvailable = vobj.IsAvailable,
                };
                HttpPostedFileBase file = vobj.PicturePath;
                if (file != null)
                {
                    string imagePath = Path.Combine("/images/", Guid.NewGuid().ToString())
                        + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath(imagePath));
                    art.Picture = imagePath;
                }
                else
                    art.Picture = vobj.Picture;
                var exhibition = _context.DataRelations
                    .Where(x => x.ArtID == art.ArtID).ToList();
                foreach (var item in exhibition)
                {
                    _context.DataRelations.Remove(item);
                }
                foreach (var item in exhibitionId)
                {
                    DataRelation D = new DataRelation
                    {
                        ArtID = art.ArtID,
                        ExhibitionID = item
                    };
                    _context.DataRelations.Add(D);
                }
                _context.Entry(art).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else { return View(); }
        }

    }
}