using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace MyGit.Models {
  public class MyForm {
    [Display(Name = "Path or File:")]
    public string tbPath { get; set; }
    [Display(Name = "Log:")]
    public string tbLog { get; set; }
    // konstruktor
    /*
        public MyForm() {
          tbPath = "init-tbPath";
          tbLog = "init-tbLog";

        }*/

    /// <summary>
    /// Provede výpočet
    /// </summary>
    public void Vypocitej() {
      tbLog = tbPath;
      tbPath = "nnnew-Vypocitej";
    }
  }

}
