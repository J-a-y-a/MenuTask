using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MenuTask.Models;
using System.Data.Entity;

namespace MenuTask.Controllers
{
    public class HomeController : Controller
    {
        public jayaEntities db = new jayaEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetInfo(string TCM_NAME)
        {
            var menu = (from USE in db.USER_PERMISSIONS
                        join TCM in db.TELE_CALLER_MASTER on USE.UserId equals TCM.TCM_ID into ps_jointable
                        from TCM in ps_jointable.DefaultIfEmpty()
                        join ME in db.Menus on USE.MenuId equals ME.MenuId into pse_jointable
                        from ME in pse_jointable.DefaultIfEmpty()
                        select new
                        {
                            USE.ID,
                            USE.UserId,
                            USE.MenuId,
                            USE.ViewPermission,
                            USE.AddPermission,
                            USE.EditPermission,
                            USE.DeletePermission,
                            TCM.TCM_NAME,
                            TCM.TCM_ID,
                            ME.MenuName
                        }).ToList();
            if (TCM_NAME != "" && TCM_NAME != null)
            {
                menu = menu.Where(e => e.TCM_ID == TCM_NAME).ToList();
            }
            return Json(new { data = menu }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Save(string[] objuser, string user)
        {
            int i = 0;
            try
            {
                foreach (var Arr in objuser)
                {
                    string[] data = Arr.Split(',');
                    var MenuId = Convert.ToInt32(data[1]);
                    var emp = (from E in db.USER_PERMISSIONS
                               where E.UserId == user && E.MenuId == MenuId
                               select E).FirstOrDefault();
                    if (emp != null)
                    {
                        emp.MenuId = Convert.ToInt32(data[1]);
                        emp.UserId = user;
                        emp.ViewPermission = Convert.ToBoolean(data[2]);
                        emp.AddPermission = Convert.ToBoolean(data[3]);
                        emp.EditPermission = Convert.ToBoolean(data[4]);
                        emp.DeletePermission = Convert.ToBoolean(data[5]);
                        db.Entry(emp).State = EntityState.Modified;
                    }
                    else
                    {
                        USER_PERMISSIONS uSER = new USER_PERMISSIONS();
                        uSER.MenuId = Convert.ToInt32(data[1]);
                        uSER.UserId = user;
                        uSER.ViewPermission = Convert.ToBoolean(data[2]);
                        uSER.AddPermission = Convert.ToBoolean(data[3]);
                        uSER.EditPermission = Convert.ToBoolean(data[4]);
                        uSER.DeletePermission = Convert.ToBoolean(data[5]);
                        db.USER_PERMISSIONS.Add(uSER);
                    }
                    i = db.SaveChanges();
                }
                if (i > 0)
                {
                    return Json(data: "Added successfully", behavior: JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(data: "NotAdded", behavior: JsonRequestBehavior.AllowGet);
                }
             //   return View();
            }
          // return View();
            catch (Exception ex)
            {
                throw (ex);
            }
            
        }
       

        public ActionResult Menunames()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var menu = db.Menus.ToList();
            return Json(new { data = menu }, JsonRequestBehavior.AllowGet);
        }


    }
}





