using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Models
{
    class Task
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TaskStatusId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueTo { get; set; }
    }
}
