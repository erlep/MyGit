using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Presmerovani  Console.WriteLine() do TextBoxu
// What is a good way to direct console output to Text-box in Windows Form? - https://is.gd/VfmeSa

namespace MyGit {
  public class TextBoxWriter : TextWriter {
    string _output = null;

    public TextBoxWriter(string output) {
      _output = output;
    }

    public override void Write(char value) {
      base.Write(value);
      _output +=(value.ToString());
    }

    public override Encoding Encoding {
      get { return System.Text.Encoding.UTF8; }
    }
  }

}
