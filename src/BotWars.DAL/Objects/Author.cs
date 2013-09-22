using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotWars.DAL.Objects
{
    public class Author
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string FolderName { get; set; }
    }
}
