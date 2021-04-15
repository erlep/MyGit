// 
//   GitFormat.cs - Definice formatu souboru ".MyGit.Cvs.Csv"
//
// using System; // nelze kvuli kolizi "DateTime"
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGit.Models {
  public class GitFormat : System.IEquatable<GitFormat> {
    public string Type { get; set; }
    public string Name { get; set; }
    public long Size { get; set; }
    public System.DateTime DateTime { get; set; }
    public int Version { get; set; }
    // Konstruktor
    public GitFormat() {
      Type = "";
      Name = "";        
      Size = 0;
      DateTime = System.DateTime.Now;
      Version = 0;
    }
    // Konstruktor - adresar 
    public GitFormat(DirectoryInfo d, int ver = 0) {
      Type = "D";
      Name = d.Name;
      Size = 0;
      DateTime = d.LastWriteTime;
      Version = ver;
    }
    // Konstruktor - soubor
    public GitFormat(FileInfo f, int ver = 0) {
      Type = "F";
      Name = f.Name;
      Size = f.Length;  
      DateTime = f.LastWriteTime;
      Version = ver;
    }
    // Porovnani na sekundy - https://is.gd/HkDKVC
    public static bool EqualsUpToSeconds(System.DateTime dt1, System.DateTime dt2) {
      return dt1.Year == dt2.Year && dt1.Month == dt2.Month && dt1.Day == dt2.Day &&
             dt1.Hour == dt2.Hour && dt1.Minute == dt2.Minute && dt1.Second == dt2.Second;
    }
    // metoda Equals 
    public override bool Equals(object obj) {
      return this.Equals(obj as GitFormat);
    }
    // metoda Equals - porovnani vseho krome verze, cas se dela na sekundy 
    public bool Equals(GitFormat other) {
      if (other == null)
        return false;
      return
        this.Type.Equals(other.Type) &&
        this.Name.Equals(other.Name) &&
        this.Size.Equals(other.Size) &&
        GitFormat.EqualsUpToSeconds(this.DateTime, other.DateTime);
    }
    // navyseni verze 
    public void Plus1() {
      this.Version += 1;
    }

  }
}
