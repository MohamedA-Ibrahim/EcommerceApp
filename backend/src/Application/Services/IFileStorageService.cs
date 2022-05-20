﻿using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> UploadAsync(FileDto file);
    }
}
