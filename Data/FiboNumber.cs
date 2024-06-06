using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAssignmentGrcp.Data
{
    [Table("FiboNumber")]
    public class FiboNumber
    {
        [Key] public int Id { get; set; }
        public int Position { get; set; }
        public int Number { get; set; }
     }
}
