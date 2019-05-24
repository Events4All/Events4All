using System.Drawing;
using System.IO;
using System.Web.Mvc;
using ZXing;

namespace Events4All.Web.Controllers
{ 

    public class BarCodeController : Controller
    {
        /// <summary>
        /// Method to Create barcode
        /// num = random num 1-1000
        /// Example to call <img src="@Url.Action("RenderBarcode", "BarCode", new { userid =num })" width="450" height="80" />
        /// example
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ActionResult RenderBarcode(string userid)
        {
            Image img = null;
            using (var ms = new MemoryStream())
            {
                var writer = new ZXing.BarcodeWriter() { Format = BarcodeFormat.CODE_128 };
                writer.Options.Height = 80;
                writer.Options.Width = 280;
                writer.Options.PureBarcode = true;
                img = writer.Write(userid);
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return File(ms.ToArray(), "image/jpeg");
            }
        }
    }
}