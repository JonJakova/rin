using System.IO;

namespace rin
{
    // Emitter object keeps track of the generated code and outputs it.
    class Emitter
    {
        private string _fullPath;
        private string _header;
        private string _code;

        public Emitter(string fullPath)
        {
            _fullPath = fullPath;
            _header = "";
            _code = "";
        }

        public void Emit(string code)
        {
            _code += code;
        }

        public void EmitLine(string code)
        {
            _code += code + '\n';
        }
        
        public void HeaderLine(string code)
        {
            _header += code + '\n';
        }

        public void WriteFile()
        {
            using (StreamWriter sw = new StreamWriter(_fullPath))
            {
                sw.Write(_header + _code);
            }
        }
    }
}