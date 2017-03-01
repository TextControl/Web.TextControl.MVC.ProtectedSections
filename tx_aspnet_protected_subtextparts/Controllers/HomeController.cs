using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TXTextControl;

namespace tx_aspnet_protected_subtextparts.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string InsertSubTextPart(string Name, string BinaryDocument, bool Protected)
        {
            byte[] data = null;

            // create a temporary ServerTextControl to create the
            // SubTextPart
            using (ServerTextControl tx = new ServerTextControl())
            {
                tx.Create();

                // load the Selection from the Web.TextControl
                tx.Load(Convert.FromBase64String(BinaryDocument),
                    BinaryStreamType.InternalUnicodeFormat);

                tx.SelectAll();
                int iTextLength = tx.Selection.Length;

                // create a new SubTextPart over the complete text
                SubTextPart part = new SubTextPart("txmb_" + Name, 
                    Convert.ToInt32(Protected), 1, iTextLength);
                tx.SubTextParts.Add(part);
                
                // save the complete document
                tx.SelectAll();
                tx.Selection.Save(out data, BinaryStreamType.InternalUnicodeFormat);
            }

            // return the Selection as a Base64 encoded string
            return Convert.ToBase64String(data);
        }
    }
}