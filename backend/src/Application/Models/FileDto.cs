﻿using Application.Extensions;

namespace Application.Models
{
    public class FileDto
    {
        public Stream Content { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }

        public string GetPathWithFileName()
        {
            string UniqueAutoGeneratedFileName = Path.GetRandomFileName();
            string shortClientSideFileNameWithoutExt = Path.GetFileNameWithoutExtension(Name).TruncateLongString(10);
            string ext = Path.GetExtension(Name);
            string basePath = "images/";

            return basePath + UniqueAutoGeneratedFileName + "_" + shortClientSideFileNameWithoutExt + ext;
        }
    }
}
