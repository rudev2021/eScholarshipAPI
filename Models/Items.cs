using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eScholarshipAPI_Client.Models
{

    public class Rootobject
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public Items items { get; set; }
    }

    public class Items
    {
        public Item[] item { get; set; }
    }

    public class Item
    {
        public string title { get; set; }
        public string id { get; set; }
        public object isbn { get; set; }
        public object bookTitle { get; set; }
        public Authors authors { get; set; }
        public string published { get; set; }
        public Unit[] units { get; set; }
    }

    public class Authors
    {
        public int total { get; set; }
        public Author[] author { get; set; }
    }

    public class Author
    {
        public string name { get; set; }
    }

    public class Unit
    {
        public string id { get; set; }
    }

}
