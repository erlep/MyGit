// 
//   GitUtilt.cs - prace s verzovacim systemem pouzivajici soubory ".MyGit.Cvs.Csv"
//
using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper; // CsvHelper - Install-Package CsvHelper - https://joshclose.github.io/CsvHelper/
using System.Windows;

namespace MyGit.Models {
  public static class GitUtilt {
    // Konfigurace   
    public const int PrvniV = 1;
    public const string MyGitFlNm = ".MyGit.Cvs.Csv";
    public const string Hvezda = "*";
    // folders - seznam adresaru pro prochazeni  
    public static List<DirectoryInfo> folders = new List<DirectoryInfo>();

    // FullDirList - vypis vsech podadresaru a souboru 
    static public void FullDirList(string MyDir) {
      DirectoryInfo dir = new DirectoryInfo(MyDir);
      // dir 
      Console.WriteLine("Directory {0}", dir.FullName);
      string searchPattern = GitUtilt.Hvezda;
      // list the files
      try {
        foreach (FileInfo f in dir.GetFiles(searchPattern)) {
          // file
          Console.WriteLine("File {0}", f.FullName);
        }
      } catch {
        Console.WriteLine("Directory {0}  \n could not be accessed!!!!", dir.FullName);
        return;  // We alredy got an error trying to access dir so dont try to access it again
      }

      // process each directory
      // If I have been able to see the files in the directory I should also be able 
      // to look at its directories so I dont think I should place this in a try catch block
      foreach (DirectoryInfo d in dir.GetDirectories()) {
        folders.Add(d);
        FullDirList(d.FullName);
      }
    }

    // Dir2List - vypis vsech podadresaru a souboru 
    public static void Dir2List(DirectoryInfo dir, string searchPattern) {
      // Console.WriteLine("Directory {0}", dir.FullName);
      // process each directory
      foreach (DirectoryInfo d in dir.GetDirectories()) {
        folders.Add(d);
        Dir2List(d, searchPattern);
      }
    }

    // CsvCrt - vytvoreni souboru ".MyGit.Cvs.Csv" pro adresar pol, inicializace: Verze=1  
    public static void CsvCrt(DirectoryInfo pol, string searchPattern) {
      Console.WriteLine("adresář: {0} - nový adresář, žádné změny", pol.Name);
      // Zaznamy 
      List<GitFormat> records = new List<GitFormat>();
      GitFormat record = new GitFormat();
      // adresare 
      foreach (DirectoryInfo d in pol.GetDirectories()) {
        record = new GitFormat(d, PrvniV);
        records.Add(record);
      }
      // soubory 
      foreach (FileInfo f in pol.GetFiles(searchPattern)) {
        // Console.WriteLine("File: " + f.FullName + " Name" + f.Name);
        if (f.Name != MyGitFlNm) {
          record = new GitFormat(f, PrvniV);
          records.Add(record);
        }
      } // for each
      // ulozeni souboru ".MyGit.Cvs.Csv"
      //Console.WriteLine("Git File: " + pol.FullName + @"\" + MyGitFlNm);
      using (var writer = new StreamWriter(pol.FullName + @"\" + MyGitFlNm))
      using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) {
        csv.WriteRecords(records);
      }
    }

    // CsvCti - cteni souboru ".MyGit.Cvs.Csv" pro adresar pol, vrati dict s obsahem csv souboru 
    public static SortedDictionary<string, GitFormat> CsvCti(DirectoryInfo pol, string searchPattern) {
      // Console.WriteLine("CsvCti : Directory {0}", pol.Name);
      // soubor ".MyGit.Cvs.Csv" v akt. adresari, kdyz neexistuje tak jej vytvor
      string FlNm = pol.FullName + @"\" + MyGitFlNm;
      if (!(File.Exists(FlNm))) {
        // vytvor soubor 
        CsvCrt(pol, Hvezda);
      }
      // dict 
      SortedDictionary<string, GitFormat> DictGit = new SortedDictionary<string, GitFormat>(); // dict <string, GitFormat>
      using (var reader = new StreamReader(FlNm))
      using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) {
        var records = csv.GetRecords<GitFormat>();
        // Vytvoreni Dictionary 
        foreach (GitFormat f in records) {
          // Console.WriteLine("Type: " + f.Type + "  Name: " + f.Name + "  Size: " + f.Size + "  DateTime: " + f.DateTime + "  Version: " + f.Version);
          DictGit.Add(f.Name, f);
        }
      }
      return DictGit;
    }

    // CsvChk - porovna 1 soubor, "GitFormat record" vs. "DictGit[record.Name]"
    public static void CsvChk(GitFormat record, List<GitFormat> recsNew, List<GitFormat> recsDel,
      List<GitFormat> recsUpd, List<GitFormat> recsVer, SortedDictionary<string, GitFormat> DictGit) {
      // neni jmeno ".MyGit.Cvs.Csv"
      if (record.Name != MyGitFlNm) {
        // kontrola vuci dict 
        if (DictGit.ContainsKey(record.Name)) {
          GitFormat recDict = DictGit[record.Name];
          // vezmu revizi z dict 
          record.Version = recDict.Version;
          // klic existuje 
          if (record.Equals(recDict)) {
            // zaznam nezmenen 
          } else {
            // navyseni verze 
            record.Plus1();
            recsVer.Add(record);
          }
          // Pridani a zruseni ze seznamu nalezenych 
          recsUpd.Add(record);
          DictGit.Remove(record.Name);
        } else {
          // novy adresar 
          recsNew.Add(record);
          recsUpd.Add(record);
        }
      }

    }

    // CsvChk - porovnani souboru ".MyGit.Cvs.Csv" vuci adresari 
    public static void CsvChk(DirectoryInfo pol, string searchPattern) {
      //Console.WriteLine("CsvChk : Directory {0}", pol.Name);
      // CsvCti - Cteni souboru ".MyGit.Cvs.Csv"
      SortedDictionary<string, GitFormat> DictGit = CsvCti(pol, Hvezda);
      // Zaznamy 
      List<GitFormat> recsNew = new List<GitFormat>(); // nove
      List<GitFormat> recsDel = new List<GitFormat>(); // smazane
      List<GitFormat> recsUpd = new List<GitFormat>(); // pro ulozeni aktualizace
      List<GitFormat> recsVer = new List<GitFormat>(); // Verze++ 
      GitFormat record = new GitFormat();
      // adresare porovnej
      foreach (DirectoryInfo d in pol.GetDirectories()) {
        record = new GitFormat(d, PrvniV);
        CsvChk(record, recsNew, recsDel, recsUpd, recsVer, DictGit);
      }
      // soubory porovnej
      foreach (FileInfo f in pol.GetFiles(searchPattern)) {
        record = new GitFormat(f, PrvniV);
        CsvChk(record, recsNew, recsDel, recsUpd, recsVer, DictGit);
      }
      /* 
        [A] = added (nový soubor)
        [M] = modified (změněný soubor)
        [D] = deleted (odstraněný soubor)
       */
      // Protokol
      if (!((recsNew.Count == 0) && (recsVer.Count == 0) && (DictGit.Count == 0))) {
        Console.WriteLine("\n\t adresář: " + pol.Name + " - změny");
        // seznam novych 
        if (recsNew.Count > 0) { Console.WriteLine("\n\t [A] = added (nový soubor):"); };
        foreach (GitFormat f in recsNew) {
          Console.WriteLine("[A] Type: " + f.Type + "  Name: " + f.Name + "  Size: " + f.Size + "  DateTime: " + f.DateTime + "  Version: " + f.Version);
        }
        // seznam zmenenych  
        if (recsVer.Count > 0) { Console.WriteLine("\n\t [M] = modified (změněný soubor):"); }
        foreach (GitFormat f in recsVer) {
          Console.WriteLine("[M] Type: " + f.Type + "  Name: " + f.Name + "  Size: " + f.Size + "  DateTime: " + f.DateTime + "  Version: " + f.Version);
        }   
        // seznam smazanych 
        if (DictGit.Count > 0) { Console.WriteLine("\n\t [D] = deleted (odstraněný soubor):"); }
        foreach (KeyValuePair<string, GitFormat> entry in DictGit) {
          // do something with entry.Value or entry.Key
          GitFormat f = entry.Value;
          Console.WriteLine("[D] Type: " + f.Type + "  Name: " + f.Name + "  Size: " + f.Size + "  DateTime: " + f.DateTime + "  Version: " + f.Version);
        }       
        /*
        // seznam aktualnich  
        Console.WriteLine("\n\t Seznam aktualnich:");
        foreach (GitFormat f in recsUpd) {
          Console.WriteLine("Type: " + f.Type + "  Name: " + f.Name + "  Size: " + f.Size + "  DateTime: " + f.DateTime + "  Version: " + f.Version);
        }  */
        // ulozeni aktualizace do  souboru ".MyGit.Cvs.Csv"
        // Console.WriteLine("Git File: " + pol.FullName + @"\" + MyGitFlNm);
        using (var writer = new StreamWriter(pol.FullName + @"\" + MyGitFlNm))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) {
          csv.WriteRecords(recsUpd);
        }
      } else {
        Console.WriteLine("\t adresář: " + pol.Name + " - žádná změna");

      }
    }

    // DoDirList - naplni adresare do - List<DirectoryInfo> folders
    public static void DoDirList(string MyDir) {
      // Console.WriteLine("Directory {0}", dir.FullName);
      // Seznam Souboru a adresaru 
      DirectoryInfo di = new DirectoryInfo(MyDir);
      // promazani 
      folders.Clear();
      // pridam korenovy 
      folders.Add(di);
      Dir2List(di, GitUtilt.Hvezda);
    }

    // DoCsvCrt - vytvoreni souboru ".MyGit.Cvs.Csv" pro adresar pol, Verze=1  
    public static void DoCsvCrt(string MyDir) {
      Console.WriteLine("\n\t Inicializace kontroly verzi pro: " + MyDir);
      // MessageBox.Show("BtCheck_Click");
      // dirs 
      DoDirList(MyDir);
      // CsvCrt - vytvoreni souboru ".MyGit.Cvs.Csv" pro adresar pol, Verze=1  
      foreach (DirectoryInfo pol in folders) { CsvCrt(pol, GitUtilt.Hvezda); };
    }

    // DoCsvChk - porovnani souboru ".MyGit.Cvs.Csv" vuci adresari 
    public static void DoCsvChk(string MyDir) {
      Console.WriteLine("\n\t Kontrola verzi pro: " + MyDir );
      // MessageBox.Show("BtCheck_Click");
      // dirs 
      DoDirList(MyDir);
      // CsvChk - porovnani souboru ".MyGit.Cvs.Csv" vuci adresari 
      foreach (DirectoryInfo pol in folders) { CsvChk(pol, GitUtilt.Hvezda); };
    }

    // DoTouch - update souboru   
    public static void DoTouch(string MyDir) {
      // Console.WriteLine("update souboru: " + MyDir);
      //Create the file if it doesn't exist
      try {
        if (!File.Exists(MyDir)) {
          using (var sw = File.CreateText(MyDir)) {
            sw.WriteLine("Hello, I'm a new file!!");
            // You don't need this as the using statement will close automatically, but it does improve readability
            sw.Close();
          }
          Console.WriteLine("Soubor: " + MyDir + " vytvoren.");
        } else {
          using (var sw = File.AppendText(MyDir)) {
            sw.WriteLine("The next line!");
          }
          Console.WriteLine("Soubor: " + MyDir + " aktualizovan.");
        }
      } catch {
        Console.WriteLine("Soubor: " + MyDir + " nelze aktualizovat!");
        return;  // We alredy got an error trying to access dir so dont try to access it again
      }
    }

    // DoDelete - smazani souboru   
    public static void DoDelete(string MyDir) {
      // Console.WriteLine("smazani souboru: " + MyDir);
      if (File.Exists(MyDir)) {
        File.Delete(MyDir);
        Console.WriteLine("Soubor: " + MyDir + " smazan.");
      } else {
        Console.WriteLine("Soubor: " + MyDir + " nenalezen!");
      }
    }


  }
}
