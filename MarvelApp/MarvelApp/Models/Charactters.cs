﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarvelApp.Models
{
    public class Charactters
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }

    }
}
