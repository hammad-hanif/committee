using Committee.Interfaces;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Committee.Models
{
    public class CommitteeMembers : TableEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Number { get; set; }
    }
}
