﻿namespace EnlightenmentApp.DAL.Entities
{
    public class SectionEntity
    {
        #nullable disable
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ChapterEntity> Chapters { get; set; }
        public int ModuleId { get; set; }
        public ModuleEntity Module { get; set; }
    }
}