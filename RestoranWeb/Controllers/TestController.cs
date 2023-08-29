using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace RestoranWeb.Controllers
{
    public class TestController : Controller
    {
        public IActionResult ExtensionMethod()
        {
            string obj = "some data in string";

            //obj =   string.Join(" ", obj.Split(' ').Select(m => m[..1].ToUpper() + m[1..]));
            //  CultureInfo.CurrentCulture.TextInfo.ToTitleCase(obj);
          obj =  obj.ToTitleCase();
            obj.AlphaNumeric();
            
            return View();
        }
    }
}
