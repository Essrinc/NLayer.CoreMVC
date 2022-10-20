using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; } //Bu bir Navigation Property
        //NAvigation property = entitiler içindeki farklı class/entitilere referans verdiğimiz proplara denir.
    }
}
