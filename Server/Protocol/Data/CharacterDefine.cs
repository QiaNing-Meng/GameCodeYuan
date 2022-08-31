using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Data
{
    public class CharacterDefine
    {
        public int TID { get; set; }
        public string Name { get; set; }
        public Define.CharacterClass Class { get; set; }
        public string Resource { get; set; }
        public string Description { get; set; }

    }
}
