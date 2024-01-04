﻿using AutoMapper;
using EnlightenmentApp.BLL.Entities;
using EnlightenmentApp.BLL.Interfaces.Services;
using EnlightenmentApp.DAL.Entities;
using EnlightenmentApp.DAL.Interfaces.Repositories;

namespace EnlightenmentApp.BLL.Services
{
    public class SectionService : GenericService<Section, SectionEntity>, ISectionService
    {
        public SectionService(ISectionRepository repository, IMapper mapper) : base(repository, mapper)
        {

        }
    }
}
