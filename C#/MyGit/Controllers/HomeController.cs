using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyGit.Models;

// https://localhost:44361/Home/MyForm?data=AAmojeDataZZ&handler=AAmujHandlerZZ&jmeno=AAmujHudecZZ
// https://localhost:44361/?data=AAmojeDataZZ&handler=AAmujHandlerZZ&jmeno=Hudce
// ASP.Net MVC How to pass data from view to controller - https://is.gd/iYUiP2


namespace MyGit.Controllers {
  /// <summary>
  /// Tento kontroler je spuštěn když uživatel otevře v prohlížeči naši aplikaci
  /// </summary>
  public class HomeController : Controller {
    // Konfigurace   
    const string MyAppName = "MyGit v1.04 - Control Version System";
    //    const string MyDirC = @"c:\aaC\04";
    const string MyDirC = @"./mygitdata/";

    const string br = "<br />";
    // Promenne 
    string MyDir = "";
    StringWriter LogTxt = new StringWriter();

    // Index - Akce se spustí v případě, když uživatel vstoupí a neodeslal formulář
    /// <summary>
    /// Akce se spustí v případě, když uživatel vstoupí a neodeslal formulář
    /// </summary>
    /// <param name="jmeno">Pokusný parametr na vyzkoušení metody GET, nemusí se vyplňovat</param>
    /// <returns></returns>
    public IActionResult Index(string data, string handler, string jmeno) {
      // zzz
      ViewData["Title"] = MyAppName;
      // zzz
      MyForm myform = new MyForm();
      myform.tbPath = @"\dir1\";
      myform.tbPath = @"/dir1/";
      // Log 
      // console writeln - https://is.gd/VfmeSa
      // Redirect Console.WriteLine() output to string - https://is.gd/UnYFnR
      Console.SetOut(LogTxt);
      Console.SetError(LogTxt);
      // Console.WriteLine("Hello world.");
      Console.WriteLine(MyAppName);
      // zobrazeni pro ladeni 
      ViewData["tbPath"] = myform.tbPath;
      ViewData["tbLog"] = myform.tbLog;
      ViewData["data"] = data;
      ViewData["handler"] = handler;
      ViewData["Message"] = "index-Message.";
      ViewData["Log"] = "index-Log.";
      ViewData["Jmeno"] = jmeno;
      ViewBag.Jmeno = jmeno;
      // Log 
      string result = LogTxt.ToString();
      ViewBag.Log = result.Replace("\n", br);
      return View(myform);
    }

    /// <summary>
    /// Akce se spustí v případě, když uživatel vstoupí a odeslal formulář
    /// </summary>
    /// <param name="kalkulacka">ViewModel vyplněný daty tak, jak jej uživatel vyplnil</param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult Index(MyForm myform, string data, string handler, string jmeno) {
      // Log 
      // console writeln - https://is.gd/VfmeSa
      // Redirect Console.WriteLine() output to string - https://is.gd/UnYFnR
      Console.SetOut(LogTxt);
      Console.SetError(LogTxt);
      Console.WriteLine(MyAppName);
      // Dir 
      MyDir = MyDirC + myform.tbPath;
      Console.WriteLine("MyDir:" + MyDir);
      // Cara();
      // Jsou data vyplněná validně?
      if (ModelState.IsValid) {
        // tady asi nic 
        // myform.Vypocitej();
      }
      ViewData["Title"] = MyAppName;
      // zobrazeni pro ladeni 
      ViewData["tbPath"] = myform.tbPath;
      ViewData["tbLog"] = myform.tbLog;
      ViewData["data"] = data;
      // Tasks 
      if (data == "P2L") {
        // P2L
        string ss = myform.tbPath + "- line1<br />line2";
        myform.tbLog = ss;
        ViewBag.Log = ss;
      } else if (data == "Check") {
        // Check
        if (Directory.Exists(MyDir)) {
          GitUtilt.DoCsvChk(MyDir);
        } else {
          Console.WriteLine(GitUtilt.mb("Adresář: " + MyDir + " nenalezen!"));
        }
      } else if (data == "Init") {
        // Init
        if (Directory.Exists(MyDir)) {
          GitUtilt.DoCsvCrt(MyDir);
        } else {
          Console.WriteLine(GitUtilt.mb("Adresář: " + MyDir + " nenalezen!"));
        }
      } else if (data == "Versions") {
        // Versions
        if (Directory.Exists(MyDir)) {
          GitUtilt.DoCsvVer(MyDir);
        } else {
          Console.WriteLine(GitUtilt.mb("Adresář: " + MyDir + " nenalezen!"));
        }
      } else if (data == "Dirs") {
        // Dirs
        if (Directory.Exists(MyDir)) {
          Console.WriteLine("\n\t Vypis adresare a podadresaru: " + MyDir);
          GitUtilt.FullDirList(MyDir);
        } else {
          Console.WriteLine(GitUtilt.mb("Adresář: " + MyDir + " nenalezen!"));
        }
      } else if (data == "Touch") {
        // Touch
        GitUtilt.DoTouch(MyDir);
      } else if (data == "Delete") {
        // Delete
        GitUtilt.DoDelete(MyDir);
      } else if (data == "SetData") {
        // SetData
        myform.tbPath = @"\dir1\";
        myform.tbLog = "l2";
      } else if (data == "Clear") {
        // Clear
        myform.tbPath = "";
        myform.tbLog = "";
      } else {
        // zobrazeni pro ladeni 
        ViewData["tbPath"] = myform.tbPath;
        ViewData["tbLog"] = myform.tbLog;
        ViewData["data"] = data;
        ViewData["handler"] = handler;
        ViewData["Message"] = "zzzIndex.";
        ViewData["Log"] = "zzzZacatek.";
        ViewData["Log"] = "zzzZacatek.";
        ViewData["Jmeno"] = jmeno;
        ViewBag.Jmeno = jmeno;

      }
      // Log 
      string result = LogTxt.ToString();
      ViewBag.Log = result.Replace("\n", br);
      return View(myform);
    }

    public IActionResult About() {
      ViewData["Message"] = "zzAbotzz Your application description page.";
      return View();
    }

    public IActionResult Contact() {
      ViewData["Message"] = "zzKontzz  Your contact page.";
      return View();
    }

    public IActionResult MyForm(string data, string handler, string jmeno) {
      MyForm myform = new MyForm();
      // zobrazeni pro ladeni 
      ViewData["tbPath"] = myform.tbPath;
      ViewData["tbLog"] = myform.tbLog;
      ViewData["data"] = data;
      ViewData["handler"] = handler;
      ViewData["Message"] = "index-Message.";
      ViewData["Log"] = "index-Log.";
      ViewData["Jmeno"] = jmeno;
      ViewBag.Jmeno = jmeno;
      return View(myform);
    }

    [HttpPost]
    public IActionResult MyForm(MyForm myform, string data, string handler, string jmeno) {
      // Jsou data vyplněná validně?
      if (ModelState.IsValid) {
        // tady asi nic 
        //myform.Vypocitej();
      }
      ViewData["Title"] = MyAppName;
      // zobrazeni pro ladeni 
      ViewData["tbPath"] = myform.tbPath;
      ViewData["tbLog"] = myform.tbLog;

      if (data == "mojeData") {
        ViewData["data"] = MyAppName;
        myform.tbLog = MyAppName;
      } else if (data == "test") {
        myform.tbPath = MyDirC;
        myform.tbLog = MyDirC;
        ViewData["data"] = MyDirC;
      } else {
        // zobrazeni pro ladeni 
        ViewData["tbPath"] = myform.tbPath;
        ViewData["tbLog"] = myform.tbLog;
        ViewData["data"] = data;
        ViewData["handler"] = handler;
        ViewData["Message"] = "zzzIndex.";
        ViewData["Log"] = "zzzZacatek.";
        ViewData["Log"] = "zzzZacatek.";
        ViewData["Jmeno"] = jmeno;
        ViewBag.Jmeno = jmeno;

      }
      return View(myform);
    }

    // udelej caru do logu
    public void Cara() {
      Console.WriteLine("\n===============================================================");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
